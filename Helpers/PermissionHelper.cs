namespace QuanLyCBCS.Helpers
{
    public static class PermissionHelper
    {
        public static bool IsAdmin =>
            CurrentUser.Role.Equals(
                "Admin",
                StringComparison.OrdinalIgnoreCase);

        public static bool IsUser =>
            CurrentUser.Role.Equals(
                "User",
                StringComparison.OrdinalIgnoreCase);

        public static bool CanManagePersonnel =>
            IsAdmin;

        public static bool CanManageUsers =>
            IsAdmin;

        public static bool CanBackupRestore =>
            IsAdmin;

        public static bool CanManageMission =>
            IsAdmin;

        public static bool CanAssignMission =>
            IsAdmin || IsUser;

        public static bool CanViewStatistics =>
            true;
    }
}