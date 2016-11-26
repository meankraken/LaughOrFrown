using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaughOrFrown.Models
{
    public class Rating //join table for users and jokes loaded with ratings
    {
        public int Id { get; set; }
        
        public int HotRating { get; set; }

        public int OffensiveRating { get; set; }
        
        public LaughUser User { get; set; }

        public Joke Joke { get; set; }

    }
}
