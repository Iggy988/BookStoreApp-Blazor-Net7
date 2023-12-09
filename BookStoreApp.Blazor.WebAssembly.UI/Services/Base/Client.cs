namespace BookStoreApp.Blazor.WebAssembly.UI.Services.Base;

//Client from ServiceClient
public partial class Client : IClient
{
    public HttpClient HttpClient
    {
        get
        {
            return _httpClient;
        }
    }
}
