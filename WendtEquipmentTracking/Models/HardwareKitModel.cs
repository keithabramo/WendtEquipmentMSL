using System;
using System.Collections.Generic;
using System.Linq;
using WendtEquipmentTracking.Common;

namespace WendtEquipmentTracking.App.Models
{
    public class HardwareKitModel
    {
        public int HardwareKitId { get; set; }
        public int ProjectId { get; set; }
        public int Revision { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public ProjectModel Project { get; set; }
    }
}