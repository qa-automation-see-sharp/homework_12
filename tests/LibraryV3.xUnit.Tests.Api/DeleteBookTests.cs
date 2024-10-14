using System.Net;
using LibraryV3.Contracts.Domain;
using LibraryV3.NUnit.Tests.Api2;
using Newtonsoft.Json;
using Test.Utils;
using Test.Utils.TestHelpers;

namespace LibraryV3.xUnit.Tests.Api;

public class DeleteBookTests : IAsyncLifetime, IClassFixture<LibraryHttpService>
{
    private readonly LibraryHttpService _libraryHttpService;
    public DeleteBookTests(LibraryHttpService libraryHttpService)
    {
        _libraryHttpService = libraryHttpService;
    }
    
    public async Task InitializeAsync()
    {
        await _libraryHttpService.LogIn(_libraryHttpService.DefaultUser, true);
    }

    [Fact]
    public async Task DeleteBookAsync_ReturnOK()
    {
        // Arrange
        var book = DataHelper.CreateBook();

        var httpResponseMessage =
            await _libraryHttpService.PostBook(_libraryHttpService.DefaultUserAuthToken.Token, book);
        var content = await httpResponseMessage.Content.ReadAsStringAsync();
        var bookFromResponse = JsonConvert.DeserializeObject<Book>(content);

        // Act
        var deleteResponseMessage =
            await _libraryHttpService
                .DeleteBook(_libraryHttpService.DefaultUserAuthToken.Token, bookFromResponse.Title,
                    bookFromResponse.Author);

        // Assert
        Assert.Equal(HttpStatusCode.OK, deleteResponseMessage.StatusCode);
    }
    
    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}