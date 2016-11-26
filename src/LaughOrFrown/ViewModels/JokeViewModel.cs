﻿using LaughOrFrown.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaughOrFrown.ViewModels
{
    public class JokeViewModel
    {
        public string JokeText { get; set; }

        public string Punchline { get; set; }

        public DateTime DateCreated { get; set; }

        public string Uploader { get; set; }

        public ICollection<Rating> Ratings { get; set; }
    }
}
