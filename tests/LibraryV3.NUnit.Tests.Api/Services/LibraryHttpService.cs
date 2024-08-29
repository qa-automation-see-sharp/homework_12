using System.Text;
using LibraryV3.Contracts.Domain;
using LibraryV3.NUnit.Tests.Api.TestHelpers;
using Newtonsoft.Json;

namespace LibraryV3.NUnit.Tests.Api.Services;

public class LibraryHttpService
{
    private readonly HttpClient _httpClient;
    private AuthorizationToken Token { get; set; }
    private User? DefaultUser { get; set; }

    public LibraryHttpService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<User> CreateDefaultUser()
    {
        DefaultUser = DataHelper.UserHelper.CreateRandomUser();

        var url = EndpointsForTest.Users.Register;
        var json = JsonConvert.SerializeObject(DefaultUser);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(url, content);

        ConsoleHelper.Info(HttpMethod.Post, "Create Default User", url, json, response);

        return DefaultUser;
    }

    public async Task<LibraryHttpService> Authorize()
    {
        var url = EndpointsForTest.Users.Login(DefaultUser.NickName, DefaultUser.Password);
        var response = await _httpClient.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();
        Token = JsonConvert.DeserializeObject<AuthorizationToken>(content);

        ConsoleHelper.Info(HttpMethod.Get, "Authorize", url, content, response);

        return this;
    }


    public async Task<HttpResponseMessage> CreateUser(User user)
    {
        var url = EndpointsForTest.Users.Register;
        var json = JsonConvert.SerializeObject(user);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(url, content);
        var jsonString = await response.Content.ReadAsStringAsync();

        ConsoleHelper.Info(HttpMethod.Post, "Create user", url, jsonString, response);

        return response;
    }

    public async Task<HttpResponseMessage> LogIn(User user)
    {
        var url = EndpointsForTest.Users.Login(user.NickName, user.Password);
        var response = await _httpClient.GetAsync(url);
        var jsonString = await response.Content.ReadAsStringAsync();

        ConsoleHelper.Info(HttpMethod.Get, "Login", url, jsonString, response);

        return response;
    }

    public async Task<HttpResponseMessage> LogIn(string nickName, string password)
    {
        var url = EndpointsForTest.Users.Login(nickName, password);
        var response = await _httpClient.GetAsync(url);
        var jsonString = await response.Content.ReadAsStringAsync();

        ConsoleHelper.Info(HttpMethod.Get, "Login", url, jsonString, response);

        return response;
    }


    public async Task<HttpResponseMessage> PostBook(Book book)
    {
        var url = EndpointsForTest.Books.Create(Token.Token);
        var json = JsonConvert.SerializeObject(book);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(url, content);
        var jsonString = await response.Content.ReadAsStringAsync();

        ConsoleHelper.Info(HttpMethod.Post, "Create book", url, jsonString, response);

        return response;
    }

    public async Task<HttpResponseMessage> GetBooksByTitle(string title)
    {
        var url = EndpointsForTest.Books.GetBooksByTitle(title);
        var response = await _httpClient.GetAsync(url);
        var jsonString = await response.Content.ReadAsStringAsync();

        ConsoleHelper.Info(HttpMethod.Get, "Get book by title", url, jsonString, response);

        return response;
    }

    public async Task<HttpResponseMessage> GetBooksByAuthor(string author)
    {
        var url = EndpointsForTest.Books.GetBooksByAuthor(author);
        var response = await _httpClient.GetAsync(url);
        var jsonString = await response.Content.ReadAsStringAsync();

        ConsoleHelper.Info(HttpMethod.Get, "Get book by author", url, jsonString, response);

        return response;
    }

    public async Task<HttpResponseMessage> DeleteBook(string title, string author)
    {
        var url = EndpointsForTest.Books.Delete(title, author, Token.Token);
        var response = await _httpClient.DeleteAsync(url);
        var jsonString = await response.Content.ReadAsStringAsync();

        ConsoleHelper.Info(HttpMethod.Delete, "Delete book", url, jsonString, response);

        return response;
    }

    public async Task<HttpResponseMessage> DeleteBook(string title, string author, string token)
    {
        var url = EndpointsForTest.Books.Delete(title, author, token);
        var response = await _httpClient.DeleteAsync(url);
        var jsonString = await response.Content.ReadAsStringAsync();

        ConsoleHelper.Info(HttpMethod.Delete, "Delete book", url, jsonString, response);

        return response;
    }

    //TODO:debug
    public async Task DeleteListBooks(List<Book> booksList, string author)
    {
        var count = booksList.Select(x=>x.Author.Equals(author)).ToList().Count;
        string url;
        string jsonString;
        HttpResponseMessage? responseMessage;

        if (count == 1)
        {
            url = EndpointsForTest.Books.Delete(booksList[0].Title, booksList[0].Author, Token.Token);
            responseMessage = await _httpClient.DeleteAsync(url);
            responseMessage.EnsureSuccessStatusCode();
            jsonString = await responseMessage.Content.ReadAsStringAsync();
            ConsoleHelper.Info(HttpMethod.Delete, "Delete book", url, jsonString, responseMessage);
        }
        if (count > 1)
        {
            foreach (var book in booksList)
            {
                url = EndpointsForTest.Books.Delete(book.Title, book.Author, Token.Token);
                responseMessage = await _httpClient.DeleteAsync(url);
                responseMessage.EnsureSuccessStatusCode();
                jsonString = await responseMessage.Content.ReadAsStringAsync();
                ConsoleHelper.Info(HttpMethod.Delete, "Delete book", url, jsonString, responseMessage);
                booksList.Remove(book);
            }
        }
    }
}