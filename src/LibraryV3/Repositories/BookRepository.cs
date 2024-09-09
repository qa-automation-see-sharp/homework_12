using LibraryV3.Contracts.Domain;

namespace LibraryV3.Repositories;

public class BookRepository : IBookRepository
{
    private readonly ILogger<BookRepository> _logger;
    public void AddBook(Book book)
    {
        _books.Add(book);
    }

    private readonly List<Book> _books = new();

    public BookRepository(ILogger<BookRepository> logger)
    {
        _logger = logger;
    }

    public Book? GetBook(Func<Book, bool> condition)
    {
        var book = _books.FirstOrDefault(condition);
        Console.WriteLine(book != null ? $"Book found: {book.Title} by {book.Author}" : "Book not found");
        return book;
        
    }


    public List<Book> GetMany(Func<Book, bool> condition)
    {
        var books = _books.Where(condition).ToList();
        Console.WriteLine(books.Any() ? $"Found {books.Count} book(s)" : "No books found"); 
        return books;
    }

    public bool Delete(Func<Book, bool> condition)
    {
        var bookToRemove = _books.FirstOrDefault(condition);
        if (bookToRemove is null)
        {
            Console.WriteLine("Book to delete not found");
            return false;
        }

        _books.Remove(bookToRemove);
        Console.WriteLine($"Deleted book: {bookToRemove.Title} by {bookToRemove.Author}");
        return true;
    }

    public bool Exists(Book book)
    {
        return _books.Exists(b => b.Title == book.Title && b.Author == book.Author);
    }

    public List<Book> GetAll()
    {
        return _books.ToList();
    }
}