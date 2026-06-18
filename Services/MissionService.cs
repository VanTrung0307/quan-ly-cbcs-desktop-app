using Microsoft.EntityFrameworkCore;
using QuanLyCBCS.Data;
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
}