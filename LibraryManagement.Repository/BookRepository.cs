using LibraryManagement.Data;
using LibraryManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Repository;

public class BookRepository : IBookRepository
{
    private readonly LibraryDbContext _context;

    public BookRepository(LibraryDbContext context)
    {
        _context = context;
    }


    public async Task<Book> Add(Book book)
    {
        _context.Books.Add(book);
        await _context.SaveChangesAsync();
        return book;
    }

    public Task<List<Book>> GetAll()
    {
        return _context.Books
            .OrderBy(x => x.BookId)
            .ToListAsync();
    }

    public Task<Book?> GetById(int bookId)
    {
        return _context.Books.FirstOrDefaultAsync(x => x.BookId == bookId);
    }

    public Task<List<Book>> SearchByTitle(string title)
    {
        return _context.Books
            .Where(x => x.Title.Contains(title))
            .OrderBy(x => x.Title)
            .ToListAsync();
    }
}
