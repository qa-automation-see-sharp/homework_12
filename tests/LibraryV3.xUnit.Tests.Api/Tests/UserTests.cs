using LibraryV3.xUnit.Tests.Api.Services;
using LibraryV3.Contracts.Domain;
using Newtonsoft.Json;
using System.Net;

namespace LibraryV3.xUnit.Tests.Api.Tests;

public class UsersTests : IAsyncLifetime, IClassFixture<LibraryHttpService>
{
    private readonly LibraryHttpService _libraryHttpService;      

    public UsersTests(LibraryHttpService libraryHttpService)
    {
        _libraryHttpService = libraryHttpService;
    }

    public async Task InitializeAsync()
    {                    
    }

    [Fact]
    public async Task CreateUser_ShouldReturnCreated()
    {
        //Act
        var httpResponseMessage = await _libraryHttpService.CreateUser(_libraryHttpService.DefaultUser);
        var content = await httpResponseMessage.Content.ReadAsStringAsync();
        var response = JsonConvert.DeserializeObject<User>(content);

        Assert.Multiple(() =>
        {
            Assert.Equal(HttpStatusCode.Created, httpResponseMessage.StatusCode);
            Assert.Equal(_libraryHttpService.DefaultUser.FullName, response.FullName);
            Assert.Equal(_libraryHttpService.DefaultUser.NickName, response.NickName);
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
    public async Task Login_ShouldReturnOK()
    {
        
        //Arrange        
        await _libraryHttpService.CreateUser(_libraryHttpService.DefaultUser);

        //Act
        var httpResponseMessage = await _libraryHttpService.LogIn(_libraryHttpService.DefaultUser);
        var content = await httpResponseMessage.Content.ReadAsStringAsync();
        var response = JsonConvert.DeserializeObject<AuthorizationToken>(content);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.Equal(HttpStatusCode.OK, httpResponseMessage.StatusCode);
            Assert.NotEmpty(response.Token);
            Assert.Equal(_libraryHttpService.DefaultUser.NickName, response.NickName);
        });
    }

    [Fact]
    public async Task Login_UserDoesNotExist_ShouldReturnBadRequest()
    {
        //Act
        var httpResponseMessage = await _libraryHttpService.LogIn(_libraryHttpService.DefaultUser);

        //Assert
        Assert.Equal(HttpStatusCode.BadRequest, httpResponseMessage.StatusCode);
    }

    public async Task DisposeAsync()
    {
    }
}