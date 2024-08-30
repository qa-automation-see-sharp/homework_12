using LibraryV3.xUnit.Tests.Api.Fixtures;
using LibraryV3.Contracts.Domain;
using Newtonsoft.Json;
using System.Net;
using LibraryV3.xUnit.Tests.Api.TestHelpers;
using Microsoft.AspNetCore.Mvc.Testing;
using LibraryV3.xUnit.Tests.Api.Services;

namespace LibraryV3.xUnit.Tests.Api.Tests;

public class GetBooksTests : IAsyncLifetime, IClassFixture<WebApplicationFactory<LibraryV3TestFixture>>
{
    private Book Book { get; set; }
    private LibraryHttpService HttpService;
    private readonly WebApplicationFactory<IApiMarker> _factory = new();


    public async Task InitializeAsync()
    {
        Book = DataHelper.BookHelper.RandomBook();

        var client = _factory.CreateClient();
        HttpService = new LibraryHttpService(client);
        await HttpService.CreateDefaultUser();
        await HttpService.Authorize();
        await HttpService.PostBook(Book);
    }

    [Fact]
    public async Task GetBooksByTitle()
    {
        var response = await HttpService.GetBooksByTitle(Book.Title);
        var listStringBooks = await response.Content.ReadAsStringAsync();
        var json = JsonConvert.DeserializeObject<List<Book>>(listStringBooks);


        Assert.Multiple(() =>
        {
            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(json);
            Assert.NotNull(response);
            Assert.Equal(json[0].Title, Book.Title);
            Assert.Equal(json[0].Author, Book.Author);
            Assert.Equal(json[0].YearOfRelease, Book.YearOfRelease);
        });
    }

    [Fact]
    public async Task BookNotFoundByTitle()
    {

        var book = DataHelper.BookHelper.BookWithTitleAuthorYear("Not Found", "Not Found Author", 1990);
        var response = await HttpService.GetBooksByTitle(book.Title);
        var message = await response.Content.ReadAsStringAsync();
        var s = message.Trim('"');

        Assert.Multiple(() =>
        {
            Assert.Equal(response.StatusCode, HttpStatusCode.NotFound);
            Assert.True(s.Equals(DataHelper.ErrorMessage.NotFoundBook(book)));
        });
    }

    [Fact]
    public async Task GetBooksByAuthor()
    {
        var response = await HttpService.GetBooksByAuthor(Book.Author);
        var listStringBooks = await response.Content.ReadAsStringAsync();
        var json = JsonConvert.DeserializeObject<List<Book>>(listStringBooks);

        Assert.Multiple(() =>
        {
            Assert.Equal(response.StatusCode, HttpStatusCode.OK);
            Assert.NotNull(response);
            Assert.Equal(json[0].Title, Book.Title);
            Assert.Equal(json[0].Author, Book.Author);
            Assert.Equal(json[0].YearOfRelease, Book.YearOfRelease);
        });
    }

    public async Task DisposeAsync()
    {
        await HttpService.DeleteBook(Book.Title, Book.Author);
    }
}