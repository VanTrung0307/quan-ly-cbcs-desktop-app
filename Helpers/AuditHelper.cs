using QuanLyCBCS.Services;

namespace QuanLyCBCS.Helpers;

public static class AuditHelper
{
    public static async Task LogAsync(
        string action,
        string description)
    {
        AuditLogService auditService = new();

        await auditService.LogAsync(
            action,
            description);
    }
}