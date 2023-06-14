using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using restapiapp.Data;
using restapiapp.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace restapiapp.Controllers
{
    
    [Route("/login")]
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LoginController(ApplicationDbContext context)
        {
            _context = context;
        }
        private string GenerateJwtToken(User user)
        {
            var jwtKey = "eyJhbGciOiJIUzI1NiJ9.eyJyb2xlIjoidXNlciJ9.FI_Nhrd8CqKObnhWpwBehNEVs69LEgk5AWQlbXdT518"; // Ten sam klucz, który został użyty w Program.cs
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
            new Claim(ClaimTypes.Name, user.Email)
                }),
                Expires = DateTime.UtcNow.AddHours(1), // Token wygasa po 1 godzinie
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private bool IsValidUserCredentials(User userCredentials)
        {
            // Sprawdź, czy wszystkie pola są wypełnione
            if (string.IsNullOrEmpty(userCredentials.Email) || string.IsNullOrEmpty(userCredentials.Password))
            {
                return false;
            }

            return true;
        }

        private User AuthenticateUser(User userCredentials)
        {
            // Znajdź użytkownika o podanym adresie e-mail i haśle w bazie danych
            return _context.Users.SingleOrDefault(user => user.Email == userCredentials.Email && user.Password == userCredentials.Password);
        }

       
        [HttpPost]
        [Route("/login/authenticate")]
        public IActionResult Authenticate([FromBody] User userCredentials)
        {
            if (!IsValidUserCredentials(userCredentials))
            {
                return BadRequest("Invalid user credentials");
            }

            // Znajdź użytkownika w bazie danych
            var user = AuthenticateUser(userCredentials);
            if (user == null)
            {
                return BadRequest("Invalid email or password");
            }
            // Generuj token JWT
            var token = GenerateJwtToken(user);

            return Ok(new { Token = token, userId = user.Id, username = user.Name });


        }
    }
}

