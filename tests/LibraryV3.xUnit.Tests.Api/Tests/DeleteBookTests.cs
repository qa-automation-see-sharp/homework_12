using System.Net;
using LibraryV3.Contracts.Domain;
using LibraryV3.xUnit.Tests.Api.Services;
using Newtonsoft.Json;
using LibraryV3.xUnit.Tests.Api.TestHelpers;

namespace LibraryV3.xUnit.Tests.Api.Tests;

public class DeleteBookTests : IAsyncLifetime, IClassFixture<LibraryHttpService>
{
    private readonly LibraryHttpService _libraryHttpService;   

    public DeleteBookTests(LibraryHttpService libraryHttpService)
    {
        _libraryHttpService = libraryHttpService;
    }

    public async Task InitializeAsync()
    {   
        //Arrange
        await _libraryHttpService.CreateDefaultUser();
        await _libraryHttpService.Authorize();        
    }

    [Fact]
    public async Task DeleteBook_ShouldReturnOK()
    {
        //Arrange
        var book = DataHelper.CreateBook();
        await _libraryHttpService.PostBook(book);

        //Act
        var response = await _libraryHttpService.DeleteBook(book.Title, book.Author);

        //Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task DeleteBook_NotExistingBook_ShouldReturnNotFound()
    {
        //Arrange - N/A

        //Act
        var httpResponseMessage = await _libraryHttpService.DeleteBook(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

        //Assert
        Assert.Equal(HttpStatusCode.NotFound, httpResponseMessage.StatusCode);
    }

    public async Task DisposeAsync()
    {
    }
}