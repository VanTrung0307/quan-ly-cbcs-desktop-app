using QuanLyCBCS.Data;
using QuanLyCBCS.Models;
using Microsoft.EntityFrameworkCore;

namespace QuanLyCBCS.Services
{
    public class DashboardService
    {
        public async Task<Dashboard> GetDashboardAsync()
        {
            using ApplicationDbContext db =
                new ApplicationDbContext();

            Dashboard dto =
                new Dashboard();

            // ====================
            // CBCS
            // ====================

            dto.TotalPersonnel =
                await db.Personnel.CountAsync();

            dto.SoldierCount =
                await db.Personnel
                    .CountAsync(x =>
                        (x.CapBacHam ?? "") == "Binh nhì"
                        ||
                        (x.CapBacHam ?? "") == "Binh nhất"
                        ||
                        (x.CapBacHam ?? "") == "Hạ sĩ");

            dto.OfficerCount =
                dto.TotalPersonnel -
                dto.SoldierCount;

            // ====================
            // USER
            // ====================

            dto.TotalUsers =
                await db.Users.CountAsync();

            dto.AdminCount =
                await db.Users
                    .CountAsync(x =>
                        x.Role == "Admin");

            dto.UserCount =
                await db.Users
                    .CountAsync(x =>
                        x.Role == "User");

            dto.LockedUsers =
                await db.Users
                    .CountAsync(x =>
                        x.LockedUntil != null &&
                        x.LockedUntil > DateTime.Now);

            // ====================
            // MISSION
            // ====================

            dto.TotalMission =
                await db.Missions.CountAsync();

            dto.ActiveMission =
                await db.Missions
                    .CountAsync(x =>
                        x.StartTime <= DateTime.Now &&
                        x.EndTime >= DateTime.Now);

            dto.FinishedMission =
                await db.Missions
                    .CountAsync(x =>
                        x.EndTime < DateTime.Now);

            dto.EndingMission =
                await db.Missions
                    .CountAsync(x =>
                        EF.Functions.DateDiffDay(
                            DateTime.Now,
                            x.EndTime) <= 3
                        &&
                        x.EndTime >= DateTime.Now);

            // ====================
            // SECURITY
            // ====================

            DateTime today =
                DateTime.Today;

            dto.LoginToday =
                await db.AuditLogs
                    .CountAsync(x =>
                        x.Action == "LOGIN"
                        &&
                        x.CreatedAt.Date == today);

            dto.FailedLoginToday =
                await db.AuditLogs
                    .CountAsync(x =>
                        x.Action == "FAILED_LOGIN"
                        &&
                        x.CreatedAt.Date == today);

            dto.LockToday =
                await db.AuditLogs
                    .CountAsync(x =>
                        x.Action == "LOCK_ACCOUNT"
                        &&
                        x.CreatedAt.Date == today);

            dto.ResetToday =
                await db.AuditLogs
                    .CountAsync(x =>
                        x.Action == "RESET_PASSWORD"
                        &&
                        x.CreatedAt.Date == today);

            return dto;
        }

        public async Task<List<AuditLog>>
            GetRecentLogsAsync()
        {
            using ApplicationDbContext db =
                new ApplicationDbContext();

            return await db.AuditLogs
                .OrderByDescending(x => x.CreatedAt)
                .Take(10)
                .ToListAsync();
        }
    }
}