using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LaughOrFrown.Models;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using LaughOrFrown.ViewModels;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace LaughOrFrown.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private ILaughRepository  _repo;
        private SignInManager<LaughUser> _signInManager;
        private UserManager<LaughUser> _userManager;

        public ProfileController(ILaughRepository repo, SignInManager<LaughUser> signInManager, UserManager<LaughUser> userManager)
        {
            _repo = repo;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index() //main profile page
        {
            ViewBag.Title = "Profile";

            var user = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.User = user.UserName;

            var profileVM = Mapper.Map<ProfileViewModel>(user);

            return View(profileVM);
        } 

        public async Task<IActionResult> Edit() //edit view
        {
            ViewBag.Title = "Profile";

            var user = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.User = user.UserName;

            var profileVM = Mapper.Map<ProfileViewModel>(user);

            return View(profileVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProfileViewModel profile)
        {
            ViewBag.Title = "Profile";
            var user = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.User = user.UserName;

            var profileVM = Mapper.Map<ProfileViewModel>(user);

            if (!ModelState.IsValid)
            {

            }
            else if (!Regex.IsMatch(profile.ZipCode, @"^\d+$") || Int32.Parse(profile.ZipCode) < 0 || Int32.Parse(profile.ZipCode) > 99999)
            {
                ModelState.AddModelError("ZipCode", "Please enter a valid zip code.");
            }
            else
            {
                user.FirstName = profile.FirstName;
                user.LastName = profile.LastName;
                user.Email = profile.Email;
                user.City = profile.City;
                user.State = profile.State;
                user.ZipCode = profile.ZipCode;
                _repo.UpdateUser(user);

                if (await _repo.Save())
                {
                    return RedirectToAction("Index");  
                }
                else
                {
                    ModelState.AddModelError("", "There was an error updating your profile. Please try again.");
                }
            }
            return View(profileVM);
        }

        public async Task<IActionResult> Jokes(string sortby) //user's uploaded jokes 
        {
            ViewBag.Title = "Jokes";
            if (!string.IsNullOrEmpty(sortby))
            {
                ViewBag.SortBy = sortby; //parameter for which field to sort list by 
            }
            else ViewBag.SortBy = "Date"; //default set to sort by date 

            var user = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.User = user.UserName;
            var jokes = _repo.GetUserJokes(user.UserName);

            var userJokes = Mapper.Map<IEnumerable<JokeViewModel>>(jokes);

            foreach (var joke in userJokes) 
            {
                joke.HotAverageRating = getAverageHotRating(joke.Ratings);
                joke.OffensiveAverageRating = getAverageOffensiveRating(joke.Ratings);
            }

            switch (sortby)
            {
                case "Title":
                    userJokes = userJokes.OrderByDescending(o => o.JokeText);
                    break;
                case "Date":
                    userJokes = userJokes.OrderByDescending(o => o.DateCreated);
                    break;
                case "Ratings":
                    userJokes = userJokes.OrderByDescending(o => o.Ratings.Count());
                    break;
                case "HotRating":
                    userJokes = userJokes.OrderByDescending(o => o.HotAverageRating);
                    break;
                case "OffensiveRating":
                    userJokes = userJokes.OrderByDescending(o => o.OffensiveAverageRating);
                    break;
                default:
                    break;

            }

            return View(userJokes); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int jokeid)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (string.IsNullOrEmpty(user.UserName))
            {
                return RedirectToAction("Index", "App");
            }

            _repo.DeleteJoke(jokeid);

            if (await _repo.Save())
            {
                return RedirectToAction("Jokes");
            }

            return RedirectToAction("Index", "App");
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
