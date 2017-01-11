﻿namespace WendtEquipmentTracking.App.Models
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

        public Colors UnitWeightColor { get; set; }
        public Colors ReadyToShipColor { get; set; }
        public Colors ShippedQtyColor { get; set; }
        public Colors LeftToShipColor { get; set; }
        public Colors FullyShippedColor { get; set; }
        public Colors CustomsValueColor { get; set; }
        public Colors SalePriceColor { get; set; }
        public Colors CountyOfOriginColor { get; set; }
        public Colors SalesOrderNumberColor { get; set; }
        public Colors BillOfLadingNumberColor { get; set; }
        public Colors DateShippedNumberColor { get; set; }
        public Colors ShippedFromColor { get; set; }
    }
}