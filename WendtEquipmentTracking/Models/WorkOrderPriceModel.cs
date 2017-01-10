using System.ComponentModel.DataAnnotations;

namespace WendtEquipmentTracking.App.Models
{
    public class WorkOrderPriceModel
    {
        public int WorkOrderPriceId { get; set; }
        public int ProjectId { get; set; }
        [Required]
        public string WorkOrderNumber { get; set; }
        [Required]
        public double SalePrice { get; set; }
        [Required]
        public double CostPrice { get; set; }

        public ProjectModel Project { get; set; }
    }
}