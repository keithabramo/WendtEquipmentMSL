using System;

namespace WendtEquipmentTracking.BusinessLogic.BO
{
    public class TruckingScheduleBO
    {
        public int TruckingScheduleId { get; set; }
        public DateTime? RequestDate { get; set; }
        public int? ProjectId { get; set; }
        public string WorkOrder { get; set; }
        public string PurchaseOrder { get; set; }
        public string RequestedBy { get; set; }
        public int? ShipFromVendorId { get; set; }
        public int? ShipToVendorId { get; set; }
        public string Description { get; set; }
        public double? NumPieces { get; set; }
        public string Dimensions { get; set; }
        public double? Weight { get; set; }
        public string Carrier { get; set; }
        public DateTime? PickUpDate { get; set; }
        public string Comments { get; set; }
        public string Status { get; set; }

        public ProjectBO Project { get; set; }
        public VendorBO ShipFromVendor { get; set; }
        public VendorBO ShipToVendor { get; set; }
    }
}