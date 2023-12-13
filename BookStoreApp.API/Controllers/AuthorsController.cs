using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookStoreApp.API.Data;
using BookStoreApp.API.Models.Author;
using AutoMapper;
using BookStoreApp.API.Static;
using Microsoft.AspNetCore.Authorization;
using AutoMapper.QueryableExtensions;
using BookStoreApp.API.Repositories;
using BookStoreApp.API.Models;

namespace BookStoreApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorsRepository _authorsRepository;

        //private readonly BookStoreDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthorsController> _logger;

        public AuthorsController(/*BookStoreDbContext context,*/IAuthorsRepository authorsRepository, IMapper mapper, ILogger<AuthorsController> logger)
        {
            _authorsRepository = authorsRepository;
            //_context = context;
            _mapper = mapper;
            _logger = logger;
        }

        // GET: api/Authors/?startindex=0&pagesize=15
        [HttpGet]
        //public async Task<ActionResult<IEnumerable<AuthorReadOnlyDto>>> GetAuthors()
        public async Task<ActionResult<VirtualizeResponse<AuthorReadOnlyDto>>> GetAuthors([FromQuery]QueryParameters queryParameters)
        {
            //if (_context.Authors == null)
            //{
            //    return NotFound();
            //}

            try
            {
                //var authors = await _authorsRepository.GetAllAsync();
                //var authorsDto = await _authorsRepository.GetAllAsync<AuthorReadOnlyDto>(queryParameters);
                //var authorDto = _mapper.Map<IEnumerable<AuthorReadOnlyDto>>(authors);
                //return Ok(authorsDto);
                return await _authorsRepository.GetAllAsync<AuthorReadOnlyDto>(queryParameters);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error Performing GET in {nameof(GetAuthors)}");
                return StatusCode(500, Messages.Error500Message);
            }
        }

        // GET: api/Authors/GetAll
        [HttpGet("GetAll")]
        public async Task<ActionResult<List<AuthorReadOnlyDto>>> GetAuthors()
        {
            try
            {
                var authors = await _authorsRepository.GetAllAsync();
                var authorDtos = _mapper.Map<IEnumerable<AuthorReadOnlyDto>>(authors);
                return Ok(authorDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error Performing GET in {nameof(GetAuthors)}");
                return StatusCode(500, Messages.Error500Message);
            }
        }


        // GET: api/Authors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorDetailsDto>> GetAuthor(int id)
        {
            //if (_context.Authors == null)
            //{
            //    return NotFound();
            //}

            try
            {
                var author = _authorsRepository.GetAuthorDetailsAsync(id);
                //var author = await _context.Authors
                //    .Include(x => x.Books)
                //    .ProjectTo<AuthorDetailsDto>(_mapper.ConfigurationProvider)
                //    .FirstOrDefaultAsync(x => x.Id == id);
                    

                if (author == null)
                {
                    _logger.LogWarning($"Record not found: {nameof(GetAuthor)} -ID: {id}");
                    return NotFound();
                }

                //var authorDto = _mapper.Map<AuthorReadOnlyDto>(author);
                return Ok(author);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, $"Error Performing GET in {nameof(GetAuthors)}");
                return StatusCode(500, Messages.Error500Message);
            }

            
        }

        // PUT: api/Authors/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Administartor")] 
        public async Task<IActionResult> PutAuthor(int id, AuthorUpdateDto authorDto)
        {
            if (id != authorDto.Id)
            {
                _logger.LogWarning($"Update ID invalid in: {nameof(PutAuthor)} -ID: {id}");
                return BadRequest();
            }

            var author = await _authorsRepository.GetAsync(id);

            if (author == null)
            {
                _logger.LogWarning($"{nameof(Author)} record not found in {nameof(PutAuthor)} -ID: {id}");
                return NotFound();
            }
            // sve sto je drugacije u authorDto, kopiraj u author
            _mapper.Map(authorDto, author);
            //_context.Entry(author).State = EntityState.Modified;

            try
            {
                await _authorsRepository.UpdateAsync(author);
                //await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!await AuthorExists(id))
                {
                    return NotFound();
                }
                else
                {
                    _logger.LogError(ex, $"Error Performing PUT in {nameof(PutAuthor)}");
                    return StatusCode(500, Messages.Error500Message);
                }
            }

            return NoContent();
        }

        // POST: api/Authors
        [HttpPost]
        [Authorize(Roles = "Administartor")]
        public async Task<ActionResult<AuthorCreateDto>> PostAuthor(AuthorCreateDto authorDto)
        {
            //if (_context.Authors == null)
            //{
            //    return Problem("Entity set 'BookStoreDbContext.Authors'  is null.");
            //}

            try
            {
                var author = _mapper.Map<Author>(authorDto);
                await _authorsRepository.AddAsync(author);
                //await _context.Authors.AddAsync(author);
                //await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetAuthor), new { id = author.Id }, author);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, $"Error Performing POST in {nameof(PostAuthor)}");
                return StatusCode(500, Messages.Error500Message);
            }
            
        }

        // DELETE: api/Authors/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administartor")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            //if (_context.Authors == null)
            //{
            //    return NotFound();
            //}

            try
            {
                var author = await _authorsRepository.GetAsync(id);
                //var author = await _context.Authors.FindAsync(id);
                if (author == null)
                {
                    _logger.LogWarning($"{nameof(Author)} record not found in {nameof(DeleteAuthor)} - ID: {id}");
                    return NotFound();
                }

                await _authorsRepository.DeleteAsync(id);
                //_context.Authors.Remove(author);
                //await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, $"Error Performing DELETE in {nameof(DeleteAuthor)}");
                return StatusCode(500, Messages.Error500Message);
            }
        }

        private async Task<bool> AuthorExists(int id)
        {
            //return await _context.Authors.AnyAsync(e => e.Id == id);
            return await _authorsRepository.Exists(id);
        }
    }
}
