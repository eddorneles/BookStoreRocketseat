using BookStoreManager.Communication.Requests;
using BookStoreManager.Entities;
using BookStoreManager.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreManager.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BookController : ControllerBase {

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Book), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById( [FromRoute] int id) {
        var retrievedBook = new Book();
        return base.Ok(retrievedBook);
    }

    
    [HttpGet]
    [ProducesResponseType(typeof(List<Book>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(List<Book>), StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetAll()
    {
        var booksRepo = new BookRepository();
        var booksRetrieved = await booksRepo.RetrieveAllAsync();
        if (booksRetrieved.Any()){
            return Ok(booksRetrieved);
        }
        return NoContent();
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(Book), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create( [FromBody] RequestBookDto bookDto ) {

        var book = new Book()
        {
            Author = bookDto.Author,
            Genre = bookDto.Genre,
            Price = bookDto.Price,
            StockAmount = bookDto.StockAmount,
            Title = bookDto.Title
        };
        var bookRepo = new BookRepository();
        await bookRepo.Insert( book );
        return base.Created(string.Empty, book);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] RequestBookDto bookDto ) {
        var bookToUpdate = new Book()
        {
            Id = id,
            Author = bookDto.Author,
            Genre = bookDto.Genre,
            Price = bookDto.Price,
            StockAmount = bookDto.StockAmount,
            Title = bookDto.Title
        };
        var booksRepos = new BookRepository();
        await booksRepos.Update( bookToUpdate );
        return base.NoContent();
    }
    
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Update([FromRoute] int id ) {

        var booksRepos = new BookRepository();
        await booksRepos.Delete( id );
        return base.NoContent();
    }
    
}//END class

