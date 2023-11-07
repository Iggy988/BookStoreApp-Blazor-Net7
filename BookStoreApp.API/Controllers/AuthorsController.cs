﻿using System;
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

namespace BookStoreApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly BookStoreDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthorsController> _logger;

        public AuthorsController(BookStoreDbContext context, IMapper mapper, ILogger<AuthorsController> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        // GET: api/Authors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorReadOnlyDto>>> GetAuthors()
        {
            if (_context.Authors == null)
            {
                return NotFound();
            }

            try
            {
                var authors = await _context.Authors.ToListAsync();
                var authorDto = _mapper.Map<IEnumerable<AuthorReadOnlyDto>>(authors);
                return Ok(authorDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error Performing GET in {nameof(GetAuthors)}");
                return StatusCode(500, Messages.Error500Message);
            }
        }

        // GET: api/Authors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Author>> GetAuthor(int id)
        {
            if (_context.Authors == null)
            {
                return NotFound();
            }

            try
            {
                var author = await _context.Authors.FindAsync(id);

                if (author == null)
                {
                    _logger.LogWarning($"Record not found: {nameof(GetAuthor)} -ID: {id}");
                    return NotFound();
                }

                var authorDto = _mapper.Map<AuthorReadOnlyDto>(author);
                return Ok(authorDto);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, $"Error Performing GET in {nameof(GetAuthors)}");
                return StatusCode(500, Messages.Error500Message);
            }

            
        }

        // PUT: api/Authors/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAuthor(int id, AuthorUpdateDto authorDto)
        {
            if (id != authorDto.Id)
            {
                _logger.LogWarning($"Update ID invalid in: {nameof(PutAuthor)} -ID: {id}");
                return BadRequest();
            }

            var author = await _context.Authors.FindAsync(id);

            if (author == null)
            {
                _logger.LogWarning($"{nameof(Author)} record not found in {nameof(PutAuthor)} -ID: {id}");
                return NotFound();
            }
            // sve sto je drugacije u authorDto, kopiraj u author
            _mapper.Map(authorDto, author);
            _context.Entry(author).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
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
        public async Task<ActionResult<AuthorCreateDto>> PostAuthor(AuthorCreateDto authorDto)
        {
            if (_context.Authors == null)
            {
                return Problem("Entity set 'BookStoreDbContext.Authors'  is null.");
            }

            try
            {
                var author = _mapper.Map<Author>(authorDto);
                await _context.Authors.AddAsync(author);
                await _context.SaveChangesAsync();

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
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            if (_context.Authors == null)
            {
                return NotFound();
            }

            try
            {
                var author = await _context.Authors.FindAsync(id);
                if (author == null)
                {
                    return NotFound();
                }

                _context.Authors.Remove(author);
                await _context.SaveChangesAsync();

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
            return await _context.Authors.AnyAsync(e => e.Id == id);
        }
    }
}