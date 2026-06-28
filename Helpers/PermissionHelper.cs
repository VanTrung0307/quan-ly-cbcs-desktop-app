using System;

namespace QuanLyCBCS.Helpers
{
    public static class PermissionHelper
    {
        //========================
        // ROLE
        //========================

        public static bool IsAdmin =>
            CurrentUser.Role.Equals(
                "Admin",
                StringComparison.OrdinalIgnoreCase);

        public static bool IsUser =>
            CurrentUser.Role.Equals(
                "User",
                StringComparison.OrdinalIgnoreCase);

        //========================
        // UI (giữ để tương thích)
        //========================

        public static bool CanManagePersonnel =>
            CurrentUser.HasPermission("Personnel.Create") ||
            CurrentUser.HasPermission("Personnel.Update") ||
            CurrentUser.HasPermission("Personnel.Delete");

        public static bool CanManageUsers =>
            CurrentUser.HasPermission("User.Manage");

        public static bool CanBackupRestore =>
            CurrentUser.HasPermission("Database.Backup") ||
            CurrentUser.HasPermission("Database.Restore");

        public static bool CanManageMission =>
            CurrentUser.HasPermission("Mission.Manage");

        public static bool CanAssignMission =>
            CurrentUser.HasPermission("Mission.Manage") ||
            CurrentUser.HasPermission("Mission.View");

        public static bool CanViewStatistics =>
            CurrentUser.HasPermission("Dashboard.View");

        //========================
        // RBAC mới
        //========================

        public static bool HasPermission(string permission)
        {
            return CurrentUser.HasPermission(permission);
        }

        public static void Require(string permission)
        {
            if (!HasPermission(permission))
            {
                throw new UnauthorizedAccessException(
                    $"Bạn không có quyền: {permission}");
            }
        }
    }
}