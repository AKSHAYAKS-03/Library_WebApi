using LibraryManagement.Models;

namespace LibraryManagement.Repository;

public interface IBookRepository
{
    Task<Book> Add(Book book);
    Task<List<Book>> GetAll();
    Task<Book?> GetById(int bookId);
    Task<List<Book>> SearchByTitle(string title);
}
