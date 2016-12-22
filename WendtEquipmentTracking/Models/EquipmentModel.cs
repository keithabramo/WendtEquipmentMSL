using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace WendtEquipmentTracking.App.Models
{
    public class EquipmentModel
    {
        public int EquipmentId { get; set; }
        public int ProjectId { get; set; }


        [DisplayName("Equipment Name")]
        public string EquipmentName { get; set; }


        [DisplayName("Priority")]
        public string Priority { get; set; }


        [DisplayName("Release Date")]
        public DateTime? ReleaseDate { get; set; }


        [DisplayName("Drawing #")]
        public string DrawingNumber { get; set; }


        [DisplayName("Work Order Number")]
        public string WorkOrderNumber { get; set; }


        [DisplayName("Quantity")]
        public double? Quantity { get; set; }


        [DisplayName("Shipping Tag #")]
        public string ShippingTagNumber { get; set; }


        [DisplayName("Description")]
        public string Description { get; set; }


        [DisplayName("Unit Weight")]
        public double? UnitWeight { get; set; }


        [DisplayName("Total Weight")]
        public double? TotalWeight { get; set; }


        [DisplayName("Total Weight Shipped")]
        public double? TotalWeightShipped { get; set; }


        [DisplayName("Ready To Ship")]
        public double? ReadyToShip { get; set; }


        [DisplayName("Shipped Quantity")]
        public double? ShippedQuantity { get; set; }


        [DisplayName("Left To Ship")]
        public double? LeftToShip { get; set; }


        [DisplayName("Fully Shipped")]
        public bool? FullyShipped { get; set; }


        [DisplayName("Date Shipped")]
        public DateTime? DateShipped { get; set; }


        [DisplayName("")]
        public string ShippedFrom { get; set; }


        [DisplayName("Customs Value")]
        public double? CustomsValue { get; set; }


        [DisplayName("Sale Price")]
        public double? SalePrice { get; set; }


        [DisplayName("HTS Code")]
        public string HTSCode { get; set; }


        [DisplayName("Country Of Origin")]
        public string CountryOfOrigin { get; set; }


        [DisplayName("Notes")]
        public string Notes { get; set; }


        [DisplayName("Auto Ship File")]
        public string AutoShipFile { get; set; }

        [DisplayName("Sales Order Number")]
        public string SalesOrderNumber { get; set; }


        [DisplayName("Qty To Storage")]
        public double? QtyToStorage { get; set; }


        [DisplayName("BOL Number To Storage")]
        public string BillOfLandingNumberToStorage { get; set; }


        [DisplayName("Date Shipped To Storage")]
        public DateTime? DateShippedToStorage { get; set; }

        public IEnumerable<BillOfLandingModel> BillOfLandings { get; set; }
        public ProjectModel Project { get; set; }
    }
}