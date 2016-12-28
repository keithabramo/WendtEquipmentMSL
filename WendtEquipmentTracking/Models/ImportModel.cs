using System.Collections.Generic;
using System.ComponentModel;
using System.Web;
using System.Web.Mvc;

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

        public string FileName { get; set; }

        [DisplayName("Status")]
        public ImportStatus Status { get; set; }

        public List<ImportSheetModel> Sheets { get; set; }

        public List<EquipmentImportModel> Equipments { get; set; }
    }
}