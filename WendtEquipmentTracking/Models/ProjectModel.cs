using System.Collections.Generic;
using System.ComponentModel;

namespace WendtEquipmentTracking.App.Models
{
    public class ProjectModel : BaseModel
    {
        public int ProjectId { get; set; }

        [DisplayName("Project Number")]
        public string ProjectNumber { get; set; }

        public IEnumerable<EquipmentModel> Equipments { get; set; }
        public IEnumerable<BillOfLadingModel> BillOfLadings { get; set; }
        public IEnumerable<HardwareKitModel> HardwareKits { get; set; }
    }
}