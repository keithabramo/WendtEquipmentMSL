﻿using System.Collections.Generic;

namespace WendtEquipmentTracking.BusinessLogic.BO
{
    public class HardwareKitBO
    {
        public int HardwareKitId { get; set; }
        public int ProjectId { get; set; }
        public int Revision { get; set; }
        public bool IsCurrentRevision { get; set; }
        public string HardwareKitNumber { get; set; }
        public double ExtraQuantityPercentage { get; set; }

        public IEnumerable<HardwareKitEquipmentBO> HardwareKitEquipments { get; set; }
    }
}