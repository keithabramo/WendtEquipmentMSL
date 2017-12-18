using System.Collections.Generic;

namespace WendtEquipmentTracking.BusinessLogic.BO
{
    public class EquipmentImportBO
    {
        public IDictionary<string, string> FilePaths { get; set; }

        public string Equipment { get; set; }
        public int? PriorityId { get; set; }
        //public string DrawingNumber { get; set; }
        public string WorkOrderNumber { get; set; }
        public int QuantityMultiplier { get; set; }

    }
}