using System;

namespace QuanLyCBCS.DTOs.BackupRestore
{
    public class BackupInfoDTO
    {
        public int STT { get; set; }

        public string FileName { get; set; }

        public string FullPath { get; set; }

        public string DirectoryPath { get; set; }

        public DateTime BackupTime { get; set; }

        public long FileSize { get; set; }

        public string FileSizeText { get; set; }

        public BackupInfoDTO()
        {
            FileName = string.Empty;
            FullPath = string.Empty;
            DirectoryPath = string.Empty;
            FileSizeText = string.Empty;
        }
    }
}