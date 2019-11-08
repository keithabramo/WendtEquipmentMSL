using System;
using System.Collections.Generic;

namespace WendtEquipmentTracking.BusinessLogic.BO
{
    public class EquipmentBO
    {
        public int EquipmentId { get; set; }
        public int ProjectId { get; set; }
        public string EquipmentName { get; set; }
        public bool IsHardware { get; set; }
        public int? PriorityId { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string DrawingNumber { get; set; }
        public string WorkOrderNumber { get; set; }
        public double? Quantity { get; set; }
        public string ShippingTagNumber { get; set; }
        public string Description { get; set; }
        public double? UnitWeight { get; set; }
        public double? TotalWeight { get; set; }
        public double? TotalWeightShipped { get; set; }
        public double ReadyToShip { get; set; }
        public double? ShippedQuantity { get; set; }
        public double? LeftToShip { get; set; }
        public bool FullyShipped { get; set; }
        public double? CustomsValue { get; set; }
        public double? SalePrice { get; set; }
        public string Notes { get; set; }
        public string ShippedFrom { get; set; }
        public string HTSCode { get; set; }
        public string CountryOfOrigin { get; set; }
        public int Revision { get; set; }

        //calculated properties
        public bool HasBillOfLading { get; set; }
        public bool HasBillOfLadingInStorage { get; set; }
        public bool IsHardwareKit { get; set; }
        public bool IsAssociatedToHardwareKit { get; set; }
        public string AssociatedHardwareKitNumber { get; set; }
        public int AttachmentCount { get; set; }

        //used to order items correctly during import
        public int Order { get; set; }
        public int? PriorityNumber { get; set; }


        public PriorityBO Priority { get; set; }
        public IEnumerable<BillOfLadingEquipmentBO> BillOfLadingEquipments { get; set; }
    }
}