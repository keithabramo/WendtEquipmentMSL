using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WendtEquipmentTracking.App.Models
{
    public class HardwareKitEquipmentModel
    {
        public int HardwareKitEquipmentId { get; set; }
        public int HardwareKitId { get; set; }
        public int EquipmentId { get; set; }

        [DisplayName("Total Quantity")]
        [Required]
        public double Quantity { get; set; }

        public HardwareKitModel HardwareKit { get; set; }
        public EquipmentModel Equipment { get; set; }
    }
}