using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryV3.Contracts.Domain;
using LibraryV3.Services;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Security.Cryptography.X509Certificates;

namespace LibraryV3.NUnit.Tests.Api.Tests
{

    public class LoginUserTests
    {
        private User _testUser;
        private readonly string _time = DateTime.Now.ToString("yyyyMMddHHmmss");
        private string _token = string.Empty;

        private LibraryHttpService _libraryHttpService;

        [OneTimeSetUp]
        public async Task OneTimeSetUp()
        {
            _libraryHttpService = new LibraryHttpService();

            _testUser = new User
            {
                FullName = "Test User",
                NickName = "TestUser",
                Password = "TestPassword"
            };
            HttpResponseMessage response = await _libraryHttpService.CreateUser(_testUser);
            var jsonResult = await response.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<User>(jsonResult);
        }

        //Login with correct credentials return Ok
        [Test]
        public async Task IfCredentialsAreCorrect_Return_Ok()
        {
            var user = new User
            {
                FullName = "Test User",
                NickName = "TestUser",
                Password = "TestPassword"
            };

            HttpResponseMessage response = await _libraryHttpService.LogIn(user);
            var jsonResult = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"jsonResult is: {jsonResult}");
            var newUser = JsonConvert.DeserializeObject<User>(jsonResult);

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(jsonResult, Is.Not.Null);
                Assert.That(newUser.NickName, Is.EqualTo(user.NickName));
            });
        }

        //Login with correct NickName and wrong password return BadRequest
        [Test]
        public async Task IfPasswordIsIncorrect_Return_BadRequest()
        {
            var user = new User
            {
                FullName = "Test User",
                NickName = "TestUser",
                Password = "WrongPassword"
            };

            HttpResponseMessage response = await _libraryHttpService.LogIn(user);
            var jsonResult = await response.Content.ReadAsStringAsync();

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
                Assert.That(jsonResult, Is.Not.Null);
            });
        }

        //Login with wrong NickName and correct password return BadRequest
        [Test]
        public async Task IfNickNameIsIncorrect_Return_BadRequest()
        {
            var user = new User
            {
                FullName = "Test User",
                NickName = "WrongNickName",
                Password = "TestPassword"
            };

            HttpResponseMessage response = await _libraryHttpService.LogIn(user);
            var jsonResult = await response.Content.ReadAsStringAsync();

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
                Assert.That(jsonResult, Is.Not.Null);
            });
        }

        //Login with wrong NickName and wrong password return BadRequest
        [Test]
        public async Task IfNickNameAndPasswordAreIncorrect_Return_BadRequest()
        {
            var user = new User
            {
                FullName = "Test User",
                NickName = "WrongNickName",
                Password = "WrongPassword"
            };

            HttpResponseMessage response = await _libraryHttpService.LogIn(user);
            var jsonResult = await response.Content.ReadAsStringAsync();

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
                Assert.That(jsonResult, Is.Not.Null);
            });
        }
    }
}

