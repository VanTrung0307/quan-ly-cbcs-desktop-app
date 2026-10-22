using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyCBCS.Models
{
    [Table("UserPermissions")]
    public class UserPermission
    {
        [Key]
        public int UserPermissionId { get; set; }

        public int UserId { get; set; }

        public int PermissionId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }

        [ForeignKey(nameof(PermissionId))]
        public Permission? Permission { get; set; }
    }
}