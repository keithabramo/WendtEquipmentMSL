using Foolproof;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WendtEquipmentTracking.App.Models
{
    public class HardwareKitGroupModel
    {
        public int HardwareKitId { get; set; }

        public bool Checked { get; set; }

        [DisplayName("Shipping Tag Number")]
        public string ShippingTagNumber { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("Quantity")]
        public double Quantity { get; set; }


        [DisplayName("Quantity To Ship")]
        [RequiredIfTrue("Checked")]
        public int QuantityToShip { get; set; }
    }
}