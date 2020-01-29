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
    }
}