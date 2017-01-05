namespace WendtEquipmentTracking.App.Models
{
    public class BillOfLadingEquipmentModel
    {
        public int BillOfLadingEquipmentId { get; set; }
        public int BillOfLadingId { get; set; }
        public int EquipmentId { get; set; }
        public double Quantity { get; set; }

        public BillOfLadingModel BillOfLading { get; set; }
        public EquipmentModel Equipment { get; set; }
    }
}