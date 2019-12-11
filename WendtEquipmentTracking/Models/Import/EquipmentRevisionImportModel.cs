using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WendtEquipmentTracking.App.Models
{
    public class EquipmentRevisionImportModel : BaseModel
    {
        public string FilePath { get; set; }

        [DisplayName("Drawing #")]
        [Required]
        public string DrawingNumber { get; set; }

        [DisplayName("Revision")]
        [Required]
        public int Revision { get; set; }

        [DisplayName("Equipment")]
        [Required]
        public string Equipment { get; set; }

        [DisplayName("Prty")]
        public int? PriorityId { get; set; }

        [DisplayName("Work Order #")]
        [Required]
        public string WorkOrderNumber { get; set; }

        [DisplayName("Quantity Multiplier")]
        [Required]
        public int QuantityMultiplier { get; set; }

        public IEnumerable<PriorityModel> Priorities { get; set; }
    }
}