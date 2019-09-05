using System;

namespace WendtEquipmentTracking.DataAccess.FileManagement.Domain
{
    public class PriorityRow
    {
        public int PriorityNumber { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? ContractualShipDate { get; set; }
        public string EquipmentName { get; set; }

    }
}
