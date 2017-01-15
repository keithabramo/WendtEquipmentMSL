using System.Collections.Generic;
using System.ComponentModel;
using System.Web;

namespace WendtEquipmentTracking.App.Models
{
    public class ImportModel : BaseModel
    {

        [DisplayName("File")]
        //[Remote("ValidImportFile", "Validate")]
        public HttpPostedFileBase File { get; set; }

        public string FileName { get; set; }

        public List<ImportSheetModel> Sheets { get; set; }
    }
}