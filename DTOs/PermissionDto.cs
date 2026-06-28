using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyCBCS.DTOs
{
    public class PermissionDto
    {
        public int PermissionId { get; set; }

        public string PermissionKey { get; set; } = string.Empty;

        public string PermissionName { get; set; } = string.Empty;

        public bool IsGranted { get; set; }
    }
}
