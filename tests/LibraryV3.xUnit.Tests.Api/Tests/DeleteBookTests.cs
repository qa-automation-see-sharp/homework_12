using System.Net;
using LibraryV3.Contracts.Domain;
using LibraryV3.xUnit.Tests.Api.Fixtures;
using LibraryV3.xUnit.Tests.Api.Services;
using LibraryV3.xUnit.Tests.Api.TestHelpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Testing;

namespace LibraryV3.xUnit.Tests.Api.Tests;

public class DeleteBookTests : IAsyncLifetime, IClassFixture<WebApplicationFactory<LibraryV3TestFixture>>
{
    private Book Book { get; set; }
    private LibraryHttpService HttpService;
    private readonly WebApplicationFactory<IApiMarker> _factory = new();

    public async Task InitializeAsync()
    {
        Book = DataHelper.BookHelper.RandomBook();
        var client =_factory.CreateClient();
        HttpService = new LibraryHttpService(client);
        await HttpService.CreateDefaultUser();
        await HttpService.Authorize();
        await HttpService.PostBook(Book);
    }

    [Fact]
    public async Task DeleteBook()
    {
        var response = await HttpService.DeleteBook(Book.Title, Book.Author);
        var jsonString = await response.Content.ReadAsStringAsync();
        var s = jsonString.Trim('"');

        Assert.Multiple(() =>
        {
            Assert.Equal($"{Book.Title} by {Book.Author} deleted", s);
            Assert.True(response.StatusCode == HttpStatusCode.OK);
        });
    }

    [Fact]
    public async Task DeleteNotFoundBook()
    {
        var title = "Happy";
        var author = "Chan";
        var response = await HttpService.DeleteBook(title, author);
        var jsonString = await response.Content.ReadAsStringAsync();
        var s = jsonString.Trim('"');

        Assert.Multiple(() =>
        {
            Assert.Equal($"Book :{title} by {author} not found", s);
            Assert.True(response.StatusCode == HttpStatusCode.NotFound);
        });
    }

    [Fact]
    public async Task DeleteBookUnauthorized()
    {
        var response = await HttpService.DeleteBook(Book.Title, Book.Author, "123");

        Assert.True(response.StatusCode == HttpStatusCode.Unauthorized);
    }

    public async Task DisposeAsync()
    {
    }
}