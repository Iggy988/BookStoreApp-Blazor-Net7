using AutoMapper;
using AutoMapper.QueryableExtensions;
using BookStoreApp.API.Data;
using BookStoreApp.API.Models.Book;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApp.API.Repositories;

public class BooksRepository : GenericRepository<Book>, IBooksRepository
{
    private readonly BookStoreDbContext _context;
    private readonly IMapper _mapper;

    public BooksRepository(BookStoreDbContext context, IMapper mapper) : base(context, mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<BookReadOnlyDto>> GetAllBooksAsync()
    {
        return await _context.Books
            .Include(q => q.Author)
            .ProjectTo<BookReadOnlyDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<BookDetailsDto> GetBookAsync(int id)
    {
        return await _context.Books
            .Include(q => q.Author)
            .ProjectTo<BookDetailsDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(q => q.Id == id);
    }
}
