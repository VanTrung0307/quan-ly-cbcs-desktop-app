using Microsoft.EntityFrameworkCore;
using QuanLyCBCS.Data;
using QuanLyCBCS.DTOs;
using QuanLyCBCS.Models;

namespace QuanLyCBCS.Services
{
    public class PersonnelReportService
    {
        private ApplicationDbContext CreateContext()
        {
            return new ApplicationDbContext();
        }

        private async Task<Mission?> GetCurrentMissionAsync(
            int personnelId,
            DateTime reportDate)
        {
            using var context = CreateContext();

            return await context.MissionPersonnel

                .Include(mp => mp.Mission)

                .Where(mp =>

                    mp.PersonnelId == personnelId

                    && mp.Mission != null

                    && mp.Mission.IsActive

                    && mp.Mission.StartTime <= reportDate

                    && mp.Mission.EndTime >= reportDate)

                .Select(mp => mp.Mission)

                .FirstOrDefaultAsync();
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

        public async Task<List<PersonnelReportDto>> GetReportAsync(DateTime reportDate)
        {
            using var context = CreateContext();

            List<Personnel> personnels =
                await context.Personnel
                    .AsNoTracking()
                    .OrderBy(p => p.HoTen)
                    .ToListAsync();

            List<PersonnelReportDto> result = new();

            foreach (var personnel in personnels)
            {
                Mission? mission =
                    await context.MissionPersonnel
                        .Include(x => x.Mission)
                        .Where(x =>
                            x.PersonnelId == personnel.PersonnelID &&
                            x.Mission != null &&
                            x.Mission.IsActive &&
                            x.Mission.StartTime <= reportDate &&
                            x.Mission.EndTime >= reportDate)
                        .Select(x => x.Mission)
                        .FirstOrDefaultAsync();

                result.Add(mission == null
                    ? CreatePresentDto(personnel)
                    : CreateAbsentDto(personnel, mission));
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
    }
}