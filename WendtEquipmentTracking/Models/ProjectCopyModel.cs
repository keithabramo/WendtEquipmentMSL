using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;

namespace WendtEquipmentTracking.App.Models
{
    public class ProjectCopyModel : BaseModel
    {
        [DisplayName("Copy From")]
        public IEnumerable<SelectListItem> Projects { get; set; }
    }
}