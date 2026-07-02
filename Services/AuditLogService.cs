using Microsoft.EntityFrameworkCore;
using QuanLyCBCS.Data;
using QuanLyCBCS.Helpers;
using QuanLyCBCS.Models;

namespace QuanLyCBCS.Services;

public class AuditLogService
{
    public async Task LogAsync(
        string action,
        string description)
    {
        using ApplicationDbContext db =
            new ApplicationDbContext();

        AuditLog log =
            new AuditLog
            {
                Username = CurrentUser.Username,

                Action = action,

                Description = description,

                CreatedAt = DateTime.Now
            };

        db.AuditLogs.Add(log);

        await db.SaveChangesAsync();
    }

    public async Task<List<AuditLog>> GetAllAsync()
    {
        using ApplicationDbContext db =
            new ApplicationDbContext();

        return await db.AuditLogs
            .OrderByDescending(x => x.AuditLogId)
            .ToListAsync();
    }

    public async Task<List<AuditLog>> SearchAsync(
        string? keyword,
        DateTime? fromDate,
        DateTime? toDate)
    {
        using ApplicationDbContext db =
            new ApplicationDbContext();

        IQueryable<AuditLog> query =
            db.AuditLogs
                .AsNoTracking();

        // ==========================
        // Lọc theo từ khóa
        // ==========================

        keyword = keyword?.Trim();

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            query = query.Where(x =>

                x.Username.Contains(keyword)

                || x.Action.Contains(keyword)

                || x.Description.Contains(keyword));
        }

        // ==========================
        // Lọc từ ngày
        // ==========================

        if (fromDate.HasValue)
        {
            query = query.Where(x =>

                x.CreatedAt >= fromDate.Value.Date);
        }

        // ==========================
        // Lọc đến ngày
        // ==========================

        if (toDate.HasValue)
        {
            DateTime endDate =
                toDate.Value.Date.AddDays(1);

            query = query.Where(x =>

                x.CreatedAt < endDate);
        }

        return await query

            .OrderByDescending(x => x.AuditLogId)

            .ToListAsync();
    }
}