using Helperland.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Helperland.viewmodel
{
    public class DashboardViewModel
    {
        public int? Day { get; set; }

        public int? Month { get; set; }

        public int? Year { get; set; }

        public List<string> spName { get; set; }

        public List<ServiceRequest> services { get; set; }

        public ServiceRescheduleViewModel serviceReschedule { get; set; }

        public User user { get; set; }

        public List<UserAddress> Address { get; set; }


        public List<DateTime> serviceEnd { get; set; }

        public CancleServiceViewModel cancelService { get; set; }

        public Rating rating { get; set; }

        public List<Decimal> rate { get; set; }

        public DeleteAddressViewModel deleteAddress { get; set; }

        public UserAddress add { get; set; }

        public ChangePasswordViewModel changePassword { get; set; }
    }
}
