using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyCBCS.Models
{
    [Table("RolePermissions")]
    public class RolePermission
    {
        [Key]
        public int RolePermissionId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Role { get; set; } = string.Empty;

        public int PermissionId { get; set; }

        [ForeignKey(nameof(PermissionId))]
        public Permission? Permission { get; set; }
    }
}