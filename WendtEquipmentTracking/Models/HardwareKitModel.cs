using System.ComponentModel;

namespace WendtEquipmentTracking.App.Models
{
    public class HardwareKitModel : BaseModel
    {
        public int HardwareKitId { get; set; }
        public int ProjectId { get; set; }

        [DisplayName("Is Current Revision")]
        public bool IsCurrentRevision { get; set; }

        [DisplayName("Revision")]
        public int Revision { get; set; }
    }
}