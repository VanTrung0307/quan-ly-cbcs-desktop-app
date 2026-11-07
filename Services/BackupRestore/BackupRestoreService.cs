using Microsoft.Data.SqlClient;
using QuanLyCBCS.DTOs.BackupRestore;
using QuanLyCBCS.Helpers.BackupRestore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;

namespace QuanLyCBCS.Services.BackupRestore
{
    public class BackupRestoreService
    {
        private readonly string _connectionString;

        private const int CommandTimeout = 0;

        public BackupRestoreService()
        {
            _connectionString = GetApplicationConnectionString();
        }

        public BackupRestoreService(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentException(
                    "Chuỗi kết nối SQL Server không hợp lệ.");

            _connectionString = connectionString;
        }

        // =====================================================
        // 1. LẤY CHUỖI KẾT NỐI DATABASE
        // =====================================================

        private static string GetApplicationConnectionString()
        {
            foreach (ConnectionStringSettings setting
                in ConfigurationManager.ConnectionStrings)
            {
                if (setting == null ||
                    string.IsNullOrWhiteSpace(setting.ConnectionString))
                    continue;

                try
                {
                    SqlConnectionStringBuilder builder =
                        new SqlConnectionStringBuilder(
                            setting.ConnectionString);

                    if (string.Equals(
                        builder.InitialCatalog,
                        BackupRestoreHelper.DatabaseName,
                        StringComparison.OrdinalIgnoreCase))
                    {
                        return setting.ConnectionString;
                    }
                }
                catch
                {
                    // Bỏ qua connection string không hợp lệ.
                }
            }

            // Đồng bộ với ApplicationDbContext.cs.
            return
                @"Server=.\VANTRUNG;" +
                "Database=QuanLyCBCS_New;" +
                "Trusted_Connection=True;" +
                "TrustServerCertificate=True;" +
                "MultipleActiveResultSets=True;" +
                "Connect Timeout=30;";
        }

        private SqlConnectionStringBuilder GetMasterConnectionBuilder()
        {
            SqlConnectionStringBuilder builder =
                new SqlConnectionStringBuilder(_connectionString);

            if (!string.Equals(
                builder.InitialCatalog,
                BackupRestoreHelper.DatabaseName,
                StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException(
                    "Database kết nối không đúng. " +
                    "Hệ thống yêu cầu database QuanLyCBCS_New.");
            }

            builder.InitialCatalog = "master";
            builder.ConnectTimeout = Math.Max(builder.ConnectTimeout, 30);
            builder.Pooling = false;

            return builder;
        }

        private SqlConnection CreateMasterConnection()
        {
            return new SqlConnection(
                GetMasterConnectionBuilder().ConnectionString);
        }

        private static string QuoteIdentifier(string value)
        {
            return "[" + value.Replace("]", "]]") + "]";
        }

        // =====================================================
        // 2. KIỂM TRA KẾT NỐI
        // =====================================================

        public void TestConnection()
        {
            using (SqlConnection connection = CreateMasterConnection())
            {
                connection.Open();

                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText =
                        "SELECT DB_ID(@databaseName);";

                    command.Parameters.Add(
                        "@databaseName",
                        SqlDbType.NVarChar,
                        128).Value =
                        BackupRestoreHelper.DatabaseName;

                    object result = command.ExecuteScalar();

                    if (result == null || result == DBNull.Value)
                    {
                        throw new InvalidOperationException(
                            "Không tìm thấy database QuanLyCBCS_New.");
                    }
                }
            }
        }

        // =====================================================
        // 3. BACKUP DATABASE
        // =====================================================

        public void BackupDatabase(string backupPath)
        {
            if (string.IsNullOrWhiteSpace(backupPath))
                throw new ArgumentException(
                    "Đường dẫn backup không được để trống.");

            string? directory = Path.GetDirectoryName(backupPath);

            if (string.IsNullOrWhiteSpace(directory))
                throw new InvalidOperationException(
                    "Không xác định được thư mục lưu backup.");

            if (!Directory.Exists(directory))
                throw new DirectoryNotFoundException(
                    "Thư mục lưu backup không tồn tại.");

            if (File.Exists(backupPath))
                throw new IOException(
                    "File backup đã tồn tại. Vui lòng chọn tên khác.");

            string database =
                QuoteIdentifier(BackupRestoreHelper.DatabaseName);

            using (SqlConnection connection = CreateMasterConnection())
            {
                connection.Open();

                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandTimeout = CommandTimeout;

                    command.CommandText =
                        "BACKUP DATABASE " + database +
                        " TO DISK = @backupPath " +
                        "WITH INIT, CHECKSUM, STATS = 10;";

                    command.Parameters.Add(
                        "@backupPath",
                        SqlDbType.NVarChar,
                        4000).Value = backupPath;

                    command.ExecuteNonQuery();
                }
            }

            // Kiểm tra lại file sau khi backup.
            VerifyBackup(backupPath);
        }

        // =====================================================
        // 4. KIỂM TRA FILE BACKUP
        // =====================================================

        public void VerifyBackup(string backupPath)
        {
            if (!BackupRestoreHelper.IsValidBackupFile(backupPath))
                throw new FileNotFoundException(
                    "File backup không tồn tại hoặc không hợp lệ.");

            using (SqlConnection connection = CreateMasterConnection())
            {
                connection.Open();

                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandTimeout = CommandTimeout;

                    command.CommandText =
                        "RESTORE VERIFYONLY FROM DISK = @backupPath;";

                    command.Parameters.Add(
                        "@backupPath",
                        SqlDbType.NVarChar,
                        4000).Value = backupPath;

                    command.ExecuteNonQuery();
                }
            }
        }

        // =====================================================
        // 5. LẤY THÔNG TIN DATABASE TRONG FILE BACKUP
        // =====================================================

        public string GetBackupDatabaseName(string backupPath)
        {
            if (!BackupRestoreHelper.IsValidBackupFile(backupPath))
                throw new FileNotFoundException(
                    "Không tìm thấy file backup.");

            using (SqlConnection connection = CreateMasterConnection())
            {
                connection.Open();

                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandTimeout = CommandTimeout;

                    command.CommandText =
                        "RESTORE HEADERONLY FROM DISK = @backupPath;";

                    command.Parameters.Add(
                        "@backupPath",
                        SqlDbType.NVarChar,
                        4000).Value = backupPath;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                            throw new InvalidOperationException(
                                "File backup không có thông tin hợp lệ.");

                        int ordinal =
                            reader.GetOrdinal("DatabaseName");

                        return reader.IsDBNull(ordinal)
                            ? string.Empty
                            : reader.GetString(ordinal);
                    }
                }
            }
        }

        // =====================================================
        // 6. RESTORE DATABASE
        // =====================================================

        public void RestoreDatabase(string backupPath)
        {
            if (!BackupRestoreHelper.IsValidBackupFile(backupPath))
                throw new FileNotFoundException(
                    "File backup không tồn tại hoặc không hợp lệ.");

            string expectedDatabase =
                BackupRestoreHelper.DatabaseName;

            // Kiểm tra database bên trong file backup.
            string sourceDatabase =
                GetBackupDatabaseName(backupPath);

            if (!string.Equals(
                sourceDatabase,
                expectedDatabase,
                StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException(
                    "Database trong file backup là '" +
                    sourceDatabase +
                    "', không phải '" +
                    expectedDatabase +
                    "'. Không thể phục hồi.");
            }

            // Kiểm tra tính toàn vẹn trước khi Restore.
            VerifyBackup(backupPath);

            string database =
                QuoteIdentifier(expectedDatabase);

            bool singleUser = false;

            using (SqlConnection connection = CreateMasterConnection())
            {
                connection.Open();

                try
                {
                    // Ngắt các kết nối khác tới database.
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandTimeout = CommandTimeout;

                        command.CommandText =
                            "ALTER DATABASE " + database +
                            " SET SINGLE_USER WITH ROLLBACK IMMEDIATE;";

                        command.ExecuteNonQuery();

                        singleUser = true;
                    }

                    // Phục hồi toàn bộ database.
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandTimeout = CommandTimeout;

                        command.CommandText =
                            "RESTORE DATABASE " + database +
                            " FROM DISK = @backupPath " +
                            "WITH REPLACE, RECOVERY, CHECKSUM, STATS = 10;";

                        command.Parameters.Add(
                            "@backupPath",
                            SqlDbType.NVarChar,
                            4000).Value = backupPath;

                        command.ExecuteNonQuery();
                    }
                }
                finally
                {
                    // Luôn cố gắng đưa database về MULTI_USER.
                    if (singleUser)
                    {
                        using (SqlCommand command =
                            connection.CreateCommand())
                        {
                            command.CommandTimeout = CommandTimeout;

                            command.CommandText =
                                "ALTER DATABASE " + database +
                                " SET MULTI_USER;";

                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        // =====================================================
        // 7. DANH SÁCH FILE BACKUP
        // =====================================================

        public List<BackupInfoDTO> GetBackupList(string directory)
        {
            List<BackupInfoDTO> result =
                new List<BackupInfoDTO>();

            if (string.IsNullOrWhiteSpace(directory) ||
                !Directory.Exists(directory))
                return result;

            FileInfo[] files;

            try
            {
                files = new DirectoryInfo(directory)
                    .GetFiles("*.bak", SearchOption.TopDirectoryOnly)
                    .OrderByDescending(x => x.LastWriteTime)
                    .ToArray();
            }
            catch (Exception ex)
            {
                throw new IOException(
                    "Không thể đọc thư mục backup.", ex);
            }

            int index = 1;

            foreach (FileInfo file in files)
            {
                result.Add(new BackupInfoDTO
                {
                    STT = index++,
                    FileName = file.Name,
                    FullPath = file.FullName,
                    DirectoryPath = file.DirectoryName ?? string.Empty,
                    BackupTime = file.LastWriteTime,
                    FileSize = file.Length,
                    FileSizeText =
                        BackupRestoreHelper.FormatFileSize(file.Length)
                });
            }

            return result;
        }

        // =====================================================
        // 8. LỊCH SỬ THAO TÁC
        // =====================================================

        public List<BackupHistoryDTO> GetHistory()
        {
            List<BackupHistoryDTO> result =
                new List<BackupHistoryDTO>();

            string path =
                BackupRestoreHelper.GetHistoryFilePath();

            if (!File.Exists(path))
                return result;

            string[] lines = File.ReadAllLines(path);

            foreach (string line in lines.Reverse().Take(100))
            {
                string[] parts = line.Split('\t');

                if (parts.Length < 4)
                    continue;

                DateTime actionTime;

                if (!DateTime.TryParseExact(
                    parts[0],
                    "dd/MM/yyyy HH:mm:ss",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None,
                    out actionTime))
                    continue;

                result.Add(new BackupHistoryDTO
                {
                    ActionTime = actionTime,
                    ActionName = parts[1],
                    Detail = parts[2],
                    Status = parts[3]
                });
            }

            return result;
        }

        public void WriteHistory(
            string action,
            string detail,
            bool success)
        {
            BackupRestoreHelper.AppendHistory(
                action,
                detail,
                success ? "Thành công" : "Thất bại");
        }
    }
}