using Microsoft.EntityFrameworkCore;
using QuanLyCBCS.Data;
using QuanLyCBCS.Helpers;
using QuanLyCBCS.Models;

namespace QuanLyCBCS.Services;

public class AuditLogService
{
    public async Task AddLogAsync(
        string action,
        string description)
    {
        using ApplicationDbContext db =
            new ApplicationDbContext();

        AuditLog log =
            new AuditLog
            {
                Username =
                    CurrentUser.Username,

                Action =
                    action,

                Description =
                    description,

                CreatedAt =
                    DateTime.Now
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

    public async Task<List<AuditLog>>
    SearchAsync(string keyword)
    {
        using ApplicationDbContext db =
            new ApplicationDbContext();

        keyword = keyword.Trim();

        return await db.AuditLogs
            .Where(x =>
                x.Username.Contains(keyword)
                || x.Action.Contains(keyword)
                || x.Description.Contains(keyword))
            .OrderByDescending(
                x => x.AuditLogId)
            .ToListAsync();
    }
}