using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Helperland.viewmodel
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string old { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string new1 { get; set; }

        [DataType(DataType.Password)]
        [Compare("new1", ErrorMessage = "Password and Confirmation Password do not match")]
        public string confirm { get; set; }

    }

}
