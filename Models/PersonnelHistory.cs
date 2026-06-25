using System.ComponentModel.DataAnnotations;

namespace QuanLyCBCS.Models
{
    public class PersonnelHistory
    {
        [Key]
        public int PersonnelHistoryId { get; set; }

        public int PersonnelId { get; set; }

        public string AffectedPersonnelName { get; set; }
            = string.Empty;

        public string Action { get; set; }
            = string.Empty;

        public string Description { get; set; }
            = string.Empty;

        public string PerformedBy { get; set; }
            = string.Empty;

        public DateTime CreatedAt { get; set; }
    }
}