using System.Collections.Generic;

namespace QuanLyCBCS.Helpers
{
    public class PermissionState
    {
        public Dictionary<string, bool> Permissions { get; set; }

        public PermissionState()
        {
            Permissions = new Dictionary<string, bool>();
        }

        public PermissionState Clone()
        {
            return new PermissionState
            {
                Permissions = new Dictionary<string, bool>(Permissions)
            };
        }
    }
}