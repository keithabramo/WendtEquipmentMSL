using System;

namespace WendtEquipmentTracking.BusinessLogic.BO
{
    public class PriorityBO
    {
        public int PriorityId { get; set; }
        public int ProjectId { get; set; }
        public int PriorityNumber { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? ContractualShipDate { get; set; }
        public string EquipmentName { get; set; }
    }
}
