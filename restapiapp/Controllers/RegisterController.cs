using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using restapiapp.Data;
using restapiapp.Models;


namespace restapiapp.Controllers
{
    [Route("/register")]
    public class RegisterController : Controller
    {
        private readonly ApplicationDbContext _context;
    
        public RegisterController(ApplicationDbContext context)
        {
            _context = context;
        }

        private bool IsValidUserData(User userData)
        {
            // Sprawdź, czy wszystkie pola są wypełnione
            if (string.IsNullOrEmpty(userData.Name) || string.IsNullOrEmpty(userData.Email) || string.IsNullOrEmpty(userData.Password))
            {
                return false;
            }

            // Sprawdź, czy adres e-mail jest poprawny
            if (!IsValidEmail(userData.Email))
            {
                return false;
            }

            // Dodaj dodatkowe warunki walidacji, jeśli to konieczne

            return true;
        }
        private bool IsEmailAlreadyRegistered(string email)
        {
            // Sprawdź, czy w bazie danych istnieje użytkownik o podanym adresie e-mail
            return _context.Users.Any(user => user.Email == email);
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var mailAddress = new System.Net.Mail.MailAddress(email);
                return mailAddress.Address == email;
            }
            catch
            {
                return false;
            }
        }

        [HttpPost]
        [Route("/register/create")]
        public IActionResult RegisterUser([FromBody] User userData)
        {
            //// Sprawdź poprawność danych
            if (!IsValidUserData(userData))
            {
                return BadRequest("Invalid user data");
            }

            // Sprawdź unikalność adresu e-mail (przykładowe założenie)
            if (IsEmailAlreadyRegistered(userData.Email))
            {
                return BadRequest("Email address is already registered");
            }

            User user= new User
            {
                Name = userData.Name,
                Email = userData.Email,
                Password = userData.Password,
            };
            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok(user);
        }

        [HttpGet]
        public IActionResult GetAllRegisters()
        {
            var users = _context.Users.ToList();
            return Json(users);
        }


    }
}

