namespace WendtEquipmentTracking.BusinessLogic.BO
{
    public class BillOfLadingAttachmentBO
    {
        public int BillOfLadingAttachmentId { get; set; }
        public int BillOfLadingId { get; set; }
        public string FileName { get; set; }
        public string FileTitle { get; set; }
    }
}