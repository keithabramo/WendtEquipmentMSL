namespace WendtEquipmentTracking.BusinessLogic.BO
{
    public class EquipmentImportBO
    {
        public string FilePath { get; set; }

        public string Equipment { get; set; }
        public int Priority { get; set; }
        public string DrawingNumber { get; set; }
        public string WorkOrderNumber { get; set; }
        public int QuantityMultiplier { get; set; }

    }
}