using LibraryV2.Tests.Api.Services;

namespace LibraryV2.Tests.Api.Fixtures;

[TestFixture]
public class LibraryV2TestFixture : GlobalSetUpFixture
{
    protected LibraryHttpService LibraryHttpService;

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        LibraryHttpService = new LibraryHttpService();
        LibraryHttpService.Configure("http://localhost:5111/");
        await LibraryHttpService.CreateDefaultUser();
        await LibraryHttpService.Authorize();
    }

    [OneTimeTearDown]
    public void TearDown()
    {
    }
}