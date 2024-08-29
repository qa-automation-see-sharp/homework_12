using System.Net;
using LibraryV2.Models;
using LibraryV2.Tests.Api.Fixtures;
using LibraryV2.Tests.Api.Services;
using Newtonsoft.Json;
using static LibraryV2.Tests.Api.TestHelpers.DataHelper;

namespace LibraryV2.Tests.Api.Tests;

public class DeleteBookTests : LibraryV2TestFixture
{
    [Test]
    public async Task DeleteBook_ShouldReturnOK()
    {
        //Arrange
        var book = CreateBook();
        await LibraryHttpService.PostBook(book);

        //Act
        var response = await LibraryHttpService.DeleteBook(book.Title, book.Author);

        //Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task DeleteBook_NotExistingBook_ShouldReturnNotFound()
    {
        //Arrange - N/A

        //Act
        var httpResponseMessage = await LibraryHttpService.DeleteBook(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

        //Assert
        Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }
}