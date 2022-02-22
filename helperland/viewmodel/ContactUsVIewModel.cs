using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Helperland.viewmodel
{
    public class ContactUsVIewModel
    {
        [Required(ErrorMessage ="Please enter your Name")]
        public string Name { get; set; }
        [Required(ErrorMessage ="Please enter your Last Name")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Please enter your Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please enter your Phone Number")]
        [EmailAddress]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Please enter the Subject")]
        public string Subject { get; set; }
        [Required(ErrorMessage = "Please enter Message")]
        public string Message { get; set; }
    }
}
