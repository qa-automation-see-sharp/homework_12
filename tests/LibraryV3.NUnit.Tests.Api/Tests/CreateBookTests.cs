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
    public class CreateBookTests
    {
        private Book _testBook;
        private string? _key;
        private string? _token;
        private int? _randomYear;

        private LibraryHttpService _libraryHttpService;


        [OneTimeSetUp]
        public async Task OneTimeSetUp()
        {
            _libraryHttpService = new LibraryHttpService();
            await _libraryHttpService.CreateTestUser();
            await _libraryHttpService.LoginTestUser();

            _testBook = new Book
            {
                Title = "Test Book",
                Author = "Test Author",
                YearOfRelease = 2021
            };
            HttpResponseMessage response = await _libraryHttpService.CreateBook(_testBook);
            var jsonResult = await response.Content.ReadAsStringAsync();
            var book = JsonConvert.DeserializeObject<Book>(jsonResult);
        }
        [SetUp]
        public async Task SetUp()
        {
            var uniqueKey = Guid.NewGuid().ToString();
            var randomKey = new Random();
            _key = $"{uniqueKey}{randomKey.Next()}";

            _randomYear = new Random().Next(1900, 2024);
        }

        //Create a book with no Authorisaton token
        [Test]
        public async Task CreateBook_IfNoAuthorizationToken_Return_Unauthorized()
        {
            var random = new Random();
            var book = new Book
            {
                Title = "Test Book" + _key,
                Author = "Test Author" + _key,
                YearOfRelease = _randomYear.Value
            };

            var response = await _libraryHttpService.CreateBook(_token, book);
            var jsonResult = await response.Content.ReadAsStringAsync();

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
                Assert.That(jsonResult, Is.Not.Null);
            });
        }

        //Create a book with new title and author
        [Test]
        public async Task CreateBook_IfNewTitleAndAuthor_Return_Created()
        {
            var book = new Book
            {
                Title = "Test Book" + _key,
                Author = "Test Author" + _key,
                YearOfRelease = _randomYear.Value
            };

            var response = await _libraryHttpService.CreateBook(book);
            var jsonResult = await response.Content.ReadAsStringAsync();
            var newBook = JsonConvert.DeserializeObject<Book>(jsonResult);

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
                Assert.That(jsonResult, Is.Not.Null);
                Assert.That(newBook.Title, Is.EqualTo(book.Title));
                Assert.That(newBook.Author, Is.EqualTo(book.Author));
                Assert.That(newBook.YearOfRelease, Is.EqualTo(book.YearOfRelease));
            });
        }

        //Create a new book with the same title but new author
        [Test]
        public async Task CreateBook_IfSameTitleNewAuthor_Return_Created()
        {
            var book = new Book
            {
                Title = "Test Book",
                Author = "Test Author" + _key,
                YearOfRelease = _randomYear.Value
            };

            var response = await _libraryHttpService.CreateBook(book);
            var jsonResult = await response.Content.ReadAsStringAsync();
            var newBook = JsonConvert.DeserializeObject<Book>(jsonResult);

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
                Assert.That(jsonResult, Is.Not.Null);
                Assert.That(newBook.Title, Is.EqualTo(book.Title));
                Assert.That(newBook.Author, Is.EqualTo(book.Author));
                Assert.That(newBook.YearOfRelease, Is.EqualTo(book.YearOfRelease));
            });
        }


        //Create a book with new title but the same author
        [Test]
        public async Task CreateBook_IfNewTitleSameAuthor_Return_Created()
        {
            var book = new Book
            {
                Title = "Test Book" + _key,
                Author = "Test Author",
                YearOfRelease = _randomYear.Value
            };

            var response = await _libraryHttpService.CreateBook(book);
            var jsonResult = await response.Content.ReadAsStringAsync();
            var newBook = JsonConvert.DeserializeObject<Book>(jsonResult);

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
                Assert.That(jsonResult, Is.Not.Null);
                Assert.That(newBook.Title, Is.EqualTo(book.Title));
                Assert.That(newBook.Author, Is.EqualTo(book.Author));
                Assert.That(newBook.YearOfRelease, Is.EqualTo(book.YearOfRelease));
            });
        }

        //Create a book with the same title and author
        [Test]
        public async Task CreateBook_IfSameTitleAndAuthor_Return_BadRequest()
        {
            var book = new Book
            {
                Title = "Test Book",
                Author = "Test Author",
                YearOfRelease = 2021
            };

            var response = await _libraryHttpService.CreateBook(book);
            var jsonResult = await response.Content.ReadAsStringAsync();

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
                Assert.That(jsonResult, Is.Not.Null);
            });
        }
    }
}
