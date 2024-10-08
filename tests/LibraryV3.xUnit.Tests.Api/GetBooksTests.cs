using System.Net;
using LibraryV3.Contracts.Domain;
using LibraryV3.Fixtures;
using LibraryV3.Services;
using LibraryV3.TestHelpers;
using Newtonsoft.Json;

namespace LibraryV3.xUnit.Tests.Api;

public class GetBooksTests 
{
    public class GetBooksTests : LibraryTestFixture
    {
        private Book _book;
        private readonly ILibraryHttpService _libraryHttpService;

        public GetBooksTests()
        {
            _libraryHttpService = new LibraryHttpService();
            Initialize().Wait();
        }

        private async Task Initialize()
        {
            await _libraryHttpService.LogIn(_libraryHttpService.DefaultUser, true);
            await CreateBook();
        }

        [Fact]
        public async Task GetBookByTitleAsync_ReturnOK()
        {
            // Act
            var httpResponseMessage = await _libraryHttpService.GetBooksByTitle(_book.Title);
        
            // Assert
            Assert.Equal(HttpStatusCode.OK, httpResponseMessage.StatusCode);
        }

        [Fact]
        public async Task GetBookByAuthorAsync_ReturnOK()
        {
            // Act
            var httpResponseMessage = await _libraryHttpService.GetBooksByAuthor(_book.Author);
        
            // Assert
            Assert.Equal(HttpStatusCode.OK, httpResponseMessage.StatusCode);
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
}