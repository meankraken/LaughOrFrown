using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaughOrFrown.Models
{
    public class LaughContextSeed
    {
        private LaughContext _context;
        private UserManager<LaughUser> _userManager;
        private ILogger<LaughContextSeed> _logger;

        public LaughContextSeed(LaughContext context, UserManager<LaughUser> userManager, ILogger<LaughContextSeed> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task EnsureSeeded() //initial seed for LaughDb
        {
            if (await _userManager.FindByEmailAsync("admin@domain.com") == null)
            {
                var newUser = new LaughUser()
                {
                    UserName = "admin",
                    Email = "admin@domain.com",
                    Jokes = new List<Joke>()
                    {
                        new Joke()
                        {
                            JokeText = "Why did the chicken cross the road?",
                            Punchline = "To get to the other side.",
                            DateCreated = DateTime.UtcNow,
                            Uploader = "admin"
                        },
                        new Joke()
                        {
                            JokeText = "What do you call two Mexicans playing basketball?",
                            Punchline = "Juan on Juan.",
                            DateCreated = DateTime.UtcNow,
                            Uploader = "admin"
                        }
                    }
                };

                //create the default admin user with two default jokes in the jokes collection
                var userResult = await _userManager.CreateAsync(newUser, "passw0rd");
                
                if (!userResult.Succeeded)
                {
                    Console.WriteLine("ERRORS CREATING USER: " + userResult.Errors);
                }

                //add the jokes to the jokes data set
                if (!_context.Jokes.Any())
                {
                    _context.Jokes.AddRange(newUser.Jokes);

                    await _context.SaveChangesAsync();
                }

            }
            
            //var testJoke = new Joke() { JokeText = "Test" };
            //_context.Users.Include(p => p.Jokes).Where(u => u.UserName == "admin").FirstOrDefault().Jokes.Add(testJoke);
            //await _context.SaveChangesAsync();

        }

    }
}
