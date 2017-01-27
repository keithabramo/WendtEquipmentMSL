namespace WendtEquipmentTracking.App.Models
{
    public class IndicatorsModel : BaseModel
    {
        public enum Colors
        {
            White,
            Red,
            Yellow,
            Green,
            Purple,
            Pink,
            Fuchsia
        }

        public string UnitWeightColor { get; set; }
        public string ReadyToShipColor { get; set; }
        public string ShippedQtyColor { get; set; }
        public string LeftToShipColor { get; set; }
        public string FullyShippedColor { get; set; }
        public string CustomsValueColor { get; set; }
        public string SalePriceColor { get; set; }
        public string CountyOfOriginColor { get; set; }
        public string SalesOrderNumberColor { get; set; }
        public string BillOfLadingNumberColor { get; set; }
        public string DateShippedNumberColor { get; set; }
        public string ShippedFromColor { get; set; }
    }
}