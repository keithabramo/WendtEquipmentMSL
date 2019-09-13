using System.Collections.Generic;

namespace WendtEquipmentTracking.DataAccess.FileManagement.Domain
{
    public class EquipmentImport
    {
        public IDictionary<string, string> FilePaths { get; set; }

        public string Equipment { get; set; }
        public int? PriorityId { get; set; }
        public string WorkOrderNumber { get; set; }
        public int QuantityMultiplier { get; set; }

        public IEnumerable<HardwareCommercialCodeRow> hardwareCommercialCodes { get; set; }
    }
}