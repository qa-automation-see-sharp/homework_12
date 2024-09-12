using LibraryV3.Contracts.Domain;
using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;
using LibraryV3.xUnit.Tests.Api.Tests.TestHelpers;
using LibraryV3.Services;
using Newtonsoft.Json;


namespace LibraryV3.xUnit.Tests.Api.Tests
{
    public class XUCreateBookTests : IAsyncLifetime, IClassFixture<LibraryHttpService>
    {
        private string _token = string.Empty;
        private Book _book;
        private readonly TestLoggerHelper _logger;
        private LibraryHttpService _libraryService;

        //setup#1
        public XUCreateBookTests(LibraryHttpService libraryHttpService, ITestOutputHelper output)
        {
            _libraryService = new LibraryHttpService();
            _logger = new TestLoggerHelper(output);
        }

        //setup2
        public async Task InitializeAsync()
        {
            await _libraryService.CreateTestUser();
            await _libraryService.LoginTestUser();
            await _libraryService.CreateTestBook();
        }

        //Tests
        //Create Book without authorization token return unauthorized
        [Fact]
        public async Task CreateBook_IfUnauthorazedUser_Return_Unauthorized()
        {
            //Arrange
            _book = BookHelpers.CreateBook();
            await _libraryService.CreateBook(_token, _book);

            //Act
            HttpResponseMessage response = await _libraryService.CreateBook(_token, _book);
            var jsonResult = await response.Content.ReadAsStringAsync();

            //Asserts
            Assert.Multiple(() =>
            {
                Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
                Assert.NotNull(jsonResult);
            });

            //Log output to console
            LoggerHelper.Logger.Information($"Response Status Code: {response.StatusCode}");
            LoggerHelper.Logger.Information($"User JSON String: {jsonResult}");


            //Log output to test output
            _logger.LogInformation($"Response Status Code: {response.StatusCode}");
            _logger.LogInformation($"User JSON String: {jsonResult}");
        }

        //Create Book with new Title and new Author return created
        [Fact]
        public async Task CreateBook_IfNewTitleNewAuthor_Return_Created()
        {
            //Arrange
            _book = BookHelpers.CreateBook();

            //Act
            HttpResponseMessage response = await _libraryService.CreateBook(_book);
            var jsonResult = await response.Content.ReadAsStringAsync();
            var newBook = JsonConvert.DeserializeObject<Book>(jsonResult);

            //Asserts
            Assert.Multiple(() =>
            {
                Assert.Equal(HttpStatusCode.Created, response.StatusCode);
                Assert.NotNull(jsonResult);
                Assert.Equal(newBook.Title, _book.Title);
                Assert.Equal(newBook.Author, _book.Author);
                Assert.Equal(newBook.YearOfRelease, _book.YearOfRelease);
            });

            //Log output to console
            LoggerHelper.Logger.Information($"Response Status Code: {response.StatusCode}");
            LoggerHelper.Logger.Information($"User JSON String: {jsonResult}");


            //Log output to test output
            _logger.LogInformation($"Response Status Code: {response.StatusCode}");
            _logger.LogInformation($"User JSON String: {jsonResult}");
        }

        //Create Book with existing Title and existing Author return conflict
        [Fact]
        public async Task CreateBook_IfExistTitleExistAuthor_Return_BadRequest()
        {
            //Arrange
            _book = BookHelpers.CreateExistBook();

            //Act
            HttpResponseMessage response = await _libraryService.CreateBook(_book);
            var jsonResult = await response.Content.ReadAsStringAsync();

            //Assert
            Assert.Multiple(() =>
            {
                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
                Assert.NotNull(jsonResult);
            });

            //Log output to console
            LoggerHelper.Logger.Information($"Response Status Code: {response.StatusCode}");
            LoggerHelper.Logger.Information($"User JSON String: {jsonResult}");


            //Log output to test output
            _logger.LogInformation($"Response Status Code: {response.StatusCode}");
            _logger.LogInformation($"User JSON String: {jsonResult}");
        }

        //Create Book with existing Title and new Author return created
        [Fact]
        public async Task CreateBook_IfExistTitleNewAuthor_Return_Created()
        {
            //Arrange
            _book = BookHelpers.CreateExistBook();
            _book.Author = "Test Author" + Guid.NewGuid().ToString();

            //Act
            HttpResponseMessage response = await _libraryService.CreateBook(_book);
            var jsonResult = await response.Content.ReadAsStringAsync();
            var newBook = JsonConvert.DeserializeObject<Book>(jsonResult);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.Equal(HttpStatusCode.Created, response.StatusCode);
                Assert.NotNull(jsonResult);
                Assert.Equal(newBook.Title, _book.Title);
                Assert.Equal(newBook.Author, _book.Author);
                Assert.Equal(newBook.YearOfRelease, _book.YearOfRelease);
            });

            //Log output to console
            LoggerHelper.Logger.Information($"Response Status Code: {response.StatusCode}");
            LoggerHelper.Logger.Information($"User JSON String: {jsonResult}");

            //Log output to test output
            _logger.LogInformation($"Response Status Code: {response.StatusCode}");
            _logger.LogInformation($"User JSON String: {jsonResult}");
        }

        //Create Book with new Title and existing Author return created
        [Fact]
        public async Task CreateBook_IfNewTitleExistAuthor_Return_Created()
        {
            //Arrange
            _book = BookHelpers.CreateExistBook();
            _book.Title = "Test Book" + Guid.NewGuid().ToString();

            //Act
            HttpResponseMessage response = await _libraryService.CreateBook(_book);
            var jsonResult = await response.Content.ReadAsStringAsync();
            var newBook = JsonConvert.DeserializeObject<Book>(jsonResult);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.Equal(HttpStatusCode.Created, response.StatusCode);
                Assert.NotNull(jsonResult);
                Assert.Equal(newBook.Title, _book.Title);
                Assert.Equal(newBook.Author, _book.Author);
                Assert.Equal(newBook.YearOfRelease, _book.YearOfRelease);
            });

            //Log output to console
            LoggerHelper.Logger.Information($"Response Status Code: {response.StatusCode}");
            LoggerHelper.Logger.Information($"User JSON String: {jsonResult}");

            //Log output to test output
            _logger.LogInformation($"Response Status Code: {response.StatusCode}");
            _logger.LogInformation($"User JSON String: {jsonResult}");
        }

        //Teardown
        public async Task DisposeAsync()
        {
            //Act
        }
    }
}
