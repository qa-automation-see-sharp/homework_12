using LibraryV3.Contracts.Domain;
using LibraryV3.Services;
using LibraryV3.xUnit.Tests.Api.Tests.TestHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Xunit.Abstractions;
using Newtonsoft.Json;

namespace LibraryV3.xUnit.Tests.Api.Tests
{
    public class XUGetBookTests : IAsyncLifetime, IClassFixture<LibraryHttpService>
    {
        private readonly TestLoggerHelper _logger;
        private LibraryHttpService _libraryService;

        //setup1
        public XUGetBookTests(LibraryHttpService libraryHttpService, ITestOutputHelper output)
        {
            _libraryService = new LibraryHttpService();
            _logger = new TestLoggerHelper(output);
        }

        //setup2
        public async Task InitializeAsync()
        {
            await _libraryService.CreateTestUser();
            await _libraryService.LoginTestUser();
            await _libraryService.CreateLibrary();
        }

        [Fact]
        public async Task GetBookByTitle_IfTitleExist_Return_Ok()
        {
            //Arrage
            var testTitle = "The Book 1";

            //Act
            HttpResponseMessage response = await _libraryService.GetBooksByTitle(testTitle);
            var jsonResult = await response.Content.ReadAsStringAsync();
            var books = JsonConvert.DeserializeObject<List<Book>>(jsonResult);

            //Asserts
            Assert.Multiple(() =>
            {
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.NotNull(jsonResult);
                Assert.NotNull(books);
                Assert.Equal(3, books.Count);
            });

            //Log output to console
            LoggerHelper.Logger.Information($"Response Status Code: {response.StatusCode}");
            LoggerHelper.Logger.Information($"User JSON String: {jsonResult}");


            //Log output to test output
            _logger.LogInformation($"Response Status Code: {response.StatusCode}");
            _logger.LogInformation($"User JSON String: {jsonResult}");
        }

        [Fact]
        public async Task GetBookByTitle_IfTitleNotExist_Return_NotFound()
        {
            //Arrange
            var testTitle = "WrongTitle";

            //Act
            HttpResponseMessage response = await _libraryService.GetBooksByTitle(testTitle);
            var jsonResult = await response.Content.ReadAsStringAsync();

            //Asserts
            Assert.Multiple(()=>
            {
                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
                Assert.NotNull(jsonResult);
            });

            //Log output to console
            LoggerHelper.Logger.Information($"Response Status Code: {response.StatusCode}");
            LoggerHelper.Logger.Information($"User JSON String: {jsonResult}");


            //Log output to test output
            _logger.LogInformation($"Response Status Code: {response.StatusCode}");
            _logger.LogInformation($"User JSON String: {jsonResult}");
        }

        [Fact]
        public async Task GetBookByAuthor_IfAuthorExist_Return_Ok()
        {
            //Arrange
            var testAuthor = "The Author 1";

            //Act
            HttpResponseMessage response = await _libraryService.GetBooksByAuthor(testAuthor);
            var jsonResult = await response.Content.ReadAsStringAsync();
            var books = JsonConvert.DeserializeObject<List<Book>>(jsonResult);

            //Asserts
            Assert.Multiple(()=>
            {
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.NotNull(jsonResult);
                Assert.NotNull(books);
                Assert.Equal(3, books.Count);
            });

            //Log output to console
            LoggerHelper.Logger.Information($"Response Status Code: {response.StatusCode}");
            LoggerHelper.Logger.Information($"User JSON String: {jsonResult}");


            //Log output to test output
            _logger.LogInformation($"Response Status Code: {response.StatusCode}");
            _logger.LogInformation($"User JSON String: {jsonResult}");
        }

        [Fact]
        public async Task GetBookByAuthor_IfAuthorNotExist_Return_NotFound()
        {
            //Arrange
            var testAuthor = "WrongAuthor";

            //Act
            HttpResponseMessage response = await _libraryService.GetBooksByAuthor(testAuthor);
            var jsonResult = await response.Content.ReadAsStringAsync();

            //Asserts
            Assert.Multiple(()=>
            {
                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
                Assert.NotNull(jsonResult);
            });

            //Log output to console
            LoggerHelper.Logger.Information($"Response Status Code: {response.StatusCode}");
            LoggerHelper.Logger.Information($"User JSON String: {jsonResult}");


            //Log output to test output
            _logger.LogInformation($"Response Status Code: {response.StatusCode}");
            _logger.LogInformation($"User JSON String: {jsonResult}");
        }

        [Fact]
        public async Task GetAllBooks_Return_Ok()
        {
            //Arrange

            //Act
            HttpResponseMessage response = await _libraryService.GetAllBooks();
            var jsonResult = await response.Content.ReadAsStringAsync();
            var books = JsonConvert.DeserializeObject<List<Book>>(jsonResult);

            //Asserts
            Assert.Multiple(()=>
            {
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.NotNull(jsonResult);
                Assert.NotNull(books);
                Assert.Equal(9, books.Count);
            });

            //Log output to console
            LoggerHelper.Logger.Information($"Response Status Code: {response.StatusCode}");
            LoggerHelper.Logger.Information($"User JSON String: {jsonResult}");


            //Log output to test output
            _logger.LogInformation($"Response Status Code: {response.StatusCode}");
            _logger.LogInformation($"User JSON String: {jsonResult}");
        }

        //TearDown
        public async Task DisposeAsync()
        {
            //Act
        }
    }
}
