﻿namespace WendtEquipmentTracking.DataAccess.FileManagement.Domain
{
    public class WorkOrderPriceRow
    {
        public string WorkOrderNumber { get; set; }
        public double CostPrice { get; set; }
        public double SalePrice { get; set; }
        public double ReleasedPercent { get; set; }
        public double ShippedPercent { get; set; }

    }
}
