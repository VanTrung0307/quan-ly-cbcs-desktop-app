using Microsoft.EntityFrameworkCore;
using QuanLyCBCS.Data;
using QuanLyCBCS.DTOs;
using QuanLyCBCS.Helpers;
using Microsoft.EntityFrameworkCore.Storage;

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

        public async Task<List<string>> GetRolesAsync()
        {
            using ApplicationDbContext db = new();

            return await db.RolePermissions
                .Select(x => x.Role)
                .Distinct()
                .OrderBy(x => x)
                .ToListAsync();
        }

        public async Task<List<PermissionDto>>
            GetAllPermissionsAsync()
        {
            using ApplicationDbContext db = new();

            return await db.Permissions
                .OrderBy(x => x.PermissionName)
                .Select(x => new PermissionDto
                {
                    PermissionId = x.PermissionId,
                    PermissionKey = x.PermissionKey,
                    PermissionName = x.PermissionName,
                    IsGranted = false
                })
                .ToListAsync();
        }

        public async Task<List<PermissionDto>>
            GetRolePermissionsAsync(string role)
        {
            using ApplicationDbContext db = new();

            List<int> grantedPermissions =
                await db.RolePermissions
                    .Where(x => x.Role == role)
                    .Select(x => x.PermissionId)
                    .ToListAsync();

            return await db.Permissions
                .OrderBy(x => x.PermissionName)
                .Select(x => new PermissionDto
                {
                    PermissionId = x.PermissionId,

                    PermissionKey = x.PermissionKey,

                    PermissionName = x.PermissionName,

                    IsGranted =
                        grantedPermissions
                            .Contains(x.PermissionId)
                })
                .ToListAsync();
        }

        public async Task RefreshCurrentUserPermissionsAsync()
        {
            CurrentUser.Permissions =
                (await GetPermissionsAsync(
                    CurrentUser.Role))
                .ToHashSet();
        }

        public async Task SaveRolePermissionsAsync(
            string role,
            List<int> permissionIds)
        {
            using ApplicationDbContext db = new();

            using IDbContextTransaction transaction =
                await db.Database.BeginTransactionAsync();

            try
            {
                db.RolePermissions.RemoveRange(
                    db.RolePermissions
                        .Where(x => x.Role == role));

                await db.SaveChangesAsync();

                foreach (int permissionId
                         in permissionIds)
                {
                    await db.RolePermissions
                        .AddAsync(
                            new Models.RolePermission
                            {
                                Role = role,
                                PermissionId = permissionId
                            });
                }

                await db.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();

                throw;
            }
        }
    }
}