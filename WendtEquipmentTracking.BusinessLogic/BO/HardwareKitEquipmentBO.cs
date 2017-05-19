namespace WendtEquipmentTracking.BusinessLogic.BO
{
    public class HardwareKitEquipmentBO
    {
        public int HardwareKitEquipmentId { get; set; }
        public int HardwareKitId { get; set; }
        public int EquipmentId { get; set; }
        public double QuantityToShip { get; set; }

        public HardwareKitBO HardwareKit { get; set; }
        public EquipmentBO Equipment { get; set; }
    }
}