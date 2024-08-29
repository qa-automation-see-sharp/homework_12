using System.Net;
using LibraryV3.Contracts.Domain;
using LibraryV3.NUnit.Tests.Api.Fixtures;
using LibraryV3.NUnit.Tests.Api.TestHelpers;


namespace LibraryV3.NUnit.Tests.Api.Tests;

public class DeleteBookTests : LibraryV3TestFixture
{
    private Book Book { get; set; }

    [SetUp]
    public new async Task SetUp()
    {
        Book = DataHelper.BookHelper.RandomBook();
        await HttpService.PostBook(Book);
    }

    [Test]
    public async Task DeleteBook()
    {
        var response = await HttpService.DeleteBook(Book.Title, Book.Author);
        var jsonString = await response.Content.ReadAsStringAsync();
        var s = jsonString.Trim('"');

        Assert.Multiple(() =>
        {
            Assert.That(s.Equals($"{Book.Title} by {Book.Author} deleted"));
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        });
    }

    [Test]
    public async Task DeleteNotFoundBook()
    {
        var title = "Happy";
        var author = "Chan";
        var response = await HttpService.DeleteBook(title, author);
        var jsonString = await response.Content.ReadAsStringAsync();
        var s = jsonString.Trim('"');

        Assert.Multiple(() =>
        {
            Assert.That(s.Equals($"Book :{title} by {author} not found"));
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        });
    }

    [Test]
    public async Task DeleteBookUnauthorized()
    {
        var response = await HttpService.DeleteBook(Book.Title, Book.Author, "123");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }
}