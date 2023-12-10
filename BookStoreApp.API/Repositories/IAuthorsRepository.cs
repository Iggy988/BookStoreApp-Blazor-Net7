using BookStoreApp.API.Data;
using BookStoreApp.API.Models.Author;

namespace BookStoreApp.API.Repositories;

public interface IAuthorsRepository : IGenerecicRepositoriy<Author>
{
    Task<AuthorDetailsDto> GetAuthorDetailsAsync(int id);
}
