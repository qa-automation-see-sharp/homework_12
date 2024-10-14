using System.Net;
using LibraryV3.Contracts.Domain;
using Newtonsoft.Json;
using Test.Utils.TestHelpers;

namespace LibraryV3.NUnit.Tests.Api2;

public class GetBooksTests: LibraryTestFixture
{
    private Book _book;

    [OneTimeSetUp]
    public async Task OneTimeSetUpAsync()
    {
        await _libraryHttpService.LogIn(_libraryHttpService.DefaultUser, true);
        await CreateBook();
    }

    [Test, Order(1)]
    [Description("This test checks if the book is found by its title successfully")]

    public async Task GetBookByTitleAsync_ReturnOK()
    {
        var httpResponseMessage = await _libraryHttpService.GetBooksByTitle(_book.Title);
        
        Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test, Order(2)]
    [Description("This test checks if the book is found by its author successfully")]
    public async Task GetBookByAuthorAsync_ReturnOK()
    {
        var httpResponseMessage = await _libraryHttpService.GetBooksByAuthor(_book.Author);
        
        Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    private async Task CreateBook()
    {
        var book = DataHelper.CreateBook();
            
        var httpResponseMessage = 
            await _libraryHttpService.PostBook(_libraryHttpService.DefaultUserAuthToken.Token, book);
        var content = await httpResponseMessage.Content.ReadAsStringAsync();
        _book = JsonConvert.DeserializeObject<Book>(content);
    }
}