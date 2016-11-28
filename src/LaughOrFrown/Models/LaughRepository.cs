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

        public Joke GetJoke(int id) //get single joke with id
        {
            return _context.Jokes.Where(j => j.Id == id).FirstOrDefault();
        }

        public IEnumerable<Joke> GetJokes() //get all jokes
        {
            return _context.Jokes.Include(j => j.Ratings).ToList();
        }
    }
}
