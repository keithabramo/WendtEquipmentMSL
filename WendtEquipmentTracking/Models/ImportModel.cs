using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WendtEquipmentTracking.Common;

namespace WendtEquipmentTracking.App.Models
{
    public class ImportModel
    {
        public enum ImportStatus
        {
            None,
            Success,
            Error 
        }

        [DisplayName("File")]
        [Remote("ValidImportFile", "Validate")]
        public HttpPostedFileBase File { get; set; }

        [DisplayName("Status")]
        public ImportStatus Status { get; set; }

        public IEnumerable<string> Sheets { get; set; }
        public IEnumerable<ProjectModel> Projects { get; set; }
    }
}