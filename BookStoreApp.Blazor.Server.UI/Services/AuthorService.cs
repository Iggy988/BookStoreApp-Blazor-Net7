using Blazored.LocalStorage;
using BookStoreApp.Blazor.Server.UI.Services.Base;

namespace BookStoreApp.Blazor.Server.UI.Services;

public class AuthorService : BaseHttpService, IAuthorService
{
    private readonly IClient _client;
    public AuthorService(IClient client, ILocalStorageService localStorage) : base(client, localStorage)
    {
        _client = client;
    }

    public async Task<Response<int>> CreateAuthor(AuthorCreateDto author)
    {
        try
        {
            await GetBearerToken();
            await _client.AuthorsPOSTAsync(author);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<Response<List<AuthorReadOnlyDto>>> GetAuthors()
    {
        Response<List<AuthorReadOnlyDto>> response;

        try
        {
            await GetBearerToken();
            var data = await _client.AuthorsAllAsync();
            response = new Response<List<AuthorReadOnlyDto>>
            {
                Data = data.ToList(),
                Success = true,
            };
        }
        catch (ApiException ex)
        {

            response = ConvertApiExceptions<List<AuthorReadOnlyDto>>(ex);
        }

        return response;
    }
}
