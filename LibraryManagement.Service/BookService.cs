using LibraryManagement.Models;
using LibraryManagement.Repository;

namespace LibraryManagement.Service;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;

    public BookService(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<Book> AddBook(Book book)
    {
        ValidateBook(book);
        return await _bookRepository.Add(book);
    }

    public Task<List<Book>> GetAllBooks()
    {
        return _bookRepository.GetAll();
    }

    public Task<Book?> GetBookById(int bookId)
    {
        return _bookRepository.GetById(bookId);
    }

    public Task<List<Book>> SearchBooks(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title should not be empty.");

        return _bookRepository.SearchByTitle(title.Trim());
    }

    private static void ValidateBook(Book book)
    {
        if (book is null)
            throw new ArgumentNullException(nameof(book));

        if (string.IsNullOrWhiteSpace(book.Title))
            throw new ArgumentException("Book title should not be empty.");

        if (string.IsNullOrWhiteSpace(book.Author))
            throw new ArgumentException("Author name should not be empty.");

        if (string.IsNullOrWhiteSpace(book.ISBN))
            throw new ArgumentException("ISBN should not be empty.");

        if (book.AvailableCopies < 0)
            throw new ArgumentException("Available copies should be greater than or equal to 0.");
    }
}
