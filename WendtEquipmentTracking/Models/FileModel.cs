using System.ComponentModel;
using System.Web;

namespace WendtEquipmentTracking.App.Models
{
    public class FileModel : BaseModel
    {

        [DisplayName("File")]
        public HttpPostedFileBase File { get; set; }

    }
}