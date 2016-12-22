using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace WendtEquipmentTracking.App.Models
{
    public class ProjectModel
    {
        public int ProjectId { get; set; }

        [DisplayName("Project Number")]
        public string ProjectNumber { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public IEnumerable<EquipmentModel> Equipments { get; set; }
        public IEnumerable<HardwareKitModel> HardwareKits { get; set; }
    }
}