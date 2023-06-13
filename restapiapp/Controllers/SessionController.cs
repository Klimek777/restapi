using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace restapiapp.Controllers
{
    [Route("/session")]
    public class SessionController : Controller
    {
        [HttpGet("/session/check")]
        public IActionResult CheckSession()
        {
            // Sprawdź, czy sesja jest aktywna
            if (HttpContext.Session.GetString("UserId") != null)
            {
                string session = HttpContext.Session.GetString("UserId");
                return Ok(session);
            }

            return Unauthorized(); // Sesja nieaktywna
        }
    }
}

