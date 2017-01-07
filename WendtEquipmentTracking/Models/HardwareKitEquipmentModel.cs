using System.ComponentModel;

namespace WendtEquipmentTracking.App.Models
{
    public class HardwareKitEquipmentModel
    {
        public int HardwareKitEquipmentId { get; set; }
        public int HardwareKitId { get; set; }
        public int EquipmentId { get; set; }

        [DisplayName("Total Quantity")]
        public double Quantity { get; set; }

        public HardwareKitModel HardwareKit { get; set; }
        public EquipmentModel Equipment { get; set; }
    }
}