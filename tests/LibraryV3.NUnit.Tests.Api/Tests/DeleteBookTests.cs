using LibraryV3.Contracts.Domain;
using LibraryV3.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LibraryV3.NUnit.Tests.Api.Tests
{
    public class DeleteBookTests
    {
        private string _toke;
        private Book _testBook;
        private List<List<Book>> _library;
        Random year = new Random();

        private LibraryHttpService _libraryHttpService = new();

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
            foreach (var books in _library)
            {
                foreach (var book in books)
                {
                    await _libraryHttpService.CreateBook(book);
                }
            }
        }
        [SetUp]
        public async Task SetUp()
        {

        }

        //Delete book if book exist in library return Ok
        [Test]
        public async Task DeleteBook_IfExistInLibrary_return_Ok()
        {
            var book = _library.SelectMany(List => List).FirstOrDefault();

            HttpResponseMessage response = await _libraryHttpService.DeleteBook(book.Title, book.Author);
            var json = await response.Content.ReadAsStringAsync();

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(json, Is.Not.Null);

            });

        }
        //Delete book if book does not exist return not found
        [Test]
        public async Task DeleteBook_IfNotExistInLibrary_return_NotFound()
        {
            var Book = new Book
            {
                Title = "Noname",
                Author = "Anonymus",
                YearOfRelease = year.Next(1900, 2024)
            };

            HttpResponseMessage response = await _libraryHttpService.DeleteBook(Book.Title, Book.Author);
            var json = await response.Content.ReadAsStringAsync();

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
                Assert.That(json, Is.Not.Null);

            });

        }
        //delete all book by title if exist return ok

        //delete all book by title if does not exist return not found

        //delete all book by author if exist return ok

        //delete all book by author if does not exist retur not found

        //delete all book
    }
}

