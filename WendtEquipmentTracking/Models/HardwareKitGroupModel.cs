using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WendtEquipmentTracking.App.Models
{
    public class HardwareKitGroupModel
    {
        public int HardwareKitId { get; set; }

        [DisplayName("Shipping Tag Number")]
        [ReadOnly(true)]
        public string ShippingTagNumber { get; set; }

        [DisplayName("Description")]
        [ReadOnly(true)]
        public string Description { get; set; }

        [DisplayName("Quantity")]
        [ReadOnly(true)]
        public double Quantity { get; set; }


        [DisplayName("Quantity To Ship")]
        [Required]
        public int QuantityToShip { get; set; }
    }
}