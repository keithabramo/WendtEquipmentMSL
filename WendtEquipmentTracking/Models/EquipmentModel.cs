using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace WendtEquipmentTracking.App.Models
{
    public class EquipmentModel : BaseModel
    {
        public int EquipmentId { get; set; }
        public int ProjectId { get; set; }
        public int? HardwareKitId { get; set; }


        [DisplayName("Equipment")]
        public string EquipmentName { get; set; }

        [DisplayName("Is Hardware?")]
        public bool IsHardware { get; set; }

        [DisplayName("Priority")]
        public string Priority { get; set; }


        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [DisplayName("Release Date")]
        public DateTime? ReleaseDate { get; set; }


        [DisplayName("Drawing #")]
        public string DrawingNumber { get; set; }


        [DisplayName("Work Order No.")]
        public string WorkOrderNumber { get; set; }


        [DisplayName("Qty")]
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


        [DisplayName("Shipped Qty")]
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

        public IList<BillOfLadingEquipmentModel> BillOfLadingEquipments { get; set; }

        public EquipmentIndicatorsModel EquipmentIndicators { get; set; }

        public string ProjectNumber { get; set; }

        public void SetEquipmentIndicators()
        {
            EquipmentIndicators = new EquipmentIndicatorsModel();

            //unit weight
            if (UnitWeight == null || UnitWeight <= 0)
            {
                EquipmentIndicators.UnitWeightColor = EquipmentIndicatorsModel.Colors.Red;
            }

            //ready to ship does not have a clean way to check for red
            if (ReadyToShip > 0)
            {
                if (BillOfLadingEquipments.Any(be => be.BillOfLading.ToStorage && be.BillOfLading.IsCurrentRevision))
                {
                    EquipmentIndicators.ReadyToShipColor = EquipmentIndicatorsModel.Colors.Green;
                }
                else
                {
                    EquipmentIndicators.ReadyToShipColor = EquipmentIndicatorsModel.Colors.Yellow;
                }
            }

            //ship qty
            if (ShippedQuantity > Quantity)
            {
                EquipmentIndicators.ShippedQtyColor = EquipmentIndicatorsModel.Colors.Red;
            }

            //left to ship
            if (ShippedQuantity > Quantity)
            {
                EquipmentIndicators.LeftToShipColor = EquipmentIndicatorsModel.Colors.Red;
            }

            //fully shipped
            if (ShippedQuantity > Quantity)
            {
                EquipmentIndicators.FullyShippedColor = EquipmentIndicatorsModel.Colors.Fuchsia;
            }
            else if (FullyShipped.HasValue && FullyShipped.Value == false)
            {
                EquipmentIndicators.FullyShippedColor = EquipmentIndicatorsModel.Colors.Pink;
            }
            else if (!FullyShipped.HasValue || FullyShipped.Value == true)
            {
                EquipmentIndicators.FullyShippedColor = EquipmentIndicatorsModel.Colors.Purple;
            }

            //customs value, needs weights calculations

            //sales price, needs weights calculations

            //country of origin
            if (string.IsNullOrEmpty(CountryOfOrigin) && !string.IsNullOrEmpty(HTSCode))
            {
                EquipmentIndicators.CountyOfOriginColor = EquipmentIndicatorsModel.Colors.Red;
            }

            //sales order
            if (!WorkOrderNumber.Contains(ProjectNumber))
            {
                EquipmentIndicators.SalesOrderNumberColor = EquipmentIndicatorsModel.Colors.Red;
            }
        }
    }
}