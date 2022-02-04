using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Helperland.viewmodel
{
    public class RegisterViewModel
    {
        internal int UserId;

        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
      
        [Required]
        public string Mobile { get; set; }
        [Required]
        [Compare("ConfirmPassword", ErrorMessage="Passwords must match")]
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
    }
}
