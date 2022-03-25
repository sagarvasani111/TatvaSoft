using Helperland.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Helperland.viewmodel
{
    public class AdminViewModel
    {
        public AdminRescheduleViewModel adminReschedule { get; set; }

        public List<ServiceRequest> services { get; set; }

        public List<User> users { get; set; }

        public List<string> Name { get; set; }

        public List<ServiceRequestAddress> address { get; set; }

        public List<DateTime> serviceEnd { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public List<DateTime> serviceDate { get; set; }

        public List<double> serviceStart { get; set; }
    }
}
