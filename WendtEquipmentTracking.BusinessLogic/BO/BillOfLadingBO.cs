using System;
using System.Collections.Generic;

namespace WendtEquipmentTracking.BusinessLogic.BO
{
    public class BillOfLadingBO
    {
        public int BillOfLadingId { get; set; }
        public int ProjectId { get; set; }
        public int Revision { get; set; }
        public bool IsCurrentRevision { get; set; }
        public string BillOfLadingNumber { get; set; }
        public DateTime? DateShipped { get; set; }
        public string FreightTerms { get; set; }
        public string Carrier { get; set; }
        public string TrailerNumber { get; set; }
        public bool ToStorage { get; set; }
        public bool IsLocked { get; set; }
        public DateTime? LockedDate { get; set; }
        public string LockedBy { get; set; }

        public IEnumerable<BillOfLadingEquipmentBO> BillOfLadingEquipments { get; set; }
    }
}