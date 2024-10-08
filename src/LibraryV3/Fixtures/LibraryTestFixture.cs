using LibraryV3.Services;
using NUnit.Framework;

namespace LibraryV3.Fixtures;

public class LibraryTestFixture
{
    protected readonly LibraryHttpService _libraryHttpService = new();


    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        _libraryHttpService.Configure("http://localhost:5111/");

        await _libraryHttpService.CreateDefaultUser();
    }

    [OneTimeTearDown]
    public void TearDown()
    {
    }
}