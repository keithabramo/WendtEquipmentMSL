using System.Collections.Generic;

namespace WendtEquipmentTracking.DataAccess.FileManagement.Domain
{
    public class EquipmentRevisionImport
    {
        public IDictionary<string, string> FilePaths { get; set; }

        public int? PriorityId { get; set; }
        public string WorkOrderNumber { get; set; }

        public IEnumerable<HardwareCommercialCodeRow> hardwareCommercialCodes { get; set; }
    }
}