using Foolproof;
using System.ComponentModel;

namespace WendtEquipmentTracking.App.Models
{
    public class HardwareKitGroupModel
    {
        public int HardwareKitId { get; set; }

        public bool Checked { get; set; }

        [DisplayName("Ship Tag #")]
        public string ShippingTagNumber { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("Qty")]
        public double Quantity { get; set; }


        [DisplayName("Qty to Ship")]
        [RequiredIfTrue("Checked")]
        public int QuantityToShip { get; set; }
    }
}