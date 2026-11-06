using Microsoft.EntityFrameworkCore;
using QuanLyCBCS.Common;
using QuanLyCBCS.Data;
using QuanLyCBCS.DTOs;
using QuanLyCBCS.Models;
using QuanLyCBCS.Services.MissionServices;

namespace QuanLyCBCS.Services
{
    public class PersonnelReportService
    {
        private readonly PersonnelReportBuilderService _reportBuilder = new();

        private readonly MissionResolverService _resolver = new();

        private readonly MissionGroupBuilderService _groupBuilder = new();

        private static ApplicationDbContext CreateContext()
            => new();

        /// <summary>
        /// Kiểm tra nhiệm vụ có phải nhiệm vụ trực tại đơn vị hay không.
        /// Các nhiệm vụ này không làm CBCS bị tính là vắng mặt.
        /// </summary>
        private static bool IsDutyAtUnit(Mission mission)
        {
            return mission.MissionType == MissionTypes.DutyCommander
                || mission.MissionType == MissionTypes.DutyOfficer;
        }

        /// <summary>
        /// Lấy các phân công bên ngoài đơn vị đang có hiệu lực đúng tại thời điểm lập báo cáo.
        /// Không lấy toàn bộ nhiệm vụ giao với ngày vì nhiệm vụ đã kết thúc/chưa bắt đầu
        /// không được tính là CBCS vắng mặt tại thời điểm chụp báo cáo.
        /// </summary>
        private async Task<List<MissionAssignmentDto>> GetMissionAssignmentsAsync(
            DateTime reportDate)
        {
            using var db = CreateContext();

            return await db.MissionPersonnel
                .AsNoTracking()
                .Include(mp => mp.Personnel)
                .Include(mp => mp.Mission)
                .Where(mp =>
                    mp.Mission != null
                    && mp.Mission.StartTime <= reportDate
                    && mp.Mission.EndTime >= reportDate
                    && mp.Mission.MissionType != MissionTypes.DutyCommander
                    && mp.Mission.MissionType != MissionTypes.DutyOfficer)
                .Select(mp => new MissionAssignmentDto
                {
                    PersonnelId = mp.PersonnelId,
                    PersonnelName = mp.Personnel != null
                        ? mp.Personnel.HoTen
                        : string.Empty,
                    MissionId = mp.Mission!.MissionId,
                    MissionType = mp.Mission.MissionType ?? "Khác",
                    MissionName = mp.Mission.MissionName ?? string.Empty,
                    Location = mp.Mission.Location ?? string.Empty,
                    Reason = mp.Mission.MissionType ?? "Khác",
                    StartTime = mp.Mission.StartTime,
                    EndTime = mp.Mission.EndTime,
                    IsPresent = false
                })
                .ToListAsync();
        }

        private async Task<ReportHeaderDto> GetReportHeaderAsync(
            DateTime reportDate,
            List<PersonnelReportDto> report)
        {
            ReportHeaderDto header = new()
            {
                ReportDate = reportDate,
                TotalPersonnel = report.Count,
                PresentPersonnel = report.Count(x => x.IsPresent),
                AbsentPersonnel = report.Count(x => !x.IsPresent)
            };

            Personnel? commander =
                await _resolver.GetDutyCommanderAsync(reportDate);

            Personnel? dutyOfficer =
                await _resolver.GetDutyOfficerAsync(reportDate);

            header.DutyCommander = commander?.HoTen ?? "Chưa phân công";
            header.DutyOfficer = dutyOfficer?.HoTen ?? "Chưa phân công";

            return header;
        }

        private PersonnelReportDto CreatePresentDto(Personnel personnel)
        {
            return new PersonnelReportDto
            {
                PersonnelId = personnel.PersonnelID,
                SoHieu = personnel.SoHieu,
                HoTen = personnel.HoTen,
                DonVi = personnel.DonVi,
                CapBacHam = personnel.CapBacHam ?? string.Empty,
                Status = "Có mặt",
                Reason = string.Empty,
                MissionName = string.Empty,
                Location = "Đơn vị",
                MissionStart = null,
                MissionEnd = null
            };
        }

        private PersonnelReportDto CreateAbsentDto(
            Personnel personnel,
            Mission mission)
        {
            return new PersonnelReportDto
            {
                PersonnelId = personnel.PersonnelID,
                SoHieu = personnel.SoHieu,
                HoTen = personnel.HoTen,
                DonVi = personnel.DonVi,
                CapBacHam = personnel.CapBacHam ?? string.Empty,
                Status = "Vắng mặt",
                Reason = string.IsNullOrWhiteSpace(mission.MissionType)
                    ? mission.MissionName
                    : mission.MissionType,
                MissionName = mission.MissionName,
                Location = mission.Location ?? string.Empty,
                MissionStart = mission.StartTime,
                MissionEnd = mission.EndTime
            };
        }

        public async Task<List<PersonnelReportDto>> GetReportAsync(
            DateTime reportDate)
        {
            using var context = CreateContext();

            // Báo cáo là ảnh chụp quân số tại một thời điểm cụ thể,
            // không phải tổng hợp tất cả công tác trong cả ngày.
            List<Personnel> personnels = await context.Personnel
                .AsNoTracking()
                .OrderBy(x => x.HoTen)
                .ToListAsync();

            List<MissionPersonnel> activeAssignments =
                await context.MissionPersonnel
                    .AsNoTracking()
                    .Include(x => x.Mission)
                    .Where(x =>
                        x.Mission != null
                        && x.Mission.StartTime <= reportDate
                        && x.Mission.EndTime >= reportDate
                        && x.Mission.MissionType != MissionTypes.DutyCommander
                        && x.Mission.MissionType != MissionTypes.DutyOfficer)
                    .ToListAsync();

            // Một CBCS chỉ được tính một lần trong quân số vắng mặt,
            // kể cả khi dữ liệu có nhiều phân công đang giao nhau.
            Dictionary<int, Mission> activeMissionByPersonnel =
                activeAssignments
                    .Where(x => x.Mission != null)
                    .GroupBy(x => x.PersonnelId)
                    .ToDictionary(
                        group => group.Key,
                        group => group
                            .Select(x => x.Mission!)
                            .OrderBy(x => x.StartTime)
                            .ThenBy(x => x.EndTime)
                            .First());

            List<PersonnelReportDto> result = new();

            foreach (Personnel personnel in personnels)
            {
                if (activeMissionByPersonnel.TryGetValue(
                    personnel.PersonnelID,
                    out Mission? activeMission))
                {
                    result.Add(CreateAbsentDto(personnel, activeMission));
                }
                else
                {
                    result.Add(CreatePresentDto(personnel));
                }
            }

            return result;
        }

        public async Task<int> GetTotalPersonnelAsync()
        {
            using var context = CreateContext();
            return await context.Personnel.CountAsync();
        }

        public async Task<int> GetPresentCountAsync(DateTime reportDate)
        {
            List<PersonnelReportDto> report = await GetReportAsync(reportDate);
            return report.Count(x => x.Status == "Có mặt");
        }

        public async Task<int> GetAbsentCountAsync(DateTime reportDate)
        {
            List<PersonnelReportDto> report = await GetReportAsync(reportDate);
            return report.Count(x => x.Status == "Vắng mặt");
        }

        public async Task<List<PersonnelReportDto>> GetPresentPersonnelAsync(
            DateTime reportDate)
        {
            List<PersonnelReportDto> report = await GetReportAsync(reportDate);

            return report
                .Where(x => x.Status == "Có mặt")
                .OrderBy(x => x.HoTen)
                .ToList();
        }

        public async Task<List<PersonnelReportDto>> GetAbsentPersonnelAsync(
            DateTime reportDate)
        {
            List<PersonnelReportDto> report = await GetReportAsync(reportDate);

            return report
                .Where(x => x.Status == "Vắng mặt")
                .OrderBy(x => x.HoTen)
                .ToList();
        }

        public async Task<string> BuildReportTextAsync(DateTime reportDate)
        {
            List<PersonnelReportDto> report = await GetReportAsync(reportDate);

            ReportHeaderDto header = await GetReportHeaderAsync(
                reportDate,
                report);

            List<MissionAssignmentDto> assignments =
                await GetMissionAssignmentsAsync(reportDate);

            var groups = _groupBuilder.Build(assignments);

            return _reportBuilder.Build(
                reportDate,
                header,
                groups);
        }
    }
}
