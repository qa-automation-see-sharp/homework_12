using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace LibraryV3.xUnit.Tests.Api.EndPointsTests.Book
{
    [Collection("LabraryV3 Collection")]
    public class GetBook : IAsyncLifetime, IClassFixture<WebApplicationFactory<IApiMarker>>
    {
        private readonly WebApplicationFactory<IApiMarker> _factory;

         public async Task InitializeAsync()
        {
            
        }

        public GetBook(WebApplicationFactory<IApiMarker> factory){
            _factory = factory;
        }

        
        
        [Fact]
        public async Task Get_ExistBookByTitle_ReturnOK()
        {
            var httpClient = _factory.CreateClient();
            var book = new Contracts.Domain.Book{
                Title = Guid.NewGuid().ToString(),
                Author= Guid.NewGuid().ToString(),
                YearOfRelease = new Random().Next(1800, 2024)
            };
            var title = "Book1";
            var response = await httpClient.GetAsync($"api/books/by-title/{title}");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Get_ExistBookByAuthor_ReturnOK()
        {
            var httpClient = _factory.CreateClient();
            var book = new Contracts.Domain.Book{
                Title = Guid.NewGuid().ToString(),
                Author= Guid.NewGuid().ToString(),
                YearOfRelease = new Random().Next(1800, 2024)
            };
            var author = "Book1";
            var response = await httpClient.GetAsync($"api/books/by-author/{author}");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

       

        public async Task DisposeAsync()
        {
        }
    }
}