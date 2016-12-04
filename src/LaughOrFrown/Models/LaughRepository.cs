using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaughOrFrown.Models
{
    public class LaughRepository : ILaughRepository //repository for interacting with DB context 
    {
        private LaughContext _context;

        public LaughRepository(LaughContext context)
        {
            _context = context;
        }

        public void AddJoke(Joke joke) //add joke
        {
            _context.Jokes.Add(joke);
            _context.Users.Where(u => u.UserName == joke.Uploader).FirstOrDefault().Jokes.Add(joke); //add joke to the user's jokes navigation collection as well  
        }

        public Joke GetJoke(int id) //get single joke with id
        {
            return _context.Jokes.Where(j => j.Id == id).FirstOrDefault();
        }

        public IEnumerable<Joke> GetJokes() //get all jokes
        {
            return _context.Jokes.Include(j => j.Ratings).ToList();
        }

        public LaughUser GetUser(string id)
        {
            return _context.Users.Include(j => j.Ratings).Include(i => i.Jokes).ThenInclude(l => l.Ratings).Where(k => k.Id == id).FirstOrDefault(); //eager loading multiple levels
        }

        public async Task<bool> Save() //save changes
        {
            return (await _context.SaveChangesAsync() > 0);
        }
    }
}
