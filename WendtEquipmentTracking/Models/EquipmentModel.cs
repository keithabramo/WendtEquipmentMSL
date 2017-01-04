using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WendtEquipmentTracking.App.Models
{
    public class EquipmentModel : BaseModel
    {
        public int EquipmentId { get; set; }
        public int ProjectId { get; set; }


        [DisplayName("Equipment Name")]
        public string EquipmentName { get; set; }


        [DisplayName("Priority")]
        public string Priority { get; set; }


        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
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
        [ReadOnly(true)]
        public double? TotalWeight { get; set; }


        [DisplayName("Total Weight Shipped")]
        [ReadOnly(true)]
        public double? TotalWeightShipped { get; set; }


        [DisplayName("Ready To Ship")]
        public double? ReadyToShip { get; set; }


        [DisplayName("Shipped Quantity")]
        [ReadOnly(true)]
        public double? ShippedQuantity { get; set; }


        [DisplayName("Left To Ship")]
        [ReadOnly(true)]
        public double? LeftToShip { get; set; }


        [DisplayName("Fully Shipped")]
        [ReadOnly(true)]
        public bool? FullyShipped { get; set; }


        [DisplayName("Customs Value")]
        [ReadOnly(true)]
        public double? CustomsValue { get; set; }


        [DisplayName("Sale Price")]
        [ReadOnly(true)]
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


        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [DisplayName("Date Shipped To Storage")]
        public DateTime? DateShippedToStorage { get; set; }

        public IList<BillOfLandingEquipmentModel> BillOfLandingEquipments { get; set; }
    }
}