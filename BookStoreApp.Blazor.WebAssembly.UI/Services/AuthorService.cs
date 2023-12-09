using AutoMapper;
using Blazored.LocalStorage;
using BookStoreApp.Blazor.WebAssembly.UI.Services.Base;

namespace BookStoreApp.Blazor.WebAssembly.UI.Services;

public class AuthorService : BaseHttpService, IAuthorService
{
    private readonly IClient _client;
    private readonly ILocalStorageService _localStorage;
    private readonly IMapper _mapper;

    public AuthorService(IClient client, ILocalStorageService localStorage, IMapper mapper) : base(client, localStorage)
    {
        _client = client;
        _localStorage = localStorage;
        _mapper = mapper;
    }

    public async Task<Response<int>> Create(AuthorCreateDto author)
    {
        Response<int> response = new ();

        try
        {
            await GetBearerToken();
            await _client.AuthorsPOSTAsync(author);
        }
        catch (ApiException ex)
        {
            response = ConvertApiExceptions<int>(ex);
        }
        return response;
    }

    public async Task<Response<int>> Delete(int id)
    {
        Response<int> response = new();

        try
        {
            await GetBearerToken();
            await _client.AuthorsDELETEAsync(id);
        }
        catch (ApiException ex)
        {
            response = ConvertApiExceptions<int>(ex);
        }
        return response;
    }

    public async Task<Response<int>> Edit(int id, AuthorUpdateDto author)
    {
        Response<int> response = new();

        try
        {
            await GetBearerToken();
            await _client.AuthorsPUTAsync(id, author);
        }
        catch (ApiException ex)
        {
            response = ConvertApiExceptions<int>(ex);
        }
        return response;
    }

    public async Task<Response<AuthorDetailsDto>> Get(int id)
    {
        Response<AuthorDetailsDto> response;

        try
        {
            await GetBearerToken();
            var data = await _client.AuthorsGETAsync(id);
            response = new Response<AuthorDetailsDto>
            {
                Data = data,
                Success = true,
            };
        }
        catch (ApiException ex)
        {

            response = ConvertApiExceptions<AuthorDetailsDto>(ex);
        }

        return response;
    }

    public async Task<Response<List<AuthorReadOnlyDto>>> Get()
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

    public async Task<Response<AuthorUpdateDto>> GetForUpdate(int id)
    {
        Response<AuthorUpdateDto> response;

        try
        {
            await GetBearerToken();
            var data = await _client.AuthorsGETAsync(id);
            response = new Response<AuthorUpdateDto>
            {
                Data = _mapper.Map<AuthorUpdateDto>(data),
                Success = true,
            };
        }
        catch (ApiException ex)
        {

            response = ConvertApiExceptions<AuthorUpdateDto>(ex);
        }

        return response;
    }
}
