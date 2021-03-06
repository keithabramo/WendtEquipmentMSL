﻿using System;

namespace WendtEquipmentTracking.DataAccess.FileManagement.Domain
{
    public class RawEquipmentRow
    {
        public string EquipmentName { get; set; }
        public int? PriorityNumber { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string DrawingNumber { get; set; }
        public string WorkOrderNumber { get; set; }
        public int Quantity { get; set; }
        public string ShippingTagNumber { get; set; }
        public string Description { get; set; }
        public double UnitWeight { get; set; }
        public double ReadyToShip { get; set; }
        public string Notes { get; set; }
        public string ShippedFrom { get; set; }
        public string HTSCode { get; set; }
        public string CountryOfOrigin { get; set; }



    }
}
