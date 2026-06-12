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
}