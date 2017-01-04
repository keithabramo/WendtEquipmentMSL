namespace WendtEquipmentTracking.App.Models
{
    public class BillOfLandingEquipmentModel
    {
        public int BillOfLandingEquipmentId { get; set; }
        public int BillOfLandingId { get; set; }
        public int EquipmentId { get; set; }
        public double Quantity { get; set; }

        public bool Checked { get; set; }

        public BillOfLandingModel BillOfLanding { get; set; }
        public EquipmentModel Equipment { get; set; }
    }
}