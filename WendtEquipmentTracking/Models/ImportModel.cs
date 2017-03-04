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

        public string FilePath { get; set; }

        public string Equipment { get; set; }
        public int Priority { get; set; }
        public string DrawingNumber { get; set; }
        public string WorkOrderNumber { get; set; }
        public int QuantityMultiplier { get; set; }

        public IEnumerable<int> Priorities { get; set; }
    }
}