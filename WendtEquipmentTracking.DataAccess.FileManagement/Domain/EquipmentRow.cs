using System;

namespace WendtEquipmentTracking.DataAccess.FileManagement.Domain
{
    public class EquipmentRow
    {
        public string EquipmentName { get; set; }
        public int? PriorityId { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string DrawingNumber { get; set; }
        public string WorkOrderNumber { get; set; }
        public int Quantity { get; set; }
        public string ShippingTagNumber { get; set; }
        public string Description { get; set; }
        public double UnitWeight { get; set; }

    }
}
