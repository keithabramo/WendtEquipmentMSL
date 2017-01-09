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
        public double Price { get; set; }

        public ProjectModel Project { get; set; }
    }
}