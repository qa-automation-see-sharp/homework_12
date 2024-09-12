using System;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Xunit;
using Xunit.Extensions;
using Xunit.Abstractions;
using LibraryV3.xUnit.Tests.Api.Tests.TestHelpers;
using LibraryV3.Contracts.Domain;
using LibraryV3.Services;
using Newtonsoft.Json;

namespace LibraryV3.xUnit.Tests.Api.Tests
{
    public class XULoginUserTests : IAsyncLifetime, IClassFixture<LibraryHttpService>
    {
        private User _testUser;
        private readonly TestLoggerHelper _logger;

        private LibraryHttpService _libraryService;

        //SetUp #1
        public XULoginUserTests(LibraryHttpService libraryHttpService, ITestOutputHelper output)
        {
            _libraryService = new LibraryHttpService();
            _logger = new TestLoggerHelper(output);
        }

        //setup#2
        public async Task InitializeAsync()
        {
            //Arrange
            await _libraryService.CreateTestUser();
            await _libraryService.LoginTestUser();
        }

        //Login with correct credentials return Ok
        [Fact]
        public async Task IfCredentialsAreCorrect_Return_Ok()
        {
            //Arrange
            _testUser = UserHelpers.LoginUser();

            //Act
            HttpResponseMessage response = await _libraryService.LogIn(_testUser);
            var jsonResult = await response.Content.ReadAsStringAsync();
            var newUser = JsonConvert.DeserializeObject<User>(jsonResult);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.NotNull(jsonResult);
                Assert.Equal(_testUser.NickName, newUser.NickName);
            });

            //Log output to console
            LoggerHelper.Logger.Information($"Response Status Code: {response.StatusCode}");
            LoggerHelper.Logger.Information($"User JSON String: {jsonResult}");


            //Log output to test output
            _logger.LogInformation($"Response Status Code: {response.StatusCode}");
            _logger.LogInformation($"User JSON String: {jsonResult}");
        }

        //Login with correct NickName and wrong password return BadRequest
        [Fact]
        public async Task IfNickNameIsCorrectAndPasswordIsWrong_Return_BadRequest()
        {
            //Arrange
            _testUser = UserHelpers.LoginUser();
            _testUser.Password = "WrongPassword";

            //Act
            HttpResponseMessage response = await _libraryService.LogIn(_testUser);
            var jsonResult = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"jsonResult is: {jsonResult}");

            //Asserts
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

        //Login with wrong NickName and correct password return BadRequest
        [Fact]
        public async Task IfNickNameIsWrongAndPasswordIsCorrect_Return_BadRequest()
        {
            //Arrange
            _testUser = UserHelpers.LoginUser();
            _testUser.NickName = "WrongNickName";

            //Act
            HttpResponseMessage response = await _libraryService.LogIn(_testUser);
            var jsonResult = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"jsonResult is: {jsonResult}");

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

        //Login with wrong NickName and wrong password return BadRequest
        [Fact]  
        public async Task IfNickNameIsWrongAndPasswordIsWrong_Return_BadRequest()
        {
            //Arrage
            _testUser = UserHelpers.LoginUser();
            _testUser.NickName = "WrongNickName";
            _testUser.Password = "WrongPassword";

            //Act
            HttpResponseMessage response = await _libraryService.LogIn(_testUser);
            var jsonResult = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"jsonResult is: {jsonResult}");

            //Asserts
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

        //TearDown
        public async Task DisposeAsync()
        {
            //Act
        }
    }
}
