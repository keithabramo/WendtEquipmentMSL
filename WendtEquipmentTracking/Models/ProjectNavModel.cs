using System;
using System.Collections.Generic;
using System.Linq;
using WendtEquipmentTracking.Common;

namespace WendtEquipmentTracking.App.Models
{
    public class ProjectNavModel
    {
        public ProjectModel CurrentProject { get; set; }
        public IEnumerable<ProjectModel> Projects { get; set; }
    }
}