using System;

namespace WendtEquipmentTracking.BusinessLogic.BO
{
    public class EquipmentRevisionBO
    {

        // EXISTING EQUIPMENT FIELDS

        public string EquipmentName { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string DrawingNumber { get; set; }
        public string WorkOrderNumber { get; set; }
        public double Quantity { get; set; }
        public string ShippingTagNumber { get; set; }
        public string Description { get; set; }
        public double? UnitWeight { get; set; }
        public string ShippedQuantity { get; set; }

        // NEW REVISION FIELDS

        public string NewEquipmentName { get; set; }
        public DateTime? NewReleaseDate { get; set; }
        public string NewDrawingNumber { get; set; }
        public string NewWorkOrderNumber { get; set; }
        public double? NewQuantity { get; set; }
        public string NewShippingTagNumber { get; set; }
        public string NewDescription { get; set; }
        public double? NewUnitWeight { get; set; }
        public int Order { get; set; }
    }
}