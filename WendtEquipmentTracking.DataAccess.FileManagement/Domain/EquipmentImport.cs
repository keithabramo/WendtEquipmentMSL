using System.Collections.Generic;

namespace WendtEquipmentTracking.DataAccess.FileManagement.Domain
{
    public class EquipmentImport
    {
        public string FilePath { get; set; }

        public string Equipment { get; set; }
        public int Priority { get; set; }
        public string DrawingNumber { get; set; }
        public string WorkOrderNumber { get; set; }
        public int QuantityMultiplier { get; set; }

        public IEnumerable<HardwareCommercialCodeImport> hardwareCommercialCodes { get; set; }
    }
}