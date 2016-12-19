using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LaughOrFrown.ViewModels
{
    public class UserViewModel
    {
        [Required]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 15 characters long.")]
        public string Username { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "Password must be at least 8 chars and contain both letters and numbers.")]
        public string Password { get; set; }
    }
}
