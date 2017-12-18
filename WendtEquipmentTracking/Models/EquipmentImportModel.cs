using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WendtEquipmentTracking.App.Models
{
    public class EquipmentImportModel : BaseModel
    {
        //contains drawing number and temp file path joined with a + and split into a dictionary
        public List<string> FilePaths { get; set; }

        //used to post back to client and join toghether in hidden input
        public string FilePath { get; set; }
        public string DrawingNumber { get; set; }

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