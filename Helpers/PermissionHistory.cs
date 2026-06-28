using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyCBCS.Helpers
{
    public class PermissionHistory
    {
        public PermissionState Before { get; set; }

        public PermissionState After { get; set; }

        public PermissionHistory(
            PermissionState before,
            PermissionState after)
        {
            Before = before.Clone();
            After = after.Clone();
        }
    }
}
