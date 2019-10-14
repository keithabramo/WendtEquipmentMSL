namespace WendtEquipmentTracking.App.Models
{
    public class EquipmentIndicatorsModel : BaseModel
    {
        public string EquipmentNameColor { get; set; }
        public string UnitWeightColor { get; set; }
        public string ReadyToShipColor { get; set; }
        public string ShippedQtyColor { get; set; }
        public string LeftToShipColor { get; set; }
        public string FullyShippedColor { get; set; }
        public string CustomsValueColor { get; set; }
        public string SalePriceColor { get; set; }
        public string WorkOrderNumberColor { get; set; }
        public string PriorityColor { get; set; }
        public string DrawingNumberColor { get; set; }
        public string ShippingTagNumberColor { get; set; }
    }
}