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
using Microsoft.AspNetCore.Authorization;

namespace LaughOrFrown.Controllers
{
    public class AppController : Controller //App and joke controller
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
 
        public IActionResult Index(string ReturnUrl) //main page + login / register form 
        {
            if (!string.IsNullOrEmpty(ReturnUrl) && ReturnUrl.Length > 0)
            {
                ViewBag.Auth = "needed";
            }
            var userId = _userManager.GetUserId(HttpContext.User);
            var thisUser = _repo.GetUser(userId);
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(AddJokeViewModel theJoke)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var thisUser = _repo.GetUser(userId);
            var userStats = new UserStatsViewModel();
            if (thisUser != null)
            {
                userStats.UserName = thisUser.UserName;
                userStats.Jokes = thisUser.Jokes;
                userStats.Ratings = thisUser.Ratings;
            }
            else
            {
                userStats.UserName = "";
            }

            if (ModelState.IsValid)
            {
                var jokeToAdd = Mapper.Map<Joke>(theJoke);

                jokeToAdd.DateCreated = DateTime.Now;
                jokeToAdd.Uploader = _userManager.GetUserName(HttpContext.User);

                _repo.AddJoke(jokeToAdd);
                if (await _repo.Save())
                {
                    return RedirectToAction("Joke", new { id = jokeToAdd.Id });
                }

                
            }

            return View(userStats);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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

            var userStats = new UserStatsViewModel();
            return View("Index", userStats);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserViewModel userVM)
        {
            ViewBag.State = "Register"; //used to track if user is logging in or registering on integrated page 
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

            var userStats = new UserStatsViewModel();
            return View("Index", userStats);
        }


        //Generate joke views
        public IActionResult TopJokes(int page=1) //return the Jokes View and pass in the jokes ordered by their Hot rating
        {
            ViewBag.Title = "Top Jokes";
            var theJokes = _repo.GetJokes();
            var jokesVM = Mapper.Map<IEnumerable<JokeViewModel>>(theJokes);

            foreach (JokeViewModel item in jokesVM)
            {
                item.HotAverageRating = getAverageHotRating(item.Ratings);
                //item.OffensiveAverageRating = getAverageOffensiveRating(item.Ratings);
            }
            jokesVM = jokesVM.OrderByDescending(j => j.HotAverageRating).Skip((page-1)*12).Take(12);

            var pagedJokes = new PagedJokesViewModel(page, 12, theJokes.Count(), jokesVM); //passing the initialized pagedJokes VM to the view 

            return View("Jokes",pagedJokes);
        }

        public IActionResult OffensiveJokes(int page=1) //return the Jokes View and pass in the jokes ordered by their Offensive rating
        {
            ViewBag.Title = "Offensive Jokes";
            var theJokes = _repo.GetJokes();
            var jokesVM = Mapper.Map<IEnumerable<JokeViewModel>>(theJokes);
            
            foreach (JokeViewModel item in jokesVM)
            {
                //item.HotAverageRating = getAverageHotRating(item.Ratings);
                item.OffensiveAverageRating = getAverageOffensiveRating(item.Ratings);
            }
            jokesVM = jokesVM.OrderByDescending(j => j.OffensiveAverageRating).Skip((page - 1) * 12).Take(12);

            var pagedJokes = new PagedJokesViewModel(page, 12, theJokes.Count(), jokesVM);

            return View("Jokes", pagedJokes);
        } 

        public IActionResult NewJokes(int page=1) //return the Jokes View and pass in the jokes ordered by date submitted
        {
            ViewBag.Title = "New Jokes";
            var theJokes = _repo.GetJokes();
            var jokesVM = Mapper.Map<IEnumerable<JokeViewModel>>(theJokes);

            jokesVM = jokesVM.OrderByDescending(j => j.DateCreated).Skip((page - 1) * 12).Take(12);

            var pagedJokes = new PagedJokesViewModel(page, 12, theJokes.Count(), jokesVM);

            return View("Jokes", pagedJokes);
        }

        public async Task<IActionResult> Joke(int id, string returnurl) //single joke page
        {
            ViewBag.Title = "Just A Joke";
            var theUser = await _userManager.GetUserAsync(HttpContext.User);
            var theJoke = _repo.GetJoke(id);

            if (theJoke == null)
            {
                return Content("Oops! Looks like this joke doesn't exist.");
            }

            if (returnurl == null)
            {
                ViewBag.ReturnUrl = Url.Action("TopJokes");
            }
            else
            {
                ViewBag.ReturnUrl = returnurl;
            }

            var jokeViewModel = Mapper.Map<JokeViewModel>(theJoke);
            jokeViewModel.HotAverageRating = getAverageHotRating(jokeViewModel.Ratings);
            jokeViewModel.OffensiveAverageRating = getAverageOffensiveRating(jokeViewModel.Ratings);

            var usersRating = new Rating(); //hold user's rating for the joke if it exists

            foreach (var rating in jokeViewModel.Ratings)
            {
                if (rating.User == theUser)
                {
                    usersRating = rating;
                }
            }

            if (usersRating.User != null)
            {
                jokeViewModel.UsersRating = usersRating;
            }

            return View(jokeViewModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Joke(RatingViewModel rating) //submit rating for joke
        {
            ViewBag.Title = "Just A Joke";
            ViewBag.ReturnUrl = Url.Action("TopJokes");
            var theUser = await _userManager.GetUserAsync(HttpContext.User);

            var theJoke = _repo.GetJoke(rating.JokeId);
            var jokeViewModel = Mapper.Map<JokeViewModel>(theJoke);
            jokeViewModel.HotAverageRating = getAverageHotRating(jokeViewModel.Ratings);
            jokeViewModel.OffensiveAverageRating = getAverageOffensiveRating(jokeViewModel.Ratings);

            if (!ModelState.IsValid)
            {
                ModelState.ClearValidationState("HotRating");
                ModelState.ClearValidationState("OffensiveRating");
                ModelState.AddModelError("", "Must select a hot and offensiveness rating.");
            }
            else if (theJoke.Uploader == theUser.UserName) //if rating own joke
            {
                ModelState.ClearValidationState("HotRating");
                ModelState.ClearValidationState("OffensiveRating");
                ModelState.AddModelError("", "You can't rate your own joke. Nice try...");
            }
            else
            {
                var existingRating = new Rating();

                foreach (var item in theJoke.Ratings)
                {
                    if (item.User == theUser)
                    {
                        existingRating = item;
                    }
                }

                if (existingRating.User != null)
                {
                    _repo.UpdateRating(existingRating.Id, rating.HotRating, rating.OffensiveRating);
                }
                else
                {
                    var jokeRating = Mapper.Map<Rating>(rating);
                    jokeRating.User = theUser;
                    jokeRating.Joke = theJoke;
                    _repo.AddRating(jokeRating);
                }

                if (await _repo.Save())
                {
                    return RedirectToAction("Joke");
                }
            }

            return View(jokeViewModel);
        }

        public async Task<IActionResult> Random() //produce random joke
        {
            ViewBag.Title = "So random...";
            var theUser = await _userManager.GetUserAsync(HttpContext.User);

            var jokes = _repo.GetJokes();
            var count = jokes.Count();

            var rand = new Random();
            var index = rand.Next(count); //grab a random joke index

            var theJoke = jokes.ElementAt(index);

            ViewBag.ReturnUrl = Url.Action("TopJokes");
            
            var jokeViewModel = Mapper.Map<JokeViewModel>(theJoke);
            jokeViewModel.HotAverageRating = getAverageHotRating(jokeViewModel.Ratings);
            jokeViewModel.OffensiveAverageRating = getAverageOffensiveRating(jokeViewModel.Ratings);

            var usersRating = new Rating();

            foreach (var rating in jokeViewModel.Ratings)
            {
                if (rating.User == theUser)
                {
                    usersRating = rating;
                }
            }

            if (usersRating.User != null)
            {
                jokeViewModel.UsersRating = usersRating;
            }

            return View("Joke", jokeViewModel);
        }



        //helper functions
        private double getAverageHotRating(ICollection<Rating> ratings) //helper function to get the average hot rating out of a collection of ratings
        {
            if (ratings.Count == 0)
            {
                return -1;
            }

            double average = 0;
            foreach (var rating in ratings)
            {
                average += rating.HotRating;
            }
            return average / ratings.Count;
        }

        private double getAverageOffensiveRating(ICollection<Rating> ratings) //helper function to get the average offensive rating out of a collection of ratings
        {
            if (ratings.Count == 0)
            {
                return -1;
            }

            double average = 0;
            foreach (var rating in ratings)
            {
                average += rating.OffensiveRating;
            }
            return average / ratings.Count;
        }
        
    }
}
