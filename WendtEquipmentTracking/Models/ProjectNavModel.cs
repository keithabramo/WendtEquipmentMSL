using System.Collections.Generic;

namespace WendtEquipmentTracking.App.Models
{
    public class ProjectNavModel : BaseModel
    {
        public ProjectModel CurrentProject { get; set; }
        public IEnumerable<ProjectModel> Projects { get; set; }
    }
}