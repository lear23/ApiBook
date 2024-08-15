using ApiBook.Configurations;
using ApiBook.Models.Dtos;
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
    public class AccessController(AppDbContext context, Utilities utils) : ControllerBase
    {

        private readonly AppDbContext _context = context;
        private readonly Utilities _utils = utils;

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(ClientDto dto)
        {
            var modelUser = new ClientEntity
            {

                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Password = _utils.EncryptSHA256(dto.Password),

            };
            await _context.Clients.AddAsync(modelUser);
            await _context.SaveChangesAsync();
            if (modelUser.Id != Guid.Empty)
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = true} );
            else
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = false });
        }


        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            var userFound = await _context.Clients.Where(u => u.Email == dto.Email && u.Password == _utils.EncryptSHA256(dto.Password)).FirstOrDefaultAsync();
            if (userFound == null)
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = false, token="" });
            else
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = true, token = _utils.GenerateJWT(userFound) });


        }

    }
}
