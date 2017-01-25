namespace WendtEquipmentTracking.BusinessLogic.BO
{
    public class WorkOrderPriceBO
    {
        public int WorkOrderPriceId { get; set; }
        public int ProjectId { get; set; }
        public string WorkOrderNumber { get; set; }
        public double CostPrice { get; set; }
        public double SalePrice { get; set; }
    }
}