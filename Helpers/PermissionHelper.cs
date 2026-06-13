namespace QuanLyCBCS.Helpers
{
    public static class PermissionHelper
    {
        public static void CheckAdmin()
        {
            if (CurrentUser.Role != "Admin")
            {
                throw new Exception(
                    "Bạn không có quyền thực hiện chức năng này.");
            }
        }
    }
}