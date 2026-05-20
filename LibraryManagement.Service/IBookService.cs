using LibraryManagement.Models;

namespace LibraryManagement.Service;

public interface IBookService
{
    Task<Book> AddBook(Book book);
    Task<List<Book>> GetAllBooks();
    Task<Book?> GetBookById(int bookId);
    Task<List<Book>> SearchBooks(string title);
}
