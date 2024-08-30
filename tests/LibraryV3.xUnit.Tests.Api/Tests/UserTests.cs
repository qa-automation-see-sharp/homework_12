using System.Net;
using LibraryV3.Contracts.Domain;
using LibraryV3.xUnit.Tests.Api.Fixtures;
using LibraryV3.xUnit.Tests.Api.Services;
using LibraryV3.xUnit.Tests.Api.TestHelpers;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;

namespace LibraryV3.xUnit.Tests.Api.Tests;

public class UsersTests : IAsyncLifetime, IClassFixture<WebApplicationFactory<LibraryV3TestFixture>>
{
    private User User { get; set; }
    private LibraryHttpService HttpService;
    private readonly WebApplicationFactory<IApiMarker> _factory = new();
   
    public new async Task InitializeAsync()
    {
        var client =_factory.CreateClient();
        HttpService = new LibraryHttpService(client);
        User = await HttpService.CreateDefaultUser();
    }

    [Fact]
    public async Task CreateUserSusses()
    {
        User user = DataHelper.UserHelper.CreateRandomUser();

        var response = await HttpService.CreateUser(user);
        var json = await response.Content.ReadAsStringAsync();
        var u = JsonConvert.DeserializeObject<User>(json);

        Assert.Multiple(() =>
        {
            Assert.Equal(response.StatusCode, HttpStatusCode.Created);
            Assert.NotNull(response);
            Assert.Equal(u.FullName, user.FullName);
            Assert.Equal(u.NickName, user.NickName);
        });
    }

    [Fact]
    public async Task CreateExistesUserBadRequest(){
        User user = DataHelper.UserHelper.CreateRandomUser();

        await HttpService.CreateUser(user);
        var response = await HttpService.CreateUser(user);
        var jsonString = await response.Content.ReadAsStringAsync();
        var s = jsonString.Trim('"'); 

        Assert.Multiple(() =>
        {
            Assert.True(s.Equals(DataHelper.ErrorMessage.ExistUser(user.NickName)));
            Assert.Equal(response.StatusCode, HttpStatusCode.BadRequest);
        });
    }

    [Fact]
    public async Task LoginUser()
    {
        var message = await HttpService.LogIn(User);
        var json = await message.Content.ReadAsStringAsync();
        var obj = JsonConvert.DeserializeObject<AuthorizationToken>(json);

        Assert.Multiple(() =>
        {
            Assert.Equal(message.StatusCode, HttpStatusCode.OK);
            Assert.NotNull(obj);
            Assert.NotEmpty(obj.Token);
            Assert.Equal(obj.NickName, User.NickName);
        });
    }

    [Fact]
    public async Task LoginBadRequest()
    {
        var message = await HttpService.LogIn("", "");
        var json = await message.Content.ReadAsStringAsync();
        var s = json.Trim('"');

        Assert.Multiple(() =>
        {
            Assert.Equal(message.StatusCode, HttpStatusCode.BadRequest);
            Assert.True(s.Equals(DataHelper.ErrorMessage.InvalidLogin));
        });
    }

    public async Task DisposeAsync()
    {
    }
}