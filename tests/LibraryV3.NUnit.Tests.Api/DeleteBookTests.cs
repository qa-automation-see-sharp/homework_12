using System.Net;
using LibraryV3.Contracts.Domain;
using LibraryV3.Fixtures;
using LibraryV3.TestHelpers;
using Newtonsoft.Json;

namespace LibraryV3.NUnit.Tests.Api;

public class DeleteBookTests: LibraryTestFixture
{
    [OneTimeSetUp]
    public async Task OneTimeSetUpAsync()
    {
        await _libraryHttpService.LogIn(_libraryHttpService.DefaultUser, true);
    }

    [Test]
    [Description("This test checks if the book is deleted successfully")]
    public async Task DeleteBookAsync_ReturnOK()
    {
        var book = DataHelper.CreateBook();
            
        var httpResponseMessage = 
            await _libraryHttpService.PostBook(_libraryHttpService.DefaultUserAuthToken.Token, book);
        var content = await httpResponseMessage.Content.ReadAsStringAsync();
        var bookFromResponse = JsonConvert.DeserializeObject<Book>(content);
        
        var deleteResponseMessage = 
            await _libraryHttpService
                .DeleteBook(_libraryHttpService.DefaultUserAuthToken.Token, bookFromResponse.Title,
                    bookFromResponse.Author);
        
        Assert.That(deleteResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }
}