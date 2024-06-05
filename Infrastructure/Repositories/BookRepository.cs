using System.Data;
using BookStoreManager.Entities;
using Microsoft.Data.Sqlite;
using Microsoft.Data;

namespace BookStoreManager.Repositories;

public class BookRepository
{

    private readonly string LocalDB = "Data Source=local-database.db";
    
    public async Task<Book> Insert(Book book) {
        var insertQuery = @"INSERT INTO book ( title, author, genre, price, stock_amount ) VALUES 
                                ($title, $author, $genre, $price, $stock_amount);
                            SELECT last_insert_rowid();";

        await using var dbConnection = new SqliteConnection(this.LocalDB);
        await dbConnection.OpenAsync();
        var command = dbConnection.CreateCommand();
        this.SetParametersToInsertOrUpdate( command.Parameters, book );

        command.CommandText = insertQuery;
        int registeredBookId = Convert.ToInt16( await command.ExecuteScalarAsync() );
        book.Id = registeredBookId;

        return book;
    }

    private void SetParametersToInsertOrUpdate( SqliteParameterCollection parameters, Book book )
    {
        parameters.AddWithValue("$title", book.Title);
        parameters.AddWithValue("$genre", book.Genre);
        parameters.AddWithValue("$author", book.Author);
        parameters.AddWithValue("$price", book.Price);
        parameters.AddWithValue("$stock_amount", book.StockAmount);
    }

    public async Task Update(Book book) {
        var updateQuery = @"
            UPDATE book SET 
                title = $title,
                author = $author,
                genre = $genre,
                price = $price,
                stock_amount = $stock_amount 
            WHERE id = $id;";

        await using var dbConnection = new SqliteConnection(this.LocalDB);
        await dbConnection.OpenAsync();
        var command = dbConnection.CreateCommand();
        this.SetParametersToInsertOrUpdate( command.Parameters, book );
        command.Parameters.AddWithValue($"id", book.Id);
        command.CommandText = updateQuery;
        await command.ExecuteNonQueryAsync();
        
    }

    public async Task<List<Book>> RetrieveAllAsync() {
        var selectQuery = "SELECT id,title,genre,author,price,stock_amount FROM book;";
        await using var dbConnection = new SqliteConnection(this.LocalDB);
        await dbConnection.OpenAsync();
        var command = dbConnection.CreateCommand();
        command.CommandText = selectQuery;

        var retrievedBooks = new List<Book>();
        var dbReader = await command.ExecuteReaderAsync();
        while (dbReader.Read()){
            var book = new Book() {
                Id = Convert.ToInt32( dbReader["id"] ),
                Author = (string) dbReader["author"],
                Genre = (string) dbReader["genre"],
                Title = (string) dbReader["title"],
                Price= Convert.ToDecimal( dbReader["price"] ),
                StockAmount = Convert.ToInt32( dbReader["stock_amount"] )
            };
            retrievedBooks.Add(book);
        }
        return retrievedBooks;
    }
    
    public async Task<Book> RetrieveById( int id ) {
        var selectQuery = "SELECT id,title,genre,author,price,stock_amount FROM book WHERE id = $id;";
        await using var dbConnection = new SqliteConnection(this.LocalDB);
        await dbConnection.OpenAsync();
        var command = dbConnection.CreateCommand();
        command.CommandText = selectQuery;
        command.Parameters.AddWithValue($"$id", id);
        var dbReader = await command.ExecuteReaderAsync();
        var book = new Book() {
            Id = Convert.ToInt32( dbReader["id"] ),
            Author = (string) dbReader["author"],
            Genre = (string) dbReader["genre"],
            Title = (string) dbReader["title"],
            Price =  Convert.ToDecimal( dbReader["price"]),
            StockAmount = Convert.ToInt32( dbReader["stock_amount"] )
        };
        return book;
    }
    
    public async Task<bool> Delete(int id) {
        var deleteQuery = "DELETE FROM book WHERE id = $id;";
        await using var dbConnection = new SqliteConnection(this.LocalDB);
        await dbConnection.OpenAsync();
        var command = dbConnection.CreateCommand();
        command.CommandText = deleteQuery;
        command.Parameters.AddWithValue($"$id", id);
        var dbReader = await command.ExecuteReaderAsync();
        return true;
    }
    
}
