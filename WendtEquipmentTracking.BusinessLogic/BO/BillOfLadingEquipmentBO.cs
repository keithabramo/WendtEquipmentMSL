namespace WendtEquipmentTracking.BusinessLogic.BO
{
    public class BillOfLadingEquipmentBO
    {
        public int BillOfLadingEquipmentId { get; set; }
        public int BillOfLadingId { get; set; }
        public int EquipmentId { get; set; }
        public double Quantity { get; set; }

        public BillOfLadingBO BillOfLading { get; set; }
        public EquipmentBO Equipment { get; set; }
    }
}