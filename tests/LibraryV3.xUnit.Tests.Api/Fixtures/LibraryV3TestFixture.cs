using LibraryV3.xUnit.Tests.Api.Services;
using Microsoft.AspNetCore.Mvc.Testing;

namespace LibraryV3.xUnit.Tests.Api.Fixtures;

public class LibraryV3TestFixture : IAsyncLifetime, IClassFixture<WebApplicationFactory<IApiMarker>>
{
    protected LibraryHttpService HttpService;
    private readonly WebApplicationFactory<IApiMarker> _factory;

    public LibraryV3TestFixture(WebApplicationFactory<IApiMarker> factory){
            _factory = factory;
    }
    public async Task DisposeAsync()
    {
    }

    public async Task InitializeAsync()
    {
        var client = _factory.CreateClient();
        HttpService = new LibraryHttpService(client);
        await HttpService.CreateDefaultUser();
        await HttpService.Authorize();
    }
}