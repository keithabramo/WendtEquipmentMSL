using System.Collections.Generic;

namespace WendtEquipmentTracking.App.Models
{
    public class EquipmentImportModel : ImportModel
    {
        public string FilePath { get; set; }

        public string Equipment { get; set; }
        public int Priority { get; set; }
        public string DrawingNumber { get; set; }
        public string WorkOrderNumber { get; set; }
        public int QuantityMultiplier { get; set; }

        public IEnumerable<int> Priorities { get; set; }
    }
}