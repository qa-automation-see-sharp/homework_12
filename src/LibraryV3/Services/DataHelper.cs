using Bogus;
using LibraryV3.Contracts.Domain;

namespace LibraryV3.NUnit.Tests.Api
{
    public static class DataHelper
    {
        public static Book CreateBook()
        {
            var faker = new Faker();

            return new Book
            {
                Title = $"Pragmatic Programmer{faker.Random.AlphaNumeric(4)}",
                Author = "Andrew Hunt",
                YearOfRelease = 1999
            };
        }
    }
}
