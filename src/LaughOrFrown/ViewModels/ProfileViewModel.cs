using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LaughOrFrown.ViewModels
{
    public class ProfileViewModel
    {
        public string UserName { get; set; }

        [StringLength(20,MinimumLength = 2, ErrorMessage = "Name should be between 2 and 20 characters.")]
        public string FirstName { get; set; }

        [StringLength(20, MinimumLength = 2, ErrorMessage = "Name should be between 2 and 20 characters.")]
        public string LastName { get; set; }

        [DataType(DataType.EmailAddress, ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }

        public string State { get; set; }

        public string City { get; set; }

        [StringLength(5,MinimumLength = 1)]
        [DataType(DataType.PostalCode, ErrorMessage = "Please enter a valid zip code.")]
        public string ZipCode { get; set; }
    }
}
