
namespace LibraryV3.xUnit.Tests.Api.TestHelpers
{
    public static class ConsoleHelper
    {
        public static void Info(HttpMethod methodHTTP, string name, string url, string jsonString, HttpResponseMessage response){
            Console.WriteLine($"{name}:");
            Console.WriteLine($"{methodHTTP} request to: {url}");
            Console.WriteLine($"Content: {jsonString}");
            Console.WriteLine($"Response status code is : {response.StatusCode}");
        }
    }
}