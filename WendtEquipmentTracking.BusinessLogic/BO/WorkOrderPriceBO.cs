namespace WendtEquipmentTracking.BusinessLogic.BO
{
    public class WorkOrderPriceBO
    {
        public int WorkOrderPriceId { get; set; }
        public int ProjectId { get; set; }
        public int WorkOrderNumber { get; set; }
        public double Price { get; set; }

        public ProjectBO Project { get; set; }
    }
}