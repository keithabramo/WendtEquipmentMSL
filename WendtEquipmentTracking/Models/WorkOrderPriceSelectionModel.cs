using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace WendtEquipmentTracking.App.Models
{
    public class WorkOrderPriceSelectionModel : WorkOrderPriceModel
    {
        public bool Checked { get; set; }

        [DisplayName("Work Order Number")]
        [Remote("ValidWorkOrderNumberImport", "Validate", AdditionalFields = "Checked", ErrorMessage = "This work order price number already exists")]
        [Required]
        public new string WorkOrderNumber { get; set; }
    }
}