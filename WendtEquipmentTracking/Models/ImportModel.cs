using System.ComponentModel;
using System.Web;

namespace WendtEquipmentTracking.App.Models
{
    public class ImportModel : BaseModel
    {

        [DisplayName("File")]
        public HttpPostedFileBase File { get; set; }

    }
}