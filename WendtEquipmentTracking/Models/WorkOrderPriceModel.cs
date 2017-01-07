namespace WendtEquipmentTracking.App.Models
{
    public class WorkOrderPriceModel
    {
        public int WorkOrderPriceId { get; set; }
        public int ProjectId { get; set; }
        public int WorkOrderNumber { get; set; }
        public double Price { get; set; }

        public ProjectModel Project { get; set; }
    }
}