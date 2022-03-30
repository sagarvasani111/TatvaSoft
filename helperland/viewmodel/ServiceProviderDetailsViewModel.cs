using Helperland.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Helperland.viewmodel
{
    public class ServiceProviderDetailsViewModel
    {
        public User User { get; set; }

        public UserAddress Address { get; set; }
 
        public UserAddress AddID { get; set; }
        public string Addressline1 { get; set; }
        public string Addressline2 { get; set; }
        public string City { get; set; }
        public string Zipcode { get; set; }
    }
}
