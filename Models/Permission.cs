using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyCBCS.Models
{
    [Table("Permissions")]
    public class Permission
    {
        [Key]
        public int PermissionId { get; set; }

        [Required]
        [MaxLength(100)]
        public string PermissionKey { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string PermissionName { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }
    }
}