using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace WendtEquipmentTracking.App.Models
{
    public class WorkOrderPriceModel
    {
        public int WorkOrderPriceId { get; set; }
        public int ProjectId { get; set; }

        [DisplayName("Work Order Number")]
        [Remote("ValidWorkOrderNumber", "Validate", AdditionalFields = "WorkOrderPriceId", ErrorMessage = "This work order price number already exists")]
        [Required]
        public string WorkOrderNumber { get; set; }


        [DataType(DataType.Currency)]
        [DisplayName("Sale Price")]
        [Required]
        public double SalePrice { get; set; }


        [DataType(DataType.Currency)]
        [DisplayName("Sales+Soft Costs")]
        [Required]
        public double CostPrice { get; set; }


        [DisplayName("Released %")]
        public double ReleasedPercent { get; set; }


        [DisplayName("Shipped %")]
        public double ShippedPercent { get; set; }
    }
}