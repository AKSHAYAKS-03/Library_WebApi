using LibraryManagement.DTOs;
using LibraryManagement.Models;
using LibraryManagement.Service;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.API.Controllers;

[ApiController]
[Route("api/books")]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;

    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }

    [HttpPost]
    public async Task<IActionResult> AddBook([FromBody] CreateBookRequest request)
    {
        try
        {
            var book = new Book
            {
                Title = request.Title?.Trim() ?? string.Empty,
                Author = request.Author?.Trim() ?? string.Empty,
                ISBN = request.ISBN?.Trim() ?? string.Empty,
                PublishedYear = request.PublishedYear,
                AvailableCopies = request.AvailableCopies
            };

            var created = await _bookService.AddBook(book);
            return CreatedAtAction(nameof(GetBookById), new { id = created.BookId }, new { message = "Book added successfully" });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllBooks()
    {
        var books = await _bookService.GetAllBooks();
        return Ok(books);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetBookById(int id)
    {
        var book = await _bookService.GetBookById(id);
        if (book is null)
            return NotFound(new { message = "Book not found." });

        return Ok(book);
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchBooks([FromQuery] string title)
    {
        try
        {
            var books = await _bookService.SearchBooks(title);
            return Ok(books);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}