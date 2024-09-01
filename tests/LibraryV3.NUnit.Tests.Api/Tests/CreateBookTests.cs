using LibraryV3.Contracts.Domain;
using LibraryV3.Services;
using Newtonsoft.Json;
using System.Net;

namespace LibraryV3.NUnit.Tests.Api.Tests;

public class CreateBookTests
{
    private LibraryHttpService _libraryHttpService;
    private Book _book;

    [OneTimeSetUp]
    public new async Task OneTimeSetUp()
    {
        _libraryHttpService = new LibraryHttpService();
        await _libraryHttpService.CreateDefaultUser();
        await _libraryHttpService.Authorize();
        _book = DataHelper.CreateBook();
    }

    [Test]
    public async Task CreateBook_WhenDataIsValid_ReturnCreated()
    {
        HttpResponseMessage response = await _libraryHttpService.CreateBook(_book);

        var jsonString = await response.Content.ReadAsStringAsync();
        var books = JsonConvert.DeserializeObject<Book>(jsonString);

        Assert.Multiple(() =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            Assert.That(books.Title, Is.EqualTo(_book.Title));
            Assert.That(books.Author, Is.EqualTo(_book.Author));
            Assert.That(books.YearOfRelease, Is.EqualTo(_book.YearOfRelease));
        });
    }

    [Test]
    public async Task CreateBook_WhenTokenIsInvalid_ReturnUnauthorized()
    {
        HttpResponseMessage response = await _libraryHttpService.CreateBook("invalid", _book);

        var jsonString = await response.Content.ReadAsStringAsync();
        var books = JsonConvert.DeserializeObject<Book>(jsonString);

        Assert.Multiple(() =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        });
    }
}