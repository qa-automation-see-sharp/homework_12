using LibraryV3.xUnit.Tests.Api.Tests.TestHelpers;
using LibraryV3.Services;
using System;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using Newtonsoft.Json;
using Xunit.Sdk;

namespace LibraryV3.xUnit.Tests.Api.Tests
{
    public class XUDeleteBookTests : IAsyncLifetime, IClassFixture<LibraryHttpService>
    {
        private string _token = string.Empty;
        private readonly TestLoggerHelper _logger;
        private LibraryHttpService _libraryService;

        //Setup#1
        public XUDeleteBookTests(LibraryHttpService libraryHttpServise, ITestOutputHelper output)
        {
            _libraryService = new LibraryHttpService();
            _logger = new TestLoggerHelper(output);
        }

        //Setup#2
        public async Task InitializeAsync()
        {
            await _libraryService.CreateTestUser();
            await _libraryService.LoginTestUser();
            await _libraryService.CreateLibrary();
        }

        //Tests
        [Fact]
        public async Task DeleteBook_IfExistInLibrary_return_Ok()
        {
            //Arrange
            var book = _libraryService.Library.FirstOrDefault()?.FirstOrDefault();

            //Act
            HttpResponseMessage response = await _libraryService.DeleteBook(book.Title, book.Author);
            var jsonResponse = await response.Content.ReadAsStringAsync();

            //Asserts
            Assert.Multiple(() =>
            {
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.NotNull(jsonResponse);
            }
            );
        }

        [Fact]
        public async Task DeleteBook_IfNotExistInLibrary_return_NotFound()
        {
            //Arrange
            var book = BookHelpers.CreateBook();

            //Act
            HttpResponseMessage response = await _libraryService.DeleteBook(book.Title, book.Author);
            var jsonResponse = await response.Content.ReadAsStringAsync();

            //Asserts
            Assert.Multiple(() =>
            {
                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
                Assert.NotNull(jsonResponse);
            }
            );
        }

        //Teardown
        public async Task DisposeAsync()
        {
            //Act
        }

    }

}
