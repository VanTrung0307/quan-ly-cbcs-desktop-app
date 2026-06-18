using Microsoft.EntityFrameworkCore;
using QuanLyCBCS.Data;
using QuanLyCBCS.Models;

namespace QuanLyCBCS.Services
{
    public class MissionPersonnelService
    {
        private readonly ApplicationDbContext
            _db =
                new();

        public async Task<List<MissionPersonnel>>
            GetByMissionAsync(
                int missionId)
        {
            return await _db
                .MissionPersonnel
                .Include(x => x.Personnel)
                .Where(x =>
                    x.MissionId == missionId)
                .ToListAsync();
        }

        public async Task AddAsync(
            int missionId,
            int personnelId)
        {
            bool exists =
                await _db
                    .MissionPersonnel
                    .AnyAsync(x =>
                        x.MissionId == missionId
                        &&
                        x.PersonnelId == personnelId);

            if (exists)
                return;

            MissionPersonnel item =
                new()
                {
                    MissionId = missionId,
                    PersonnelId = personnelId,
                    AssignedAt = DateTime.Now
                };

            _db.MissionPersonnel.Add(item);

            await _db.SaveChangesAsync();
        }

        public async Task RemoveAsync(
            int missionId,
            int personnelId)
        {
            var item =
                await _db
                    .MissionPersonnel
                    .FirstOrDefaultAsync(x =>
                        x.MissionId == missionId
                        &&
                        x.PersonnelId == personnelId);

            if (item == null)
                return;

            _db.MissionPersonnel.Remove(item);

            await _db.SaveChangesAsync();
        }
    }
}