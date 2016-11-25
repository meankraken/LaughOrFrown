using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaughOrFrown.Models
{
    public class Rating
    {
        public int Id { get; set; }
        
        public int HotRating { get; set; }

        public int OffensiveRating { get; set; }

        public LaughUser UserRated { get; set; }

        public Joke JokeRated { get; set; }

    }
}
