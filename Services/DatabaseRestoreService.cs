using Microsoft.Data.SqlClient;

namespace QuanLyCBCS.Services;

public class DatabaseRestoreService
{
    private readonly string _connectionString;

    public DatabaseRestoreService(
        string connectionString)
    {
        _connectionString =
            connectionString;
    }

    public async Task RestoreDatabaseAsync(
        string backupFilePath)
    {
        string sql =
            $@"
USE master;

ALTER DATABASE QuanLyCBCS
SET SINGLE_USER
WITH ROLLBACK IMMEDIATE;

RESTORE DATABASE QuanLyCBCS
FROM DISK = N'{backupFilePath}'
WITH REPLACE;

ALTER DATABASE QuanLyCBCS
SET MULTI_USER;
";

        using SqlConnection conn =
            new SqlConnection(
                _connectionString);

        await conn.OpenAsync();

        using SqlCommand cmd =
            new SqlCommand(
                sql,
                conn);

        cmd.CommandTimeout = 300;

        await cmd.ExecuteNonQueryAsync();
    }
}