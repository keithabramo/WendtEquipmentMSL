using System.ComponentModel;
using System.Web;

namespace WendtEquipmentTracking.App.Models
{
    public class EquipmentAttachmentFileModel : FileModel
    {
        public int EquipmentId { get; set; }
    }
}