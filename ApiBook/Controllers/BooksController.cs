using Infrastructure.Contexts;
using Infrastructure.Dtos;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiBook.Controllers
{
    [Route("api/[controller]")]
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
                      PublicationDate = dto.PublicationDate,
                      ImageName = dto.ImageName,


                    };
                    _context.Books.Add(bookEntity);
                    await _context.SaveChangesAsync();
                    return Created("", null);
                }

                return Conflict();
            }
            return BadRequest();
        }
        #endregion

        #region GET
        [HttpGet]

        public async Task<IActionResult> GetAll()
        {
            var courses = await _context.Books.ToListAsync();
            return Ok(courses);
        }

        [HttpGet("{id}")]

        public async Task<IActionResult> GetOne(Guid id)
        {
            var courseEntity = await _context.Books.FirstOrDefaultAsync(x => x.Id == id);
            if (courseEntity != null)
            {
                return Ok(courseEntity);
            }
            return NotFound();
        }
        #endregion

        #region UPDATE

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id,  BookDto dto)
        {
            if (ModelState.IsValid)
            {
                var book = await _context.Books.FirstOrDefaultAsync(x => x.Id == id);
                if (book != null)
                {
                    book.Title = dto.Title;
                    book.Description = dto.Description;
                    book.Author = dto.Author;
                    book.PublicationDate = dto.PublicationDate;
                    book.ImageName = dto.ImageName;
                  

                    _context.Books.Update(book);
                    await _context.SaveChangesAsync();

                    return Ok();
                }
                return NotFound();
            }
            return BadRequest();
        }

        #endregion


        #region Delete
        [HttpDelete("{id}")]

        public async Task<IActionResult> Delete(Guid id)
        {
            var course = await _context.Quotes.FirstOrDefaultAsync(x => x.Id == id);
            if (course != null)
            {
                _context.Quotes.Remove(course);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return NotFound();
        }
        #endregion

    }
}
