using System.ComponentModel;
using WendtEquipmentTracking.App.Common;

namespace WendtEquipmentTracking.App.Models
{
    public class BaseModel
    {
        
        [DisplayName("Status")]
        public SuccessStatus Status { get; set; }
    }
}