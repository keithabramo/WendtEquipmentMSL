using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace WendtEquipmentTracking.App.Models
{
    public class VendorModel
    {
        public int VendorId { get; set; }
        public int ProjectId { get; set; }

        [DisplayName("Vendor")]
        [Remote("ValidVendorName", "Validate", AdditionalFields = "VendorId", ErrorMessage = "This vendor name already exists")]
        [Required]
        public string Name { get; set; }

        [DisplayName("Address")]
        [Required]
        public string Address { get; set; }

        [DisplayName("Contact 1")]
        [Required]
        public string Contact1 { get; set; }

        [DisplayName("Phone/Fax")]
        public string PhoneFax { get; set; }

        [DisplayName("Email")]
        public string Email { get; set; }

        public bool IsDuplicate { get; set; }
    }
}