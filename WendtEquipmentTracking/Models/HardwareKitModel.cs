using System.ComponentModel;

namespace WendtEquipmentTracking.App.Models
{
    public class HardwareKitModel
    {
        public int HardwareKitId { get; set; }
        public int ProjectId { get; set; }

        [DisplayName("Revision")]
        public int Revision { get; set; }

        public ProjectModel Project { get; set; }
    }
}