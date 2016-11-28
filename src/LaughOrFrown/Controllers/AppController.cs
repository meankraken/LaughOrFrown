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

        public async Task<IActionResult> Index()
        {
            var thisUser = await _userManager.GetUserAsync(HttpContext.User);
            var userStats = new UserStatsViewModel();
            if (thisUser!=null)
            {
                userStats.UserName = thisUser.UserName;
                userStats.Jokes = thisUser.Jokes;
                userStats.Ratings = thisUser.Ratings;
            }
            else
            {
                userStats.UserName = "";
            }

            return View(userStats); 
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
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index");
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

        public IActionResult TopJokes() //return the Jokes View and pass in the jokes ordered by their Hot rating
        {
            var theJokes = _repo.GetJokes();
            var jokesVM = Mapper.Map<IEnumerable<JokeViewModel>>(theJokes);

            foreach (JokeViewModel item in jokesVM)
            {
                item.HotAverageRating = getAverageHotRating(item.Ratings);
                //item.OffensiveAverageRating = getAverageOffensiveRating(item.Ratings);
            }
            jokesVM = jokesVM.OrderByDescending(j => j.HotAverageRating);

            return View("Jokes",jokesVM);
        }

        public IActionResult OffensiveJokes() //return the Jokes View and pass in the jokes ordered by their Offensive rating
        {
            var theJokes = _repo.GetJokes();
            var jokesVM = Mapper.Map<IEnumerable<JokeViewModel>>(theJokes);
            
            foreach (JokeViewModel item in jokesVM)
            {
                //item.HotAverageRating = getAverageHotRating(item.Ratings);
                item.OffensiveAverageRating = getAverageOffensiveRating(item.Ratings);
            }
            jokesVM = jokesVM.OrderByDescending(j => j.OffensiveAverageRating);

            return View("Jokes", jokesVM);
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

        private double getAverageHotRating(ICollection<Rating> ratings) //helper function to get the average hot rating out of a collection of ratings
        {
            double average = 0;
            foreach (var rating in ratings)
            {
                average += rating.HotRating;
            }
            return average / ratings.Count;
        }

        private double getAverageOffensiveRating(ICollection<Rating> ratings) //helper function to get the average offensive rating out of a collection of ratings
        {
            double average = 0;
            foreach (var rating in ratings)
            {
                average += rating.OffensiveRating;
            }
            return average / ratings.Count;
        }

    }
}
