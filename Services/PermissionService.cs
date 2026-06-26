using Microsoft.EntityFrameworkCore;
using QuanLyCBCS.Data;

namespace QuanLyCBCS.Services
{
    public class PermissionService
    {
        public async Task<List<string>> GetPermissionsAsync(string role)
        {
            using ApplicationDbContext db = new();

            return await db.RolePermissions
                .Include(x => x.Permission)
                .Where(x => x.Role == role)
                .Select(x => x.Permission!.PermissionKey)
                .ToListAsync();
        }

        public async Task<bool> HasPermissionAsync(
            string role,
            string permissionKey)
        {
            using ApplicationDbContext db = new();

            return await db.RolePermissions
                .Include(x => x.Permission)
                .AnyAsync(x =>
                    x.Role == role &&
                    x.Permission!.PermissionKey == permissionKey);
        }
    }
}