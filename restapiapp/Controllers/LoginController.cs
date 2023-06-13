using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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

            HttpContext.Session.SetString("UserId", user.Id.ToString());
            String session = HttpContext.Session.GetString("UserId");



            return Ok(session);
        }

        
        
        

    }
}

