using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaughOrFrown.Models
{
    public class LaughUser : IdentityUser 
    {
        public ICollection<Joke> Jokes { get; set; } //Jokes navigation property to pull all the jokes uploaded by a user
    }
}
