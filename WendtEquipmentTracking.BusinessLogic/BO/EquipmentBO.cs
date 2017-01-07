using System;
using System.Collections.Generic;

namespace WendtEquipmentTracking.BusinessLogic.BO
{
    public class EquipmentBO
    {
        public int EquipmentId { get; set; }
        public int ProjectId { get; set; }
        public int? HardwareKitId { get; set; }
        public string EquipmentName { get; set; }
        public bool IsHardware { get; set; }
        public string Priority { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string DrawingNumber { get; set; }
        public string WorkOrderNumber { get; set; }
        public double? Quantity { get; set; }
        public string ShippingTagNumber { get; set; }
        public string Description { get; set; }
        public double? UnitWeight { get; set; }
        public double? TotalWeight { get; set; }
        public double? TotalWeightShipped { get; set; }
        public double? ReadyToShip { get; set; }
        public double? ShippedQuantity { get; set; }
        public double? LeftToShip { get; set; }
        public bool? FullyShipped { get; set; }
        public double? CustomsValue { get; set; }
        public double? SalePrice { get; set; }
        public string HTSCode { get; set; }
        public string CountryOfOrigin { get; set; }
        public string Notes { get; set; }
        public string AutoShipFile { get; set; }
        public string SalesOrderNumber { get; set; }

        public IEnumerable<BillOfLadingEquipmentBO> BillOfLadingEquipments { get; set; }
        public IEnumerable<HardwareKitEquipmentBO> HardwareKitEquipments { get; set; }
    }
}