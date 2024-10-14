using System.Net;
using LibraryV3.NUnit.Tests.Api2;
using static Test.Utils.TestHelpers.DataHelper;

namespace LibraryV3.xUnit.Tests.Api;

public class UserTests: LibraryTestFixture
{
    [Fact]
    public async Task RegisterUserAsync_ReturnCreated()
    {
        // Arrange
        var response = await _libraryHttpService.CreateUser(CreateUser());

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task LogInAsync_ReturnOK()
    {
        // Arrange
        var response = await _libraryHttpService.LogIn(_libraryHttpService.DefaultUser, false);
        var token = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.False(string.IsNullOrEmpty(token));
    }
}