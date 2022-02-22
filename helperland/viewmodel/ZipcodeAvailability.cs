using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Helperland.viewmodel
{
    public class ZipcodeAvailability
    {
        [Required(ErrorMessage ="Please Enter Valid ZipCode")]
        public string ZipCode { get; set; }
    }
}
