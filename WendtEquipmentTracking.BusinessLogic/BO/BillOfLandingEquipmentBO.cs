namespace WendtEquipmentTracking.BusinessLogic.BO
{
    public class BillOfLandingEquipmentBO
    {
        public int BillOfLandingEquipmentId { get; set; }
        public int BillOfLandingId { get; set; }
        public int EquipmentId { get; set; }
        public double Quantity { get; set; }

        public BillOfLandingBO BillOfLanding { get; set; }
        public EquipmentBO Equipment { get; set; }
    }
}