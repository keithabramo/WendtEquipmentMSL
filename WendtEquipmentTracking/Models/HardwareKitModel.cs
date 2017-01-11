using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

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

        [DisplayName("Hardware Kit Number")]
        [Required]
        public string HardwareKitNumber { get; set; }

        [DisplayName("Extra Quantity %")]
        [Required]
        public double ExtraQuantityPercentage { get; set; }

        public IList<HardwareKitGroupModel> HardwareGroups { get; set; }
    }
}