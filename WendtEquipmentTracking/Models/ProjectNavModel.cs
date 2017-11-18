using System.Collections.Generic;
using System.Web.Mvc;

namespace WendtEquipmentTracking.App.Models
{
    public class ProjectNavModel : BaseModel
    {
        public ProjectModel CurrentProject { get; set; }
        public IEnumerable<SelectListItem> Projects { get; set; }
    }
}