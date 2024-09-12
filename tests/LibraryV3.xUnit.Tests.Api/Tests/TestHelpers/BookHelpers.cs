using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryV3.Contracts.Domain;

namespace LibraryV3.xUnit.Tests.Api.Tests.TestHelpers
{
    public static class BookHelpers
    {
        public static Book CreateBook()
        {
            return new Book
            {
                Title = "Test Book" + Guid.NewGuid().ToString(),
                Author = "Test Author" + Guid.NewGuid().ToString(),
                YearOfRelease = new Random().Next(1900, 2022)
            };
        }

        public static Book CreateBook(string title, string author)
        {
            return new Book
            {
                Title = title,
                Author = author,
                YearOfRelease = new Random().Next(1900, 2022)
            };
        }

        public static Book CreateExistBook()
        {
            return new Book
            {
                Title = "Test Book",
                Author = "Test Author",
                YearOfRelease = 2021
            };
        }


        public static List<List<Book>> CreateLibrary()
        {
            return new List<List<Book>>
            {
                new()
                {   new Book() { Title = "The Book 1", Author = "The Author 1", YearOfRelease = new Random().Next(1900, 2024) },
                    new Book() { Title = "The Book 2", Author = "The Author 1", YearOfRelease=new Random().Next(1900, 2024) },
                    new Book() { Title = "The Book 3", Author = "The Author 1", YearOfRelease=new Random().Next(1900, 2024) }
                },
                new()
                {
                    new Book() { Title = "The Book 1", Author = "The Author 2", YearOfRelease = new Random().Next(1900, 2024) },
                    new Book() { Title = "The Book 2", Author = "The Author 2", YearOfRelease=new Random().Next(1900, 2024) },
                    new Book() { Title = "The Book 3", Author = "The Author 2", YearOfRelease=new Random().Next(1900, 2024) }
                },
                new()
                {
                    new Book() { Title = "The Book 1", Author = "The Author 3", YearOfRelease = new Random().Next(1900, 2024) },
                    new Book() { Title = "The Book 2", Author = "The Author 3", YearOfRelease=new Random().Next(1900, 2024) },
                    new Book() { Title = "The Book 3", Author = "The Author 3", YearOfRelease=new Random().Next(1900, 2024) }
                }
            };
        }
    }
}
