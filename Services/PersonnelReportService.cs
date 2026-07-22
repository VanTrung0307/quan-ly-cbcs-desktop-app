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
        private static ApplicationDbContext CreateContext()
            => new();

        private readonly MissionResolverService _resolver = new();
        private readonly MissionGroupBuilderService _groupBuilder = new();

        private async Task<List<MissionAssignmentDto>>
            GetMissionAssignmentsAsync(DateTime reportDate)
        {
            DateTime day = reportDate.Date;

            using var db = CreateContext();

            var result =
                await db.MissionPersonnel

                    .Where(mp =>

                        mp.Mission != null

                        && mp.Mission.IsActive

                        // Chỉ lấy nhiệm vụ trong ngày báo cáo
                        && mp.Mission.StartTime.Date <= day
                        && mp.Mission.EndTime.Date >= day

                        // Không tính trực tại đơn vị là vắng mặt
                        && mp.Mission.MissionType != MissionTypes.DutyCommander
                        && mp.Mission.MissionType != MissionTypes.DutyOfficer)

                    .Select(mp =>
                        new MissionAssignmentDto
                        {
                            PersonnelId = mp.PersonnelId,

                            MissionName = mp.Mission!.MissionName ?? "",

                            Location = mp.Mission.Location ?? "",

                            Reason = mp.Mission.MissionType ?? "Khác"
                        })

                    .ToListAsync();

            return result;
        }

        private async Task<ReportHeaderDto> GetReportHeaderAsync(
            DateTime reportDate,
            List<PersonnelReportDto> report)
        {
            ReportHeaderDto header = new()
            {
                ReportDate = reportDate,

                TotalPersonnel = report.Count,

                PresentPersonnel =
                    report.Count(x => x.IsPresent),

                AbsentPersonnel =
                    report.Count(x => !x.IsPresent)
            };

            Personnel? commander =
                await _resolver.GetDutyCommanderAsync(reportDate);

            Personnel? dutyOfficer =
                await _resolver.GetDutyOfficerAsync(reportDate);

            header.DutyCommander =
                commander?.HoTen ?? "Chưa phân công";

            header.DutyOfficer =
                dutyOfficer?.HoTen ?? "Chưa phân công";

            return header;
        }

        private PersonnelReportDto CreatePresentDto(
            Personnel personnel)
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

            //-------------------------------------------------
            // 1. Lấy toàn bộ CBCS
            //-------------------------------------------------

            List<Personnel> personnels =
                await context.Personnel
                    .AsNoTracking()
                    .OrderBy(x => x.HoTen)
                    .ToListAsync();

            //-------------------------------------------------
            // 2. Lấy toàn bộ Mission đang hiệu lực
            //-------------------------------------------------

            List<MissionPersonnel> missionPersonnelList =
                await context.MissionPersonnel

                    .Include(x => x.Mission)

                    .Where(x =>

                        x.Mission != null

                        && x.Mission.IsActive

                        && x.Mission.StartTime <= reportDate

                        && x.Mission.EndTime >= reportDate)

                    .AsNoTracking()

                    .ToListAsync();

            //-------------------------------------------------
            // 3. Dictionary:
            // PersonnelId -> Mission
            //-------------------------------------------------

            Dictionary<int, Mission> missionLookup =
                missionPersonnelList

                    .Where(x => x.Mission != null)

                    .GroupBy(x => x.PersonnelId)

                    .ToDictionary(

                        g => g.Key,

                        g => g.First().Mission!);

            //-------------------------------------------------
            // 4. Tạo báo cáo
            //-------------------------------------------------

            List<PersonnelReportDto> result = new();

            foreach (Personnel personnel in personnels)
            {
                if (missionLookup.TryGetValue(
                        personnel.PersonnelID,
                        out Mission? mission))
                {
                    bool isDutyAtUnit =
                        mission.MissionType == MissionTypes.DutyCommander
                        ||
                        mission.MissionType == MissionTypes.DutyOfficer;

                    if (isDutyAtUnit)
                    {
                        result.Add(
                            CreatePresentDto(
                                personnel));
                    }
                    else
                    {
                        result.Add(
                            CreateAbsentDto(
                                personnel,
                                mission));
                    }
                }
                else
                {
                    result.Add(
                        CreatePresentDto(
                            personnel));
                }
            }

            return result;
        }

        public async Task<int> GetTotalPersonnelAsync()
        {
            using var context = CreateContext();
            return await context.Personnel.CountAsync();
        }

        public async Task<int> GetPresentCountAsync(
            DateTime reportDate)
        {
            List<PersonnelReportDto> report =
                await GetReportAsync(reportDate);

            return report.Count(x =>
                x.Status == "Có mặt");
        }

        public async Task<int> GetAbsentCountAsync(
            DateTime reportDate)
        {
            List<PersonnelReportDto> report =
                await GetReportAsync(reportDate);

            return report.Count(x =>
                x.Status == "Vắng mặt");
        }

        public async Task<List<PersonnelReportDto>>
            GetPresentPersonnelAsync(
                DateTime reportDate)    
        {
            List<PersonnelReportDto> report =
                await GetReportAsync(reportDate);

            return report

                .Where(x => x.Status == "Có mặt")

                .OrderBy(x => x.HoTen)

                .ToList();
        }

        public async Task<List<PersonnelReportDto>>
            GetAbsentPersonnelAsync(
                DateTime reportDate)
        {
            List<PersonnelReportDto> report =
                await GetReportAsync(reportDate);

            return report

                .Where(x => x.Status == "Vắng mặt")

                .OrderBy(x => x.HoTen)

                .ToList();
        }

        public async Task<string> BuildReportTextAsync(DateTime reportDate)
        {
            List<PersonnelReportDto> report =
                await GetReportAsync(reportDate);

            ReportHeaderDto header =
                await GetReportHeaderAsync(
                    reportDate,
                    report);

            var assignments =
                await GetMissionAssignmentsAsync(reportDate);

            var groups =
                _groupBuilder.Build(assignments);

            return _reportBuilder.Build(
                reportDate,
                header,
                groups);
        }
    }
}