using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LaughOrFrown.ViewModels
{
    public class RatingViewModel
    {
        [Required]
        [Range(0,5)]
        public int HotRating { get; set; }

        [Required]
        [Range(0, 5)]
        public int OffensiveRating { get; set; }

        public int JokeId { get; set; }
    }
}
