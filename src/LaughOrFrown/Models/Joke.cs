using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaughOrFrown.Models
{
    public class Joke
    {
        public int Id { get; set; }

        public string JokeText { get; set; }

        public string Punchline { get; set; }

        public DateTime DateCreated { get; set; }

        public string Uploader { get; set; }

        //public ICollection<LaughUser> UsersLiked { get; set; }
    }
}
