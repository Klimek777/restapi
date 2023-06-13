using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace restapiapp.Controllers
{
    [Route("/session")]
    public class SessionController : Controller
    {
        [Route("/session/check")]
        [HttpGet]
        public IActionResult CheckSession()
        {
            try
            {
                // Pobierz token z nagłówka żądania
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

                // Weryfikuj poprawność tokena
                var jwtKey = "eyJhbGciOiJIUzI1NiJ9.eyJyb2xlIjoidXNlciJ9.FI_Nhrd8CqKObnhWpwBehNEVs69LEgk5AWQlbXdT518"; // Klucz JWT, używany do generowania i weryfikacji tokenów
                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtKey)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };

                SecurityToken validatedToken;
                tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

                // Token jest poprawny, zwróć odpowiedź HTTP 200 OK
                return Ok("pozdro");
            }
            catch (Exception)
            {
                // Token jest niepoprawny lub wygasł, zwróć odpowiedź HTTP 401 Unauthorized
                return Unauthorized();
            }
        }

    }
}

