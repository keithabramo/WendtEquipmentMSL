namespace WendtEquipmentTracking.Common.DTO
{
    public class EmailDTO
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public AttachmentDTO Attachment { get; set; }
    }
}
