using Infrastructure.Contexts;
using Infrastructure.Dtos;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiBook.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
    [ApiController]
    public class QuotesController(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;



        #region CREATE
        [HttpPost]
        public async Task<IActionResult> Create(QuoteDto dto)
        {
          var client = await _context.Clients.FindAsync(dto.ClientId);
            if (client == null)
            {
                return NotFound($"Client with ID {dto.ClientId} was not found.");
            }

            var quoteEntity = new QuoteEntity
            {
                Text = dto.Text,
                Author = dto.Author,
                ClientId = dto.ClientId
            };

            try
            {
                _context.Quotes.Add(quoteEntity);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetOne), new { id = quoteEntity.Id }, quoteEntity);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            
        }
        #endregion


        #region GETALL
        [HttpGet]
        [Route("List")]
        public async Task<IActionResult> GetAll()
        {
            var quotes = await _context.Quotes.ToListAsync();
            return Ok(quotes);
        }
        #endregion



        #region GET
        [HttpGet("{id}")]

        public async Task<IActionResult> GetOne(Guid id)
        {
            var quoteEntity = await _context.Quotes.FirstOrDefaultAsync(x => x.Id == id);
            if (quoteEntity != null)
            {
                return Ok(quoteEntity);
            }
            return NotFound();
        }
        #endregion

        #region UPDATE

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, QuoteDto dto)
        {
            if (ModelState.IsValid)
            {
                var quote = await _context.Quotes.FirstOrDefaultAsync(x => x.Id == id);
                if (quote != null)
                {
                    quote.Text = dto.Text;
                    quote.Author = dto.Author;


                    _context.Quotes.Update(quote);
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
            var quote = await _context.Quotes.FirstOrDefaultAsync(x => x.Id == id);
            if (quote != null)
            {
                _context.Quotes.Remove(quote);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return NotFound();
        }
        #endregion

    }
}
