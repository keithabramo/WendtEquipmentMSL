namespace WendtEquipmentTracking.App.Models
{
    public class EquipmentAttachmentModel
    {
        public int EquipmentAttachmentId { get; set; }
        public int EquipmentId { get; set; }
        public string FileName { get; set; }
        public string FileTitle { get; set; }
    }
}