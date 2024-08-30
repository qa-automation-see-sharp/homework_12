using System.Net;
using LibraryV3.Contracts.Domain;
using LibraryV3.xUnit.Tests.Api.Fixtures;
using LibraryV3.xUnit.Tests.Api.Services;
using LibraryV3.xUnit.Tests.Api.TestHelpers;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;

namespace LibraryV3.xUnit.Tests.Api.Tests;

public class CreateBookTests : IAsyncLifetime, IClassFixture<WebApplicationFactory<LibraryV3TestFixture>>
{
    private LibraryHttpService _httpService;
    private Book _book;
    private readonly WebApplicationFactory<IApiMarker> _factory = new();

    public async Task InitializeAsync()
    {
        var client =_factory.CreateClient();
        _httpService = new LibraryHttpService(client);
        await _httpService.CreateDefaultUser();
        await _httpService.Authorize();
    }

    [Theory]
    [InlineData("Philosopher's Stone", "Joanne Rowling", 1997)]
    [InlineData("Chamber of Secrets", "Joanne Rowling", 1998)]
    [InlineData("Prisoner of Azkaban", "Joanne Rowling", 1999)]
    [InlineData("Goblet of Fire ", "Joanne Rowling", 2000)]
    [InlineData("Order of the Phoenix", "Joanne Rowling", 2003)]
    [InlineData("Half-Blood Prince", "Joanne Rowling", 2005)]
    public async Task CreateBook(string title, string author, int year)
    {
        _book = DataHelper.BookHelper.BookWithTitleAuthorYear(title, author, year);

        var obj = await _httpService.PostBook(_book);
        var response = await obj.Content.ReadAsStringAsync();
        var bookObj = JsonConvert.DeserializeObject<Book>(response);

        Assert.Multiple(() =>
        {
            Assert.Equal(HttpStatusCode.Created, obj.StatusCode);
            Assert.Equal(_book.Title, bookObj.Title);
            Assert.Equal(_book.Author, bookObj.Author);
            Assert.Equal(_book.YearOfRelease, bookObj.YearOfRelease);
        });
    }

    [Fact]
    public new async Task CreateExistedBook()
    {
        _book = DataHelper.BookHelper.RandomBook();
        
        await _httpService.PostBook(_book);
        var obj = await _httpService.PostBook(_book);
        var response = await obj.Content.ReadAsStringAsync();
        var s = response.Trim('"'); 

        Assert.Multiple(() =>
       {
           Assert.Equal(obj.StatusCode, HttpStatusCode.BadRequest);
           Assert.NotNull(response);
           Assert.Contains(DataHelper.ErrorMessage.ExistBook(_book), s);
       });
    }

    public new async Task DisposeAsync()
    {
        await _httpService.DeleteBook(_book.Title, _book.Author);
    }
}