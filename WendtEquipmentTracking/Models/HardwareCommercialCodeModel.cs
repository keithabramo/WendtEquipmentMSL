using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WendtEquipmentTracking.App.Models
{
    public class HardwareCommercialCodeModel
    {
        public int HardwareCommercialCodeId { get; set; }

        [DisplayName("Part #")]
        [Required]
        public string PartNumber { get; set; }

        [DisplayName("Description")]
        [Required]
        public string Description { get; set; }

        [DisplayName("Commodity Code")]
        [Required]
        public string CommodityCode { get; set; }

        public bool IsDuplicate { get; set; }

    }
}