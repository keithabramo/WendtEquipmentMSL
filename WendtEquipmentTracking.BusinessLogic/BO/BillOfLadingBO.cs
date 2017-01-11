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
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string BillOfLadingNumber { get; set; }
        public DateTime? DateShipped { get; set; }
        public string ShippedFrom { get; set; }
        public bool ToStorage { get; set; }

        public IEnumerable<BillOfLadingEquipmentBO> BillOfLadingEquipments { get; set; }
    }
}