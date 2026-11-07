using System;

namespace QuanLyCBCS.DTOs.BackupRestore
{
    public class BackupHistoryDTO
    {
        public DateTime ActionTime { get; set; }

        public string ActionName { get; set; }

        public string Detail { get; set; }

        public string Status { get; set; }

        public BackupHistoryDTO()
        {
            ActionName = string.Empty;
            Detail = string.Empty;
            Status = string.Empty;
        }
    }
}