using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using restapiapp.Data;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace restapiapp.Controllers
{
    [Route("/create")]
    public class Register : Controller
    {
        private readonly ApplicationDbContext _context;
        // GET: /<controller>/

        public Register(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        public IActionResult Create()
        {
            RegisterModel register = new RegisterModel
            {
                id = 2,
                name = "Mateusz",
                lastname = "Kwiatowski"
            };

            _context.Registers.Add(register);
            _context.SaveChanges();

            return Json(register);
        }

        [HttpGet]
        public IActionResult GetAllRegisters()
        {
            var registers = _context.Registers.ToList();
            return Json(registers);
        }


    }
}

