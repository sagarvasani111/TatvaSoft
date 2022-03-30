using Helperland.Models;
using HelperLand.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Helperland.viewmodel
{
    public class ServiceProviderViewModel
    {
        public List<ServiceRequest> services { get; set; }

        public List<ServiceRequestAddress> addresses { get; set; }
        public UserAddress userAddress1 { get; set; }
        public UserAddress userAddress2 { get; set; }
        public UserAddress Postalcode { get; set; }
        public UserAddress city { get; set; }

        public List<DateTime> serviceEnd { get; set; }

        public List<string> custName { get; set; }

        public List<string> extras { get; set; }

        public bool IsPet { get; set; }

        public AcceptServiceViewModel acceptService { get; set; }

        public DateTime time { get; set; }

        public CancleServiceViewModel cancel { get; set; }

        public List<Rating> ratings { get; set; }

        public List<string> ratingRes { get; set; }

        public User user { get; set; }

        public int? Day { get; set; }

        public int? Month { get; set; }

        public int? Year { get; set; }

        public UserAddress userAddress { get; set; }

        public ChangePasswordViewModel changePassword { get; set; }

        public List<FavoriteAndBlocked> favorites { get; set; }

        public List<int> userid { get; set; }
    }
}
