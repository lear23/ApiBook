using Infrastructure.Contexts;
using Infrastructure.Dtos;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiBook.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BooksController(AppDbContext context)
        {
            _context = context;
        }

        #region CREATE
        [HttpPost]
        public async Task<IActionResult> Create(BookDto dto)
        {
            if (ModelState.IsValid)
            {
                if (!await _context.Books.AnyAsync(x => x.Title == dto.Title))
                {
                    var bookEntity = new BookEntity
                    {
                        Title = dto.Title,
                        Description = dto.Description,
                        Author = dto.Author,
                        ImageName = dto.ImageName
                    };

                    _context.Books.Add(bookEntity);
                    await _context.SaveChangesAsync();
                    return CreatedAtAction(nameof(GetOne), new { id = bookEntity.Id }, bookEntity);
                }

                return Conflict("A book with this title already exists.");
            }
            return BadRequest(ModelState);
        }
        #endregion

        #region GET
        [HttpGet]
        [Route("List")]
        public async Task<IActionResult> GetAll()
        {
            var books = await _context.Books.ToListAsync();
            return Ok(new { value = books });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(Guid id)
        {
            var bookEntity = await _context.Books.FirstOrDefaultAsync(x => x.Id == id);
            if (bookEntity != null)
            {
                return Ok(bookEntity);
            }
            return NotFound();
        }
        #endregion

        #region UPDATE
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] BookDto dto)
        {
            if (ModelState.IsValid)
            {
                var book = await _context.Books.FirstOrDefaultAsync(x => x.Id == id);
                if (book != null)
                {
                    book.Title = dto.Title;
                    book.Description = dto.Description;
                    book.Author = dto.Author;
                    book.ImageName = dto.ImageName;

                    _context.Books.Update(book);
                    await _context.SaveChangesAsync();

                    return Ok(book);
                }
                return NotFound();
            }
            return BadRequest(ModelState);
        }
        #endregion

        #region DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var book = await _context.Books.FirstOrDefaultAsync(x => x.Id == id);
            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            return NotFound();
        }
        #endregion
    }
}
