using AutoMapper;
using Blazored.LocalStorage;
using BookStoreApp.Blazor.Server.UI.Services.Base;

namespace BookStoreApp.Blazor.Server.UI.Services;

public class BookService : BaseHttpService, IBookService
{
    private readonly IClient _client;
    private readonly ILocalStorageService _localStorage;
    private readonly IMapper _mapper;

    public BookService(IClient client, ILocalStorageService localStorage, IMapper mapper) : base(client, localStorage)
    {
        _client = client;
        _localStorage = localStorage;
        _mapper = mapper;
    }

    public async Task<Response<int>> Create(BookCreateDto author)
    {
        Response<int> response = new();

        try
        {
            await GetBearerToken();
            await _client.BooksPOSTAsync(author);
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
            await _client.BooksDELETEAsync(id);
        }
        catch (ApiException ex)
        {
            response = ConvertApiExceptions<int>(ex);
        }
        return response;
    }

    public async Task<Response<int>> Edit(int id, BookUpdateDto author)
    {
        Response<int> response = new();

        try
        {
            await GetBearerToken();
            await _client.BooksPUTAsync(id, author);
        }
        catch (ApiException ex)
        {
            response = ConvertApiExceptions<int>(ex);
        }
        return response;
    }

    public async Task<Response<BookDetailsDto>> Get(int id)
    {
        Response<BookDetailsDto> response;

        try
        {
            await GetBearerToken();
            var data = await _client.BooksGETAsync(id);
            response = new Response<BookDetailsDto>
            {
                Data = data,
                Success = true,
            };
        }
        catch (ApiException ex)
        {

            response = ConvertApiExceptions<BookDetailsDto>(ex);
        }

        return response;
    }

    public async Task<Response<List<BookReadOnlyDto>>> Get()
    {
        Response<List<BookReadOnlyDto>> response;

        try
        {
            await GetBearerToken();
            var data = await _client.BooksAllAsync();
            response = new Response<List<BookReadOnlyDto>>
            {
                Data = data.ToList(),
                Success = true,
            };
        }
        catch (ApiException ex)
        {

            response = ConvertApiExceptions<List<BookReadOnlyDto>>(ex);
        }

        return response;
    }

    public async Task<Response<BookUpdateDto>> GetForUpdate(int id)
    {
        Response<BookUpdateDto> response;

        try
        {
            await GetBearerToken();
            var data = await _client.BooksGETAsync(id);
            response = new Response<BookUpdateDto>
            {
                Data = _mapper.Map<BookUpdateDto>(data),
                Success = true,
            };
        }
        catch (ApiException ex)
        {

            response = ConvertApiExceptions<BookUpdateDto>(ex);
        }

        return response;
    }
}
