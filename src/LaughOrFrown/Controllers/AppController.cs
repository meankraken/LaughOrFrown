using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LaughOrFrown.Models;
using AutoMapper;
using LaughOrFrown.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace LaughOrFrown.Controllers
{
    public class AppController : Controller //single controller for entire App
    {
        private ILaughRepository _repo;
        private SignInManager<LaughUser> _signInManager;
        private UserManager<LaughUser> _userManager;
        private ILogger<AppController> _logger;

        public AppController(ILaughRepository repo, SignInManager<LaughUser> signInManager, UserManager<LaughUser> userManager, ILogger<AppController> logger)
        {
            _repo = repo;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View(); 
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserViewModel userVM)
        {
            ViewBag.State = "Login"; //used to track if user is loggin in or registering on integrated page 
            if (ModelState.IsValid)
            {            
                var loginSuccess = await _signInManager.PasswordSignInAsync(userVM.Username, userVM.Password, false, false);
                if (!loginSuccess.Succeeded)
                {
                    ModelState.AddModelError("", "Username or password incorrect. Please verify your login information and try again.");
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }

            return View("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserViewModel userVM)
        {
            ViewBag.State = "Register"; //used to track if user is loggin in or registering on integrated page 
            if (ModelState.IsValid)
            {
                var newUser = new LaughUser { UserName = userVM.Username };
                var registerSuccess = await _userManager.CreateAsync(newUser, userVM.Password);

                if (!registerSuccess.Succeeded)
                {
                    _logger.LogError(registerSuccess.Errors.ToString());
                    ModelState.AddModelError("Password", "Password must be at least 8 characters long and contain a combination of letters and numbers.");
                }
                else
                {
                    await _signInManager.SignInAsync(newUser, isPersistent: false);
                    return RedirectToAction("Index");
                }
            }

            return View("Index");
        }

        public IActionResult Jokes()
        {
            var theJokes = _repo.GetJokes();
            return View(theJokes);
        }

        public IActionResult Joke(int id, string returnurl)
        {
            var theJoke = _repo.GetJoke(id);

            if (theJoke == null)
            {
                return Content("Oops! Looks like this joke doesn't exist.");
            }

            if (returnurl == null)
            {
                ViewBag.ReturnUrl = Url.Action("Jokes");
            }
            else
            {
                ViewBag.ReturnUrl = returnurl;
            }

            var jokeViewModel = Mapper.Map<JokeViewModel>(theJoke);
            return View(jokeViewModel);
        }
        
    }
}
