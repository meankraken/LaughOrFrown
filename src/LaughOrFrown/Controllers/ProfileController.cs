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
            var profileVM = Mapper.Map<ProfileViewModel>(user);

            return View(profileVM);
        } 

        public async Task<IActionResult> Edit() //edit view
        {
            ViewBag.Title = "Profile";

            var user = await _userManager.GetUserAsync(HttpContext.User);
            var profileVM = Mapper.Map<ProfileViewModel>(user);

            return View(profileVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProfileViewModel profile)
        {
            ViewBag.Title = "Profile";
            var user = await _userManager.GetUserAsync(HttpContext.User);
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
    }
}
