using System.ComponentModel;

namespace WendtEquipmentTracking.App.Models
{
    public class HardwareCommercialCodeModel
    {
        public int HardwareCommercialCodeId { get; set; }

        [DisplayName("Part #")]
        public string PartNumber { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("Commodity Code")]
        public string CommodityCode { get; set; }
    }
}