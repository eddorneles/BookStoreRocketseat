using Microsoft.Data.Sqlite;

namespace BookStoreManager.Repositories;

public abstract class BaseRepository
{
    public SqliteConnection OpenConnection()
    {
        using var connection = new SqliteConnection( "Data Source=local-database.db");
        connection.OpenAsync();
        return connection;
    }
}