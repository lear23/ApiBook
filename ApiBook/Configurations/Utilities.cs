using Infrastructure.Dtos;
using Infrastructure.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ApiBook.Configurations
{
    public class Utilities
    {
        private readonly IConfiguration _configuration;

        public Utilities(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public string EncryptSHA256(string text)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(text));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                return builder.ToString();

            }
        }
        public string GenerateJWT(ClientEntity model)
        {
            var userClaims = new[]
            {
               new Claim(ClaimTypes.NameIdentifier, model.Id.ToString()),
               new Claim(ClaimTypes.Email, model.Email)
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var jwtConfig = new JwtSecurityToken(
                claims: userClaims,
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: credentials

                );


            return new JwtSecurityTokenHandler().WriteToken(jwtConfig);
        }



    }
}




//    public string GenerateJWT(ClientDto model)
//{
//    var userClaims = new[]
//    {
//                new Claim(ClaimTypes.NameIdentifier, model.Email)
//            };
//}