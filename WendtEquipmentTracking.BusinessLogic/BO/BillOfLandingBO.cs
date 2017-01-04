using System;
using System.Collections.Generic;

namespace WendtEquipmentTracking.BusinessLogic.BO
{
    public class BillOfLandingBO
    {
        public int BillOfLandingId { get; set; }
        public int ProjectId { get; set; }
        public int Revision { get; set; }
        public bool IsCurrentRevision { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string BillOfLandingNumber { get; set; }
        public DateTime? DateShipped { get; set; }
        public string ShippedFrom { get; set; }
        public string ShipToCompany { get; set; }
        public string ShipToAddress { get; set; }
        public string ShipToCSZ { get; set; }
        public string ShipToPhoneFax { get; set; }
        public string ShipToContact1 { get; set; }
        public string ShipToContact2 { get; set; }

        public IEnumerable<BillOfLandingEquipmentBO> BillOfLandingEquipments { get; set; }
    }
}