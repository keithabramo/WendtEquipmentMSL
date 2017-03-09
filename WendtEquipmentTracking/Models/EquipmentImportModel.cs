using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WendtEquipmentTracking.App.Models
{
    public class EquipmentImportModel : BaseModel
    {
        public string FilePath { get; set; }

        [DisplayName("Equipment")]
        [Required]
        public string Equipment { get; set; }

        [DisplayName("Priority")]
        [Required]
        public int Priority { get; set; }

        [DisplayName("Drawing #")]
        [Required]
        public string DrawingNumber { get; set; }

        [DisplayName("Work Order #")]
        [Required]
        public string WorkOrderNumber { get; set; }

        [DisplayName("Quantity Multiplier")]
        [Required]
        public int QuantityMultiplier { get; set; }

        public IEnumerable<int> Priorities { get; set; }
    }
}