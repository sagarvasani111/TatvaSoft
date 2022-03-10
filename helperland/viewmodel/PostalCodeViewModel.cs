using System.ComponentModel.DataAnnotations;

namespace HelperLand.ViewModels
{
    public class PostalCodeViewModel
    {
        [Required(ErrorMessage = "Postal Code is required")]
        public string PostalCode { get; set; }
    }
}