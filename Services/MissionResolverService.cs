using Microsoft.EntityFrameworkCore;
using QuanLyCBCS.Data;
using QuanLyCBCS.DTOs;
using QuanLyCBCS.Models;
using QuanLyCBCS.Common;

namespace QuanLyCBCS.Services.MissionServices;

public class MissionResolverService
{
    private static ApplicationDbContext CreateContext()
        => new();

    public async Task<MissionResolveResultDto> ResolveAsync(
    int personnelId,
    DateTime reportDate)
    {
        using var context = CreateContext();

        DateTime day = reportDate.Date;

        List<Mission> missions = await context.MissionPersonnel

            .Where(mp => mp.PersonnelId == personnelId)

            .Include(mp => mp.Mission)

            .Where(mp =>
                mp.Mission != null &&
                mp.Mission.IsActive &&
                mp.Mission.StartTime.Date <= day &&
                mp.Mission.EndTime.Date >= day)

            .Select(mp => mp.Mission!)

            .AsNoTracking()

            .ToListAsync();

        //--------------------------------------------------
        // Không có nhiệm vụ
        //--------------------------------------------------

        if (missions.Count == 0)
        {
            return new MissionResolveResultDto
            {
                HasMission = false,
                HasConflict = false
            };
        }

        //--------------------------------------------------
        // Có nhiều nhiệm vụ
        //--------------------------------------------------

        if (missions.Count > 1)
        {
            Mission first = missions.First();

            return new MissionResolveResultDto
            {
                HasMission = true,

                HasConflict = true,

                MissionId = first.MissionId,

                MissionName = first.MissionName,

                MissionType = first.MissionType ?? "",

                Location = first.Location ?? "",

                StartTime = first.StartTime,

                EndTime = first.EndTime
            };
        }

        //--------------------------------------------------
        // Có đúng một nhiệm vụ
        //--------------------------------------------------

        Mission mission = missions[0];

        return new MissionResolveResultDto
        {
            HasMission = true,

            HasConflict = false,

            MissionId = mission.MissionId,

            MissionName = mission.MissionName,

            MissionType = mission.MissionType ?? "",

            Location = mission.Location ?? "",

            StartTime = mission.StartTime,

            EndTime = mission.EndTime
        };
    }

    private async Task<Personnel?> GetDutyPersonnelAsync(
    DateTime reportDate,
    string missionType)
    {
        using var context = CreateContext();

        DateTime day = reportDate.Date;

        MissionPersonnel? assignment =
            await context.MissionPersonnel
                .Include(x => x.Personnel)
                .Include(x => x.Mission)
                .Where(x =>
                    x.Mission != null &&
                    x.Mission.IsActive &&
                    x.Mission.MissionType == missionType &&
                    x.Mission.StartTime.Date <= day &&
                    x.Mission.EndTime.Date >= day)
                .AsNoTracking()
                .FirstOrDefaultAsync();

        return assignment?.Personnel;
    }

    public Task<Personnel?> GetDutyCommanderAsync(DateTime reportDate)
    {
        return GetDutyPersonnelAsync(
            reportDate,
            MissionTypes.DutyCommander);
    }

    public Task<Personnel?> GetDutyOfficerAsync(DateTime reportDate)
    {
        return GetDutyPersonnelAsync(
            reportDate,
            MissionTypes.DutyOfficer);
    }
}