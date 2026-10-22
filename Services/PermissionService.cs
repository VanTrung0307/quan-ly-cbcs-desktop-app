using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using QuanLyCBCS.Data;
using QuanLyCBCS.DTOs;
using QuanLyCBCS.Helpers;
using QuanLyCBCS.Models;

namespace QuanLyCBCS.Services
{
    public class PermissionService
    {
        // =====================================================
        // 1. QUYỀN THEO VAI TRÒ - GIỮ TƯƠNG THÍCH VỚI CODE CŨ
        // =====================================================

        public async Task<List<string>> GetPermissionsAsync(string role)
        {
            using ApplicationDbContext db = new();

            return await db.RolePermissions
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

        public async Task<List<PermissionDto>> GetAllPermissionsAsync()
        {
            using ApplicationDbContext db = new();

            return await db.Permissions
                .AsNoTracking()
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

        public async Task<List<PermissionDto>> GetRolePermissionsAsync(
            string role)
        {
            using ApplicationDbContext db = new();

            List<int> grantedIds = await db.RolePermissions
                .Where(x => x.Role == role)
                .Select(x => x.PermissionId)
                .ToListAsync();

            return await db.Permissions
                .AsNoTracking()
                .OrderBy(x => x.PermissionName)
                .Select(x => new PermissionDto
                {
                    PermissionId = x.PermissionId,
                    PermissionKey = x.PermissionKey,
                    PermissionName = x.PermissionName,
                    IsGranted = grantedIds.Contains(x.PermissionId)
                })
                .ToListAsync();
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
                List<RolePermission> oldPermissions =
                    await db.RolePermissions
                        .Where(x => x.Role == role)
                        .ToListAsync();

                db.RolePermissions.RemoveRange(oldPermissions);

                foreach (int permissionId in permissionIds.Distinct())
                {
                    db.RolePermissions.Add(new RolePermission
                    {
                        Role = role,
                        PermissionId = permissionId
                    });
                }

                await db.SaveChangesAsync();
                await transaction.CommitAsync();

                // Nạp lại quyền hiệu lực của người đang đăng nhập.
                await RefreshCurrentUserPermissionsAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        // =====================================================
        // 2. TÀI KHOẢN - PHỤC VỤ DROPDOWN TÌM KIẾM
        // =====================================================

        public async Task<List<User>> GetUsersAsync()
        {
            using ApplicationDbContext db = new();

            return await db.Users
                .AsNoTracking()
                .OrderBy(x => x.Username)
                .ToListAsync();
        }

        // =====================================================
        // 3. ĐỌC QUYỀN HIỆU LỰC CỦA MỘT TÀI KHOẢN
        // =====================================================

        public async Task<List<PermissionDto>> GetUserPermissionsAsync(
            int userId)
        {
            using ApplicationDbContext db = new();

            User? user = await db.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (user == null)
            {
                throw new InvalidOperationException(
                    "Không tìm thấy tài khoản cần phân quyền.");
            }

            List<int> grantedIds;

            if (user.PermissionsCustomized)
            {
                // Tài khoản đã có bộ quyền riêng.
                grantedIds = await db.UserPermissions
                    .Where(x => x.UserId == userId)
                    .Select(x => x.PermissionId)
                    .ToListAsync();
            }
            else
            {
                // Chưa tùy chỉnh: kế thừa quyền của vai trò.
                grantedIds = await db.RolePermissions
                    .Where(x => x.Role == user.Role)
                    .Select(x => x.PermissionId)
                    .ToListAsync();
            }

            return await db.Permissions
                .AsNoTracking()
                .OrderBy(x => x.PermissionName)
                .Select(x => new PermissionDto
                {
                    PermissionId = x.PermissionId,
                    PermissionKey = x.PermissionKey,
                    PermissionName = x.PermissionName,
                    IsGranted = grantedIds.Contains(x.PermissionId)
                })
                .ToListAsync();
        }

        // =====================================================
        // 4. LƯU QUYỀN RIÊNG CHO MỘT TÀI KHOẢN
        // =====================================================

        public async Task SaveUserPermissionsAsync(
            int userId,
            List<int> permissionIds)
        {
            using ApplicationDbContext db = new();

            using IDbContextTransaction transaction =
                await db.Database.BeginTransactionAsync();

            try
            {
                User? user = await db.Users
                    .FirstOrDefaultAsync(x => x.UserId == userId);

                if (user == null)
                {
                    throw new InvalidOperationException(
                        "Không tìm thấy tài khoản cần lưu quyền.");
                }

                List<int> uniqueIds = permissionIds
                    .Distinct()
                    .ToList();

                // Kiểm tra mọi PermissionId được gửi lên có tồn tại.
                int validCount = await db.Permissions
                    .CountAsync(x => uniqueIds.Contains(x.PermissionId));

                if (validCount != uniqueIds.Count)
                {
                    throw new InvalidOperationException(
                        "Danh sách quyền chứa mã quyền không hợp lệ.");
                }

                List<UserPermission> oldPermissions =
                    await db.UserPermissions
                        .Where(x => x.UserId == userId)
                        .ToListAsync();

                db.UserPermissions.RemoveRange(oldPermissions);

                foreach (int permissionId in uniqueIds)
                {
                    db.UserPermissions.Add(new UserPermission
                    {
                        UserId = userId,
                        PermissionId = permissionId
                    });
                }

                // Kể cả khi không tích quyền nào, tài khoản vẫn có
                // bộ quyền riêng rỗng, không quay về kế thừa Role.
                user.PermissionsCustomized = true;

                await db.SaveChangesAsync();
                await transaction.CommitAsync();

                // Nếu đang sửa quyền chính tài khoản hiện tại,
                // cập nhật quyền ngay trong phiên đăng nhập.
                if (CurrentUser.UserId == userId)
                {
                    await RefreshCurrentUserPermissionsAsync();
                }
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        // =====================================================
        // 5. NẠP QUYỀN HIỆU LỰC CHO NGƯỜI ĐANG ĐĂNG NHẬP
        // =====================================================

        public async Task RefreshCurrentUserPermissionsAsync()
        {
            using ApplicationDbContext db = new();

            User? user = await db.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.UserId == CurrentUser.UserId);

            if (user == null)
            {
                CurrentUser.Permissions =
                    new HashSet<string>(
                        StringComparer.OrdinalIgnoreCase);

                return;
            }

            List<string> permissionKeys;

            if (user.PermissionsCustomized)
            {
                permissionKeys = await db.UserPermissions
                    .Where(x => x.UserId == user.UserId)
                    .Select(x => x.Permission!.PermissionKey)
                    .ToListAsync();
            }
            else
            {
                permissionKeys = await db.RolePermissions
                    .Where(x => x.Role == user.Role)
                    .Select(x => x.Permission!.PermissionKey)
                    .ToListAsync();
            }

            CurrentUser.Permissions = permissionKeys.ToHashSet(
                StringComparer.OrdinalIgnoreCase);
        }
    }
}