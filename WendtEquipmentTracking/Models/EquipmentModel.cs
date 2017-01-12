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

        [DisplayName("Equipment")]
        [Required]
        public string EquipmentName { get; set; }

        [DisplayName("Is Hardware?")]
        public bool IsHardware { get; set; }

        [DisplayName("Priority")]
        [Required]
        public string Priority { get; set; }


        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [DisplayName("Release Date")]
        [Required]
        public DateTime? ReleaseDate { get; set; }


        [DisplayName("Drawing #")]
        [Required]
        public string DrawingNumber { get; set; }


        [DisplayName("Work Order No.")]
        [Required]
        public string WorkOrderNumber { get; set; }


        [DisplayName("Qty")]
        [Required]
        public double? Quantity { get; set; }


        [DisplayName("Shipping Tag #")]
        [Required]
        public string ShippingTagNumber { get; set; }


        [DisplayName("Description")]
        [Required]
        public string Description { get; set; }


        [DisplayName("Unit Weight")]
        [Required]
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

        public HardwareKitModel HardwareKit { get; set; }

        public IList<HardwareKitEquipmentModel> HardwareKitEquipments { get; set; }


        public HardwareKitEquipmentModel HardwwareKitEquipment
        {
            get
            {
                return HardwareKitEquipments.Where(h => h.HardwareKit.IsCurrentRevision).FirstOrDefault();
            }
        }

        public IndicatorsModel Indicators { get; set; }

        public string ProjectNumber { get; set; }

        public void SetIndicators()
        {
            Indicators = new IndicatorsModel();

            //unit weight
            if (UnitWeight == null || UnitWeight <= 0)
            {
                Indicators.UnitWeightColor = IndicatorsModel.Colors.Red;
            }

            //ready to ship does not have a clean way to check for red
            if (ReadyToShip > 0)
            {
                if (BillOfLadingEquipments.Any(be => be.BillOfLading.ToStorage && be.BillOfLading.IsCurrentRevision))
                {
                    Indicators.ReadyToShipColor = IndicatorsModel.Colors.Green;
                }
                else
                {
                    Indicators.ReadyToShipColor = IndicatorsModel.Colors.Yellow;
                }
            }

            //ship qty
            if (ShippedQuantity > Quantity)
            {
                Indicators.ShippedQtyColor = IndicatorsModel.Colors.Red;
            }

            //left to ship
            if (ShippedQuantity > Quantity)
            {
                Indicators.LeftToShipColor = IndicatorsModel.Colors.Red;
            }

            //fully shipped
            if (ShippedQuantity > Quantity)
            {
                Indicators.FullyShippedColor = IndicatorsModel.Colors.Fuchsia;
            }
            else if (FullyShipped.HasValue && FullyShipped.Value == false)
            {
                Indicators.FullyShippedColor = IndicatorsModel.Colors.Pink;
            }
            else if (!FullyShipped.HasValue || FullyShipped.Value == true)
            {
                Indicators.FullyShippedColor = IndicatorsModel.Colors.Purple;
            }

            //customs value
            if (!CustomsValue.HasValue || CustomsValue.Value <= 0)
            {
                Indicators.CustomsValueColor = IndicatorsModel.Colors.Red;
            }

            //sales price
            if (!SalePrice.HasValue || SalePrice.Value <= 0)
            {
                Indicators.SalePriceColor = IndicatorsModel.Colors.Red;
            }

            //country of origin
            if (string.IsNullOrEmpty(CountryOfOrigin) && !string.IsNullOrEmpty(HTSCode))
            {
                Indicators.CountyOfOriginColor = IndicatorsModel.Colors.Red;
            }

            //sales order
            if (string.IsNullOrEmpty(WorkOrderNumber) || !WorkOrderNumber.Contains(ProjectNumber.Trim()))
            {
                Indicators.SalesOrderNumberColor = IndicatorsModel.Colors.Red;
            }
        }
    }
}