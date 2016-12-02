using LaughOrFrown.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaughOrFrown.ViewModels
{
    public class UserStatsViewModel //used to pass in user stats to Index view
    {
        public string UserName { get; set; }

        public ICollection<Joke> Jokes { get; set; }

        public ICollection<Rating> Ratings { get; set; }

        public UserStatsViewModel()
        {
            Jokes = new List<Joke>();

        }
    }
}
