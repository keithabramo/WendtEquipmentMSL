namespace WendtEquipmentTracking.BusinessLogic.BO
{
    public class EquipmentRevisionImportBO
    {
        public string FilePath { get; set; }

        public string Equipment { get; set; }
        public int? PriorityId { get; set; }
        public string DrawingNumber { get; set; }
        public string WorkOrderNumber { get; set; }
        public int QuantityMultiplier { get; set; }
        public int Revision { get; set; }

    }
}