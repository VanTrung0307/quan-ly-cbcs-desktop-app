using System;
using System.IO;
using System.Text.RegularExpressions;

namespace QuanLyCBCS.Helpers.BackupRestore
{
    public static class BackupRestoreHelper
    {
        public const string DatabaseName = "QuanLyCBCS_New";

        public static string GenerateBackupFileName()
        {
            return DatabaseName + "_" +
                DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".bak";
        }

        public static bool IsValidBackupFileName(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return false;

            if (fileName.IndexOfAny(
                Path.GetInvalidFileNameChars()) >= 0)
                return false;

            if (fileName.Contains("\\") || fileName.Contains("/"))
                return false;

            return true;
        }

        public static string NormalizeBackupFileName(string fileName)
        {
            fileName = (fileName ?? string.Empty).Trim();

            if (!fileName.EndsWith(
                ".bak", StringComparison.OrdinalIgnoreCase))
            {
                fileName += ".bak";
            }

            return fileName;
        }

        public static bool IsValidBackupFile(string filePath)
        {
            return !string.IsNullOrWhiteSpace(filePath)
                && File.Exists(filePath)
                && string.Equals(
                    Path.GetExtension(filePath),
                    ".bak",
                    StringComparison.OrdinalIgnoreCase);
        }

        public static string FormatFileSize(long bytes)
        {
            string[] units =
            {
                "B", "KB", "MB", "GB", "TB"
            };

            double size = bytes;
            int index = 0;

            while (size >= 1024 && index < units.Length - 1)
            {
                size /= 1024;
                index++;
            }

            return size.ToString("0.##") + " " + units[index];
        }

        public static string GetApplicationDataDirectory()
        {
            string root = Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData);

            return Path.Combine(root, "QuanLyCBCS");
        }

        public static string GetHistoryFilePath()
        {
            return Path.Combine(
                GetApplicationDataDirectory(),
                "BackupHistory.log");
        }

        public static string GetSettingsFilePath()
        {
            return Path.Combine(
                GetApplicationDataDirectory(),
                "BackupSettings.txt");
        }

        public static string LoadBackupDirectory()
        {
            try
            {
                string path = GetSettingsFilePath();

                if (!File.Exists(path))
                    return string.Empty;

                string directory = File.ReadAllText(path).Trim();

                return Directory.Exists(directory)
                    ? directory
                    : string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        public static void SaveBackupDirectory(string directory)
        {
            string settingsPath = GetSettingsFilePath();

            Directory.CreateDirectory(
                GetApplicationDataDirectory());

            File.WriteAllText(settingsPath, directory ?? string.Empty);
        }

        public static void AppendHistory(
            string action,
            string detail,
            string status)
        {
            try
            {
                Directory.CreateDirectory(
                    GetApplicationDataDirectory());

                string safeDetail = (detail ?? string.Empty)
                    .Replace("\r", " ")
                    .Replace("\n", " ")
                    .Replace("\t", " ");

                string safeAction = (action ?? string.Empty)
                    .Replace("\t", " ");

                string safeStatus = (status ?? string.Empty)
                    .Replace("\t", " ");

                string line =
                    DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") +
                    "\t" + safeAction +
                    "\t" + safeDetail +
                    "\t" + safeStatus;

                File.AppendAllText(
                    GetHistoryFilePath(),
                    line + Environment.NewLine);
            }
            catch
            {
                // Không để lỗi ghi lịch sử ảnh hưởng đến kết quả Backup/Restore.
            }
        }
    }
}