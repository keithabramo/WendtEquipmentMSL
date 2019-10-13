namespace WendtEquipmentTracking.DataAccess.FileManagement.Domain
{
    public class EquipmentRow
    {
        public string DrawingNumber { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public string PartNumber { get; set; }
        public double UnitWeight { get; set; }

    }
}
