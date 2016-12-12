using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaughOrFrown.Models
{
    public class LaughUser : IdentityUser 
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string State { get; set; }

        public string City { get; set; }

        public string ZipCode { get; set; } 

        public ICollection<Joke> Jokes { get; set; } //Jokes navigation collection to hold all jokes uploaded by the user

        public ICollection<Rating> Ratings { get; set; } //Ratings navigation collection to hold all ratings given by the user
    }
}
