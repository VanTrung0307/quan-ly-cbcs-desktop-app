using Microsoft.Data.SqlClient;
using QuanLyCBCS.Helpers;

namespace QuanLyCBCS.Services;

public class DatabaseBackupService
{
    private readonly string _connectionString;

    public DatabaseBackupService(
        string connectionString)
    {
        _connectionString =
            connectionString;
    }

    public async Task BackupDatabaseAsync(
        string backupFilePath)
    {
        PermissionHelper.CheckAdmin();

        string sql =
            $"""
            BACKUP DATABASE QuanLyCBCS
            TO DISK = N'{backupFilePath}'
            WITH INIT
            """;

        using SqlConnection conn =
            new SqlConnection(
                _connectionString);

        await conn.OpenAsync();

        using SqlCommand cmd =
            new SqlCommand(
                sql,
                conn);

        await cmd.ExecuteNonQueryAsync();
    }
}