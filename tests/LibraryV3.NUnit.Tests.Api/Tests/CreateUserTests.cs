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

    public class CreateUserTests
    {
        private User _testUser;
        private string? _key;

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
        [SetUp]
        public async Task SetUp()
        {
            var uniqueKey = Guid.NewGuid().ToString();
            var randomKey = new Random();
            _key = $"{uniqueKey}{randomKey.Next()}";
        }

        [Test]
        public async Task CreateUser_IfUserDoesnotExist_Return_Created()
        {
            var user = new User
            {
                FullName = "Test User",
                NickName = "TestUser" + _key,
                Password = "TestPassword"
            };

            HttpResponseMessage response = await _libraryHttpService.CreateUser(user);
            var jsonResult = await response.Content.ReadAsStringAsync();
            var newUser = JsonConvert.DeserializeObject<User>(jsonResult);

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
                Assert.That(jsonResult, Is.Not.Null);
                Assert.That(newUser.FullName, Is.EqualTo(user.FullName));
                Assert.That(newUser.NickName, Is.EqualTo(user.NickName));
            });
        }

        [Test]
        public async Task CreateUser_IfUserExist_Return_BadRequest()
        {
            var user = new User
            {
                FullName = "Test User",
                NickName = "TestUser",
                Password = "TestPassword"
            };

            HttpResponseMessage response = await _libraryHttpService.CreateUser(user);
            var jsonResult = await response.Content.ReadAsStringAsync();

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
                Assert.That(jsonResult, Is.Not.Null);
            });
        }

        //Create a new user with blanc nickname return BadRequest
        [Test]
        public async Task CreateUser_IfNickNameIsBlank_Return_BadRequest()
        {
            var user = new User
            {
                FullName = "Test User",
                NickName = "", //Blank NickName
                Password = "TestPassword"
            };

            HttpResponseMessage response = await _libraryHttpService.CreateUser(user);
            var jsonResult = await response.Content.ReadAsStringAsync();

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
                Assert.That(jsonResult, Is.Not.Null);
            });
        }

        //Create a new user with spaced nickname return BadRequest
        [Test]
        public async Task CreateUser_IfNickNameIsSpaced_Return_BadRequest()
        {
            var user = new User
            {
                FullName = "Test User",
                NickName = "   ", //Spaced NickName
                Password = "TestPassword"
            };

            HttpResponseMessage response = await _libraryHttpService.CreateUser(user);
            var jsonResult = await response.Content.ReadAsStringAsync();

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
                Assert.That(jsonResult, Is.Not.Null);
            });
        }

        //Create a new user with blanc password return BadRequest
        [Test]
        public async Task CreateUser_IfPasswordIsBlank_Return_BadRequest()
        {
            var user = new User
            {
                FullName = "Test User",
                NickName = "TestUser" + _key,
                Password = "" //Blank Password
            };

            HttpResponseMessage response = await _libraryHttpService.CreateUser(user);
            var jsonResult = await response.Content.ReadAsStringAsync();

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
                Assert.That(jsonResult, Is.Not.Null);
            });
        }

        //Create a new user with blanc fullname return Created
        [Test]
        public async Task CreateUser_IfFullNameIsBlank_Return_Created()
        {
            var user = new User
            {
                FullName = "",
                NickName = "TestUser" + _key,
                Password = "TestPassword"
            };

            HttpResponseMessage response = await _libraryHttpService.CreateUser(user);
            var jsonResult = await response.Content.ReadAsStringAsync();
            var newUser = JsonConvert.DeserializeObject<User>(jsonResult);

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
                Assert.That(jsonResult, Is.Not.Null);
                Assert.That(newUser.FullName, Is.EqualTo(user.FullName));
                Assert.That(newUser.NickName, Is.EqualTo(user.NickName));
            });
        }

    }
}

