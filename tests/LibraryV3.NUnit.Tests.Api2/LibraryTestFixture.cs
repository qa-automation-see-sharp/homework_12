using LibraryV3.Services;
using Test.Utils;

namespace LibraryV3.NUnit.Tests.Api2;

public class LibraryTestFixture
{
    protected readonly LibraryHttpService _libraryHttpService = new();


    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        await _libraryHttpService.CreateDefaultUser();
    }

    [OneTimeTearDown]
    public void TearDown()
    {
    }
}