using Microsoft.Data.SqlClient;

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
        string sql =
            $"""
            BACKUP DATABASE QuanLyCBCS_New
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