using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace WendtEquipmentTracking.App.Models
{
    public class EquipmentModel : BaseModel
    {
        public EquipmentModel()
        {
            Indicators = new IndicatorsModel();
        }

        public int EquipmentId { get; set; }
        public int ProjectId { get; set; }

        [DisplayName("Equipment")]
        [Required]
        public string EquipmentName { get; set; }

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
        public double Quantity { get; set; }


        [DisplayName("SHIP TAG #")]
        [Required]
        public string ShippingTagNumber { get; set; }


        [DisplayName("Description")]
        [Required]
        public string Description { get; set; }


        [DisplayName("Unit Weight")]
        [DisplayFormat(DataFormatString = "{#.##}")]
        [Required]
        public double? UnitWeight { get; set; }


        [DisplayName("Total Weight")]
        public double? TotalWeight { get; set; }


        [DisplayName("Total Weight Shipped")]
        public double? TotalWeightShipped { get; set; }


        [DisplayName("Ready To Ship")]
        public double? ReadyToShip { get; set; }


        [DisplayName("Shipped Qty")]
        public double? ShippedQuantity { get; set; }


        [DisplayName("Left To Ship")]
        public double? LeftToShip { get; set; }


        [DisplayName("Fully Shipped")]
        public bool? FullyShipped { get; set; }


        [DisplayName("Customs Value")]
        [DataType(DataType.Currency)]
        public double? CustomsValue { get; set; }


        [DisplayName("Sale Price")]
        [DataType(DataType.Currency)]
        public double? SalePrice { get; set; }


        [DisplayName("Notes")]
        public string Notes { get; set; }


        [DisplayName("DWG/PT")]
        public string AutoShipFile { get; set; }

        [DisplayName("Sales Order Number")]
        public string SalesOrderNumber { get; set; }

        [DisplayName("Shipped From")]
        public string ShippedFrom { get; set; }

        [DisplayName("HTS Code")]
        public string HTSCode { get; set; }

        [DisplayName("Country Of Origin")]
        public string CountryOfOrigin { get; set; }







        public bool HasBillOfLading { get; set; }
        public bool IsHardwareKit { get; set; }
        public bool IsAssociatedToHardwareKit { get; set; }
        public string AssociatedHardwareKitNumber { get; set; }
        public bool IsDuplicate { get; set; }

        public IList<BillOfLadingEquipmentModel> BillOfLadingEquipments { get; set; }

        public HardwareKitModel HardwareKit { get; set; }

        public IList<HardwareKitEquipmentModel> HardwareKitEquipments { get; set; }


        public IndicatorsModel Indicators { get; set; }

        public void SetIndicators(string projectNumber, bool isCustomsProject)
        {
            Indicators = new IndicatorsModel();

            //unit weight
            if ((UnitWeight == null || UnitWeight <= 0) && !EquipmentName.Equals("hardware", StringComparison.InvariantCultureIgnoreCase))
            {
                Indicators.UnitWeightColor = IndicatorsModel.Colors.Red.ToString();
            }

            //ready to ship does not have a clean way to check for red
            if (ReadyToShip > 0)
            {
                if (BillOfLadingEquipments.Any(be => be.BillOfLading.ToStorage && be.BillOfLading.IsCurrentRevision))
                {
                    Indicators.ReadyToShipColor = IndicatorsModel.Colors.Green.ToString();
                }
                else if (ReadyToShip > Quantity)
                {
                    Indicators.ReadyToShipColor = IndicatorsModel.Colors.Pink.ToString();
                }
                else
                {
                    Indicators.ReadyToShipColor = IndicatorsModel.Colors.Yellow.ToString();
                }
            }

            //ship qty
            if (ShippedQuantity > Quantity)
            {
                Indicators.ShippedQtyColor = IndicatorsModel.Colors.Red.ToString();
            }

            //left to ship
            if (ShippedQuantity > Quantity)
            {
                Indicators.LeftToShipColor = IndicatorsModel.Colors.Red.ToString();
            }

            //fully shipped
            if (ShippedQuantity > Quantity)
            {
                Indicators.FullyShippedColor = IndicatorsModel.Colors.Fuchsia.ToString();
            }
            else if (FullyShipped.HasValue && FullyShipped.Value == false)
            {
                Indicators.FullyShippedColor = IndicatorsModel.Colors.Pink.ToString();
            }
            else if (!FullyShipped.HasValue || FullyShipped.Value == true)
            {
                Indicators.FullyShippedColor = IndicatorsModel.Colors.Purple.ToString();
            }

            //customs value
            if (isCustomsProject && !EquipmentName.Equals("hardware", StringComparison.InvariantCultureIgnoreCase) && (!CustomsValue.HasValue || CustomsValue.Value <= 0))
            {
                Indicators.CustomsValueColor = IndicatorsModel.Colors.Red.ToString();
            }

            //sales price
            if (isCustomsProject && !EquipmentName.Equals("hardware", StringComparison.InvariantCultureIgnoreCase) && (!SalePrice.HasValue || SalePrice.Value <= 0))
            {
                Indicators.SalePriceColor = IndicatorsModel.Colors.Red.ToString();
            }

            //sales order
            if (string.IsNullOrEmpty(WorkOrderNumber) || !WorkOrderNumber.Contains(projectNumber.Trim()))
            {
                Indicators.SalesOrderNumberColor = IndicatorsModel.Colors.Red.ToString();
            }
        }
    }
}