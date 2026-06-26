using System;
using System.Collections.Generic;

namespace QuanLyCBCS.Helpers
{
    public static class CurrentUser
    {
        public static int UserId { get; set; }

        public static string Username { get; set; } = "";

        public static string Role { get; set; } = "";

        // Danh sách quyền của User hiện tại
        public static HashSet<string> Permissions { get; set; }
            = new();

        public static bool HasPermission(string permissionKey)
        {
            return Permissions.Contains(permissionKey);
        }

        public static void Clear()
        {
            UserId = 0;
            Username = "";

            Role = "";

            Permissions.Clear();
        }
    }
}