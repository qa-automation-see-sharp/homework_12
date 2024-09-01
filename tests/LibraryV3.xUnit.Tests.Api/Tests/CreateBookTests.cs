using LibraryV3.Contracts.Domain;
using LibraryV3.Services;
using Newtonsoft.Json;
using System.Net;

namespace LibraryV3.NUnit.Tests.Api.Tests;

public class CreateBookTests : IAsyncLifetime, IClassFixture<LibraryHttpService>
{
    private readonly LibraryHttpService _libraryHttpService;
    private Book _book;

    public CreateBookTests(LibraryHttpService libraryHttpService) {  _libraryHttpService = libraryHttpService; }

    public async Task InitializeAsync()
    {
        await _libraryHttpService.CreateDefaultUser();
        await _libraryHttpService.Authorize();
        _book = DataHelper.CreateBook();
    }

    [Fact]
    public async Task CreateBook_WhenDataIsValid_ReturnCreated()
    {
        HttpResponseMessage response = await _libraryHttpService.CreateBook(_book);

        var jsonString = await response.Content.ReadAsStringAsync();
        var books = JsonConvert.DeserializeObject<Book>(jsonString);

        Assert.Multiple(() =>
        {
            Assert.Equal(response.StatusCode, HttpStatusCode.Created);
            Assert.Equal(books.Title, _book.Title);
            Assert.Equal(books.Author, _book.Author);
            Assert.Equal(books.YearOfRelease, _book.YearOfRelease);
        });
    }

    [Fact]
    public async Task CreateBook_WhenTokenIsInvalid_ReturnUnauthorized()
    {
        HttpResponseMessage response = await _libraryHttpService.CreateBook("invalid", _book);

        var jsonString = await response.Content.ReadAsStringAsync();
        var books = JsonConvert.DeserializeObject<Book>(jsonString);

        Assert.Equal(response.StatusCode, HttpStatusCode.Unauthorized);
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}