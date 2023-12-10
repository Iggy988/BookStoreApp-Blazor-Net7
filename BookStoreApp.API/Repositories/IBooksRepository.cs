using BookStoreApp.API.Data;
using BookStoreApp.API.Models.Book;

namespace BookStoreApp.API.Repositories;

public interface IBooksRepository : IGenerecicRepositoriy<Book>
{
    Task<List<BookReadOnlyDto>> GetAllBooksAsync();
    Task<BookDetailsDto> GetBookAsync(int id);
}
