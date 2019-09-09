using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace WendtEquipmentTracking.App.Models
{
    public class BrokerModel
    {
        public int BrokerId { get; set; }

        [DisplayName("Broker")]
        [Remote("ValidBrokerName", "Validate", AdditionalFields = "BrokerId", ErrorMessage = "This broker name already exists")]
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