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
        /// Lấy các nhiệm vụ bên ngoài đơn vị có thời gian giao với ngày báo cáo.
        /// Không lọc IsActive vì IsActive là trạng thái hiện tại của nhiệm vụ,
        /// không phản ánh trạng thái nhiệm vụ tại một ngày báo cáo trong quá khứ.
        /// </summary>
        private async Task<List<MissionAssignmentDto>> GetMissionAssignmentsAsync(
            DateTime reportDate)
        {
            DateTime dayStart = reportDate.Date;
            DateTime nextDay = dayStart.AddDays(1);

            using var db = CreateContext();

            return await db.MissionPersonnel
                .AsNoTracking()
                .Include(mp => mp.Personnel)
                .Include(mp => mp.Mission)
                .Where(mp =>
                    mp.Mission != null
                    && mp.Mission.StartTime < nextDay
                    && mp.Mission.EndTime >= dayStart
                    && mp.Mission.MissionType != MissionTypes.DutyCommander
                    && mp.Mission.MissionType != MissionTypes.DutyOfficer)
                .Select(mp => new MissionAssignmentDto
                {
                    PersonnelId = mp.PersonnelId,
                    PersonnelName = mp.Personnel != null
                        ? mp.Personnel.HoTen
                        : string.Empty,
                    MissionName = mp.Mission!.MissionName ?? string.Empty,
                    Location = mp.Mission.Location ?? string.Empty,
                    Reason = mp.Mission.MissionType ?? "Khác"
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

            DateTime dayStart = reportDate.Date;
            DateTime nextDay = dayStart.AddDays(1);

            // 1. Lấy toàn bộ CBCS.
            List<Personnel> personnels = await context.Personnel
                .AsNoTracking()
                .OrderBy(x => x.HoTen)
                .ToListAsync();

            // 2. Lấy các phân công có thời gian giao với ngày báo cáo.
            // Không dùng Mission.IsActive vì báo cáo có thể xem lại ngày quá khứ.
            List<MissionPersonnel> missionPersonnelList =
                await context.MissionPersonnel
                    .AsNoTracking()
                    .Include(x => x.Mission)
                    .Where(x =>
                        x.Mission != null
                        && x.Mission.StartTime < nextDay
                        && x.Mission.EndTime >= dayStart)
                    .ToListAsync();

            // 3. Gom tất cả nhiệm vụ theo CBCS để không phụ thuộc vào
            // bản ghi đầu tiên được database trả về.
            Dictionary<int, List<Mission>> missionsByPersonnel =
                missionPersonnelList
                    .Where(x => x.Mission != null)
                    .GroupBy(x => x.PersonnelId)
                    .ToDictionary(
                        group => group.Key,
                        group => group
                            .Select(x => x.Mission!)
                            .OrderBy(x => x.StartTime)
                            .ThenBy(x => x.EndTime)
                            .ToList());

            // 4. Tạo báo cáo cho từng CBCS.
            List<PersonnelReportDto> result = new();

            foreach (Personnel personnel in personnels)
            {
                if (!missionsByPersonnel.TryGetValue(
                    personnel.PersonnelID,
                    out List<Mission>? missions))
                {
                    result.Add(CreatePresentDto(personnel));
                    continue;
                }

                // Nếu CBCS có ít nhất một nhiệm vụ bên ngoài đơn vị
                // giao với ngày báo cáo thì ghi nhận vắng mặt.
                // Nhiệm vụ trực chỉ huy/trực ban tại đơn vị không được
                // dùng để ghi đè một nhiệm vụ bên ngoài.
                Mission? externalMission = missions
                    .Where(mission => !IsDutyAtUnit(mission))
                    .OrderBy(mission => mission.StartTime)
                    .ThenBy(mission => mission.EndTime)
                    .FirstOrDefault();

                if (externalMission != null)
                {
                    result.Add(CreateAbsentDto(personnel, externalMission));
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
