using System;
using System.Collections.Generic;
using System.Linq;
using WendtEquipmentTracking.Common;

namespace WendtEquipmentTracking.BusinessLogic.BO
{
    public class HardwareKitBO
    {
        public int HardwareKitId { get; set; }
        public int ProjectId { get; set; }
        public int Revision { get; set; }
    }
}