using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Helperland.viewmodel
{
    public class ServiceRescheduleViewModel
    {
        [DataType(DataType.Date)]
        [Required]
        public string serviceDate { get; set; }

        [Required]
        public int serviceTime { get; set; }
        public object ServiceId { get; internal set; }
    }
}
