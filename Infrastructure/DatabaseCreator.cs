using Microsoft.Data.Sqlite;

namespace BookStoreManager.Infrastructure;

public static class DatabaseCreator
{
    public static void Create()
    {
        using var connection = new SqliteConnection( "Data Source=local-database.db");
        connection.OpenAsync();
        var command = connection.CreateCommand();
        command.CommandText = 
            @"
                    CREATE TABLE IF NOT EXISTS book(
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        title TEXT, 
                        author TEXT,
                        genre TEXT,
                        price REAL,
                        stock_amount INTEGER
                    );
                ";
        command.ExecuteNonQuery();
        connection.CloseAsync();
    }
}