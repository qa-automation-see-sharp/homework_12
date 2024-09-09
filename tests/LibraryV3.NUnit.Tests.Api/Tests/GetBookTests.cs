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
using Microsoft.AspNetCore.Mvc;

namespace LibraryV3.NUnit.Tests.Api.Tests
{
    public class GetBookTests
    {
        private string? _token;
        private Book _testBook;
        private List<List<Book>> _library;
        Random year = new Random();

        private LibraryHttpService _libraryHttpService;

        [OneTimeSetUp]
        public async Task OneTimeSetUp()
        {
            _libraryHttpService = new LibraryHttpService();
            await _libraryHttpService.CreateTestUser();
            await _libraryHttpService.LoginTestUser();

            _library = new()
            {
                new()
                {   new Book() { Title = "The Book 1", Author = "The Author 1", YearOfRelease = year.Next(1900, 2024) },
                    new Book() { Title = "The Book 2", Author = "The Author 1", YearOfRelease=year.Next(1900, 2024) },
                    new Book() { Title = "The Book 3", Author = "The Author 1", YearOfRelease=year.Next(1900, 2024) }
                },
                new()
                {
                    new Book() { Title = "The Book 1", Author = "The Author 2", YearOfRelease = year.Next(1900, 2024) },
                    new Book() { Title = "The Book 2", Author = "The Author 2", YearOfRelease=year.Next(1900, 2024) },
                    new Book() { Title = "The Book 3", Author = "The Author 2", YearOfRelease=year.Next(1900, 2024) }
                },
                new()
                {
                    new Book() { Title = "The Book 1", Author = "The Author 3", YearOfRelease = year.Next(1900, 2024) },
                    new Book() { Title = "The Book 2", Author = "The Author 3", YearOfRelease=year.Next(1900, 2024) },
                    new Book() { Title = "The Book 3", Author = "The Author 3", YearOfRelease=year.Next(1900, 2024) }
                }
            };
            foreach(var books in _library)
            {
                foreach(var book in books)
                {
                    await _libraryHttpService.CreateBook(book);
                }
            }
        }
        [SetUp]
        public async Task SetUp()
        {
            
        }


        //Get Book by Title If title exist return Ok
        [Test]
        public async Task GetBookByTitle_IfTitleExist_Return_Ok()
        {
            var testTitle = "The Book 1";

            HttpResponseMessage response = await _libraryHttpService.GetBooksByTitle(testTitle);
            var jsonResult = await response.Content.ReadAsStringAsync();
            var books = JsonConvert.DeserializeObject<List<Book>>(jsonResult);

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(jsonResult, Is.Not.Null);
                Assert.That(books.Count, Is.EqualTo(3));
                Assert.That(books[0].Title, Is.EqualTo(testTitle));
            });
        }
        //Get Book by title If title does not exist return Not Found
        [Test]
        public async Task GetBookByTitle_IfTitleNotExist_Return_NotFound()
        {
            var TestBook = "The Book 4";

            HttpResponseMessage response = await _libraryHttpService.GetBooksByTitle(TestBook);
            var jsonResult = await response.Content.ReadAsStringAsync();

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
                Assert.That(jsonResult, Is.Not.Null);
            });
        }
        //Get Book By Author if Author exist return Ok
        [Test]
        public async Task GetBookByAuthor_IfAuthorExist_Return_Ok()
        {
            var testAuthor = "The Author 1";

            HttpResponseMessage response = await _libraryHttpService.GetBooksByAuthor(testAuthor);
            var jsonResult = await response.Content.ReadAsStringAsync();
            var books = JsonConvert.DeserializeObject<List<Book>>(jsonResult);

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(jsonResult, Is.Not.Null);
                Assert.That(books.Count, Is.EqualTo(3));
                Assert.That(books[0].Author, Is.EqualTo(testAuthor));
            });
        }
        //Get Book by Author if Author does not exist return Not Foud
        [Test]
        public async Task GetBookByAuthor_IfAuthorNotExist_Return_NotFound()
        {
            var testAuthor = "The Author 4";

            HttpResponseMessage response = await _libraryHttpService.GetBooksByAuthor(testAuthor);
            var jsonResult = await response.Content.ReadAsStringAsync();

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
                Assert.That(jsonResult, Is.Not.Null);
            });
        }
        //Get All Books return Ok
        
        [Test]
        public async Task GetAllBooks_Return_Ok()
        {
            HttpResponseMessage response = await _libraryHttpService.GetAllBooks();
            var jsonResult = await response.Content.ReadAsStringAsync();
            var books = JsonConvert.DeserializeObject<List<Book>>(jsonResult);


            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(jsonResult, Is.Not.Null);
                Assert.That(books.Count, Is.EqualTo(9));
            });
        }
    }
}
