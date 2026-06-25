using Microsoft.EntityFrameworkCore;
using QuanLyCBCS.Data;
using QuanLyCBCS.Helpers;
using QuanLyCBCS.Models;

namespace QuanLyCBCS.Services
{
    public class PersonnelHistoryService
    {
        public async Task AddHistoryAsync(
            int personnelId,
            string personnelName,
            string action,
            string description)
        {
            using ApplicationDbContext db = new();

            PersonnelHistory history = new()
            {
                PersonnelId = personnelId,
                AffectedPersonnelName = personnelName,
                Action = action,
                Description = description,
                PerformedBy = CurrentUser.Username,
                CreatedAt = DateTime.Now
            };

            db.PersonnelHistories.Add(history);

            await db.SaveChangesAsync();
        }

        public async Task<List<PersonnelHistory>> GetAllAsync()
        {
            using ApplicationDbContext db = new();

            return await db.PersonnelHistories
                           .OrderByDescending(x => x.CreatedAt)
                           .ToListAsync();
        }

        public async Task<List<PersonnelHistory>> GetByPersonnelAsync(int personnelId)
        {
            using ApplicationDbContext db = new();

            return await db.PersonnelHistories
                           .Where(x => x.PersonnelId == personnelId)
                           .OrderByDescending(x => x.CreatedAt)
                           .ToListAsync();
        }
    }
}