using Xunit;
using LibraryV3.Contracts;
using Microsoft.AspNetCore.Mvc.Testing;

namespace LibraryV3.xUnit.Tests.Api.EndPointsTests.Book
{
    public class CreateBook
    {
        [Fact]
        public async Task Post_Book_ShouldReturnCreated()
        {
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("http://localhost:5292");
            var book = new Contracts.Domain.Book
            {
                Title = Guid.NewGuid().ToString(),
                Author = Guid.NewGuid().ToString(),
                YearOfRelease = new Random().Next(1800, 2024)
            };

            //var response = await httpClient.PostAsync();

        }
    }
}