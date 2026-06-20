using Microsoft.EntityFrameworkCore;
using QuanLyCBCS.Data;
using QuanLyCBCS.DTOs;
using QuanLyCBCS.Models;

namespace QuanLyCBCS.Services;

public class MissionService
{
    private readonly ApplicationDbContext
        _context = new();

    public async Task<List<Mission>>
        GetAllAsync()
    {
        return await _context.Missions
            .OrderByDescending(x => x.StartTime)
            .ToListAsync();
    }

    public async Task<Mission?>
        GetByIdAsync(int missionId)
    {
        return await _context.Missions
            .FirstOrDefaultAsync(
                x => x.MissionId == missionId);
    }

    public async Task AddAsync(
        Mission mission)
    {
        _context.Missions.Add(mission);

        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(
        Mission mission)
    {
        using ApplicationDbContext db =
            new();

        db.Missions.Update(mission);

        await db.SaveChangesAsync();
    }

    public async Task DeleteAsync(
        int missionId)
    {
        using ApplicationDbContext db =
            new();

        Mission? mission =
            await db.Missions
                .FindAsync(missionId);

        if (mission == null)
            return;

        db.Missions.Remove(mission);

        await db.SaveChangesAsync();
    }

    public async Task<MissionDashboardDto>
        GetDashboardAsync()
    {
        using ApplicationDbContext db =
            new();

        MissionDashboardDto dto =
            new();

        dto.TongCongTac =
            await db.Missions.CountAsync();

        dto.DangHoatDong =
            await db.Missions.CountAsync(
                x => x.IsActive);

        dto.DaKetThuc =
            await db.Missions.CountAsync(
                x => !x.IsActive);

        var topPersonnel =
            await db.MissionPersonnel
                .GroupBy(
                    x => x.PersonnelId)
                .Select(g =>
                    new
                    {
                        PersonnelId =
                            g.Key,

                        Count =
                            g.Count()
                    })
                .OrderByDescending(
                    x => x.Count)
                .FirstOrDefaultAsync();

        if (topPersonnel != null)
        {
            var personnel =
                await db.Personnel
                    .FirstOrDefaultAsync(
                        x =>
                            x.PersonnelID ==
                            topPersonnel.PersonnelId);

            dto.TopPersonnelName =
                personnel?.HoTen;

            dto.TopPersonnelCount =
                topPersonnel.Count;
        }

        return dto;
    }

    public async Task<List<Mission>>
        GetRecentMissionsAsync()
    {
        using ApplicationDbContext db =
            new();

        return await db
            .Missions
            .OrderByDescending(
                x => x.CreatedAt)
            .Take(5)
            .ToListAsync();
    }

    public async Task<List<PersonnelMissionStatisticDto>>
        GetTopPersonnelAsync()
    {
        using ApplicationDbContext db =
            new();

        return await db.Personnel
            .Select(x =>
                new PersonnelMissionStatisticDto
                {
                    PersonnelId = x.PersonnelID,

                    SoHieu = x.SoHieu,

                    HoTen = x.HoTen,

                    DonVi = x.DonVi,

                    CapBacHam = x.CapBacHam,

                    SoLanCongTac =
                        db.MissionPersonnel.Count(
                            mp => mp.PersonnelId ==
                                  x.PersonnelID)
                })
            .OrderByDescending(
                x => x.SoLanCongTac)
            .Take(5)
            .ToListAsync();
    }
}