using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace LaughOrFrown.Controllers
{
    public class AppController : Controller
    {
        public IActionResult Index()
        {
            return View(); 
        }

        [HttpPost]
        public IActionResult Login()
        {
            return Content("Sup");
        }

        [HttpPost]
        public IActionResult Register()
        {
            return Content("Register");
        }

        public IActionResult Jokes()
        {
            return View();
        }
    }
}
