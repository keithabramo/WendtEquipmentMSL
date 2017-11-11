using System.ComponentModel;

namespace WendtEquipmentTracking.App.Models
{
    public class HardwareKitGroupModel
    {
        public int HardwareKitId { get; set; }

        //only used for editor and datatables to keep unique row id
        public int HardwareKitGroupId { get; set; }


        [DisplayName("Ship Tag #")]
        public string ShippingTagNumber { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("Qty")]
        public double Quantity { get; set; }


        [DisplayName("Qty to Ship")]
        public int QuantityToShip { get; set; }
    }
}