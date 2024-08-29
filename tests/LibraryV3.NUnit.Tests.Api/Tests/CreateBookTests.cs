using System.Net;
using LibraryV3.Contracts.Domain;
using LibraryV3.NUnit.Tests.Api.Fixtures;
using LibraryV3.NUnit.Tests.Api.Services;
using Newtonsoft.Json;
using static LibraryV3.NUnit.Tests.Api.TestHelpers.DataHelper;

namespace LibraryV3.NUnit.Tests.Api.Tests;

public sealed class CreateBookTests : LibraryV3TestFixture
{
    [Test]
    public async Task PostBook_ShouldReturnCreated()
    {
        //Arrange
        var book = CreateBook(); 
        
        //Act
        var response = await LibraryHttpService.PostBook(book);
        var bookJsonString = await response.Content.ReadAsStringAsync();
        var createdBook = JsonConvert.DeserializeObject<Book>(bookJsonString);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            Assert.That(createdBook.Title, Is.EqualTo(book.Title));
            Assert.That(createdBook.Author, Is.EqualTo(book.Author));
            Assert.That(createdBook.YearOfRelease, Is.EqualTo(book.YearOfRelease));
        });
    }

    [Test]
    public async Task PostBook_AlreadyExists_ShouldReturnBadRequest()
    {
        //Arrange
        var book = CreateBook();    

        await LibraryHttpService.PostBook(book);

        //Act
        var response = await LibraryHttpService.PostBook(book);

        //Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task PostBook_ShouldReturnUnauthorized()
    {
        //Arrange
        var book = CreateBook();

        //Arrange
        var httpResponseMessage = await LibraryHttpService.PostBook(Guid.NewGuid().ToString(), book);

        //Assert
        Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }
}