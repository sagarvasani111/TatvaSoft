using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Helperland.viewmodel
{
    public class AdminRescheduleViewModel
    {
        public int serviceId { get; set; }
        public DateTime ServiceDate { get; set; }

        public int time { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public string PostalCode { get; set; }

        public string City { get; set; }

        public string IAddressLine1 { get; set; }

        public string IAddressLine2 { get; set; }

        public string IPostalCode { get; set; }

        public string ICity { get; set; }

        public string rescheduleCmt { get; set; }

        public string EMP { get; set; }
    }
}
