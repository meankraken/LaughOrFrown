using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LaughOrFrown.ViewModels
{
    public class AddJokeViewModel
    {
        [Required]
        [StringLength(50, MinimumLength = 5)] 
        public string JokeText { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 1)]
        public string Punchline { get; set; }
    }
}
