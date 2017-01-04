using System;
using System.Collections.Generic;

namespace WendtEquipmentTracking.BusinessLogic.BO
{
    public class ProjectBO
    {
        public int ProjectId { get; set; }
        public string ProjectNumber { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public IEnumerable<EquipmentBO> Equipments { get; set; }
        public IEnumerable<BillOfLandingBO> BillOfLandings { get; set; }
        public IEnumerable<HardwareKitBO> HardwareKits { get; set; }
    }
}