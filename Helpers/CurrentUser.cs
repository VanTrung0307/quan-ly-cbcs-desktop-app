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

        public static bool HasPermission(string permission)
        {
            if (Permissions == null)
                return false;

            return Permissions.Contains(permission);
        }

        public static void Clear()
        {
            UserId = 0;
            Username = "";
            Role = "";

            if (Permissions != null)
            {
                Permissions.Clear();
            }
            else
            {
                Permissions = new HashSet<string>(
                    StringComparer.OrdinalIgnoreCase);
            }
        }
    }
}