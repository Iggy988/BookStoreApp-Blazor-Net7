using BookStoreApp.API.Data;

namespace BookStoreApp.API.Repositories;

public class BooksRepository : GenericRepository<Book>, IBooksRepository
{
    public BooksRepository(BookStoreDbContext context) : base(context)
    {
    }
}
