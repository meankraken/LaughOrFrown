using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LaughOrFrown.Models;

namespace LaughOrFrown.Controllers
{
    public class AppController : Controller
    {
        private ILaughRepository _repo;

        public AppController(ILaughRepository repo)
        {
            _repo = repo;
        }

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
            var theJokes = _repo.GetJokes();
            return View(theJokes);
        }

        
    }
}
