namespace WendtEquipmentTracking.App.Models
{
    public class BillOfLadingAttachmentModel
    {
        public int BillOfLadingAttachmentId { get; set; }
        public int BillOfLadingId { get; set; }
        public string FileName { get; set; }
        public string FileTitle { get; set; }
    }
}