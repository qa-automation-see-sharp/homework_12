using LibraryV3.Contracts.Domain;
using LibraryV3.xUnit.Tests.Api.TestHelpers;
using LibraryV3.xUnit.Tests.Api.Services;
using LibraryV3.Services;
using Newtonsoft.Json;
using System.Net;

namespace LibraryV3.NUnit.Tests.Api.Tests;

public class UsersTests : IClassFixture<LibraryHttpService>
{
    private readonly LibraryHttpService _libraryHttpService;
    public UsersTests(LibraryHttpService libraryHttpService) 
    {
        _libraryHttpService = libraryHttpService; 
    }

    [Fact]
    public async Task CreateUser_ShouldReturnCreated()
    {

        //Arrange
        var user = DataHelper.CreateUser();

        //Act
        var httpResponseMessage = await _libraryHttpService.CreateUser(user);
        var content = await httpResponseMessage.Content.ReadAsStringAsync();
        var response = JsonConvert.DeserializeObject<User>(content);

        Assert.Multiple(() =>
        {
            Assert.Equal(HttpStatusCode.Created, httpResponseMessage.StatusCode);
            Assert.Equal(user.FullName, response.FullName);
            Assert.Equal(user.NickName, response.NickName);
        });
    }

    [Fact]
    public async Task CreateUser_AlreadyExists_ShouldReturnBadRequest()
    {
        //Arrange
        await _libraryHttpService.CreateUser(_libraryHttpService.DefaultUser);

        //Act
        var httpResponseMessage = await _libraryHttpService.CreateUser(_libraryHttpService.DefaultUser);

        //Assert
        Assert.Equal(HttpStatusCode.BadRequest, httpResponseMessage.StatusCode);
    }


    [Fact]
    public async Task LogIn_WhenUserExists_ReturnOk()
    {
        await _libraryHttpService.CreateUser(_libraryHttpService.DefaultUser);

        HttpResponseMessage response = await _libraryHttpService.LogIn(_libraryHttpService.DefaultUser);

        var jsonString = await response.Content.ReadAsStringAsync();

        Assert.Multiple(() =>
        {
            Assert.Equal(response.StatusCode, HttpStatusCode.OK);
            Assert.NotNull(jsonString);
        });
    }
}