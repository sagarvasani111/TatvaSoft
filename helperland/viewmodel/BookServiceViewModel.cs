using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Helperland.viewmodel
{
    public class BookServiceViewModel : RegisterViewModel
    {
        public List<AddressViewModel> address { get; set; }
        public ZipCodeViewModel zipCodeViewModel { get; set; }
        public ServiceRequestViewModel ServiceRequestViewModel { get; set; }
        public int addressId { get; set; }
        [Required]
        public bool checkPolicy { get; set; }
    }
}
