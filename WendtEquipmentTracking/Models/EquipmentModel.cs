using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using WendtEquipmentTracking.App.Common;

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
        public int? PriorityNumber { get; set; }

        [JsonConverter(typeof(CustomDateTimeConverter))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [DisplayName("Release Date")]
        [Required]
        public DateTime? ReleaseDate { get; set; }

        [DataType(DataType.MultilineText)]
        [DisplayName("Drawing #")]
        [Required]
        public string DrawingNumber { get; set; }


        [DisplayName("Work Order #")]
        [Required]
        public string WorkOrderNumber { get; set; }


        [DisplayName("Qty")]
        public double Quantity { get; set; }

        [DataType(DataType.MultilineText)]
        [DisplayName("Ship Tag #")]
        [Required]
        public string ShippingTagNumber { get; set; }

        [DataType(DataType.MultilineText)]
        [DisplayName("Description")]
        [Required]
        public string Description { get; set; }


        [DisplayName("Unit Wt")]
        [DisplayFormat(DataFormatString = "{#.##}")]
        [Required]
        public double? UnitWeight { get; set; }


        [DisplayName("Total Wt")]
        public string TotalWeight { get; set; }


        [DisplayName("Total Wt Shipped")]
        public string TotalWeightShipped { get; set; }


        [DisplayName("RTS")]
        public double ReadyToShip { get; set; }


        [DisplayName("Ship Qty")]
        public string ShippedQuantity { get; set; }


        [DisplayName("Left To Ship")]
        public string LeftToShip { get; set; }


        [DisplayName("Fully Shipped")]
        public bool FullyShipped { get; set; }


        [DisplayName("Sales+Soft Costs")]
        [DataType(DataType.Currency)]
        public double? CustomsValue { get; set; }


        [DisplayName("Sale Price")]
        [DataType(DataType.Currency)]
        public double? SalePrice { get; set; }


        [DisplayName("Sales+Soft Costs")]
        [DataType(DataType.Currency)]
        public string CustomsValueText
        {
            get
            {
                return CustomsValue.HasValue ? CustomsValue.Value.ToString() : string.Empty;
            }
        }

        [DisplayName("Sale Price")]
        [DataType(DataType.Currency)]

        public string SalePriceText
        {
            get
            {
                return SalePrice.HasValue ? SalePrice.Value.ToString() : string.Empty;
            }
        }

        [DisplayName("Unit Wt")]
        [DataType(DataType.Currency)]
        public string UnitWeightText
        {
            get
            {
                return UnitWeight.HasValue ? UnitWeight.Value.ToString() : string.Empty;
            }
        }

        [DataType(DataType.MultilineText)]
        [DisplayName("Notes")]
        public string Notes { get; set; }


        [DisplayName("Shipped From")]
        public string ShippedFrom { get; set; }

        [DisplayName("HTS")]
        public string HTSCode { get; set; }

        [DisplayName("COO")]
        public string CountryOfOrigin { get; set; }


        [DisplayName("Fully Shipped")]
        public string FullyShippedText
        {
            get
            {
                return FullyShipped ? "YES" : "NO";
            }
        }

        public string IsAssociatedToHardwareKitText
        {
            get
            {
                return IsAssociatedToHardwareKit.ToString();
            }
        }

        public string IsHardwareKitText
        {
            get
            {
                return IsHardwareKit.ToString();
            }
        }

        public string HasErrorsText
        {
            get
            {
                return HasErrors.ToString();
            }
        }


        //Calculated Properties
        public bool HasBillOfLading { get; set; }
        public bool HasBillOfLadingInStorage { get; set; }
        public bool IsHardwareKit { get; set; }
        public bool IsAssociatedToHardwareKit { get; set; }
        public string AssociatedHardwareKitNumber { get; set; }
        public bool IsDuplicate { get; set; }
        public bool HasErrors { get; set; }



        public IEnumerable<int> Priorities { get; set; }

        public IndicatorsModel Indicators { get; set; }

        public void SetIndicators(double projectNumber, bool isCustomsProject)
        {
            Indicators = new IndicatorsModel();

            //equipment
            if (EquipmentName.Equals("DO NOT USE", StringComparison.InvariantCultureIgnoreCase))
            {
                HasErrors = true;
                Indicators.EquipmentNameColor = IndicatorsModel.Colors.Red.ToString();
            }

            //unit weight
            if ((UnitWeight == null || UnitWeight <= 0) && !EquipmentName.Equals("hardware", StringComparison.InvariantCultureIgnoreCase))
            {
                HasErrors = true;
                Indicators.UnitWeightColor = IndicatorsModel.Colors.Red.ToString();
            }

            //ready to ship does not have a clean way to check for red
            if (ReadyToShip != 0 && !FullyShipped)
            {
                if (HasBillOfLadingInStorage)
                {
                    Indicators.ReadyToShipColor = IndicatorsModel.Colors.Green.ToString();
                }
                else if (ReadyToShip == Convert.ToInt32(LeftToShip))
                {
                    Indicators.ReadyToShipColor = IndicatorsModel.Colors.Yellow.ToString();
                }
                else if (ReadyToShip > Convert.ToInt32(LeftToShip))
                {
                    HasErrors = true;
                    Indicators.ReadyToShipColor = IndicatorsModel.Colors.Pink.ToString();
                }
                else
                {
                    Indicators.ReadyToShipColor = IndicatorsModel.Colors.LightBlue.ToString();
                }
            }

            //ship qty
            if (ShippedQuantity.ToNullable<double>() > Quantity)
            {
                HasErrors = true;
                Indicators.ShippedQtyColor = IndicatorsModel.Colors.Red.ToString();
            }

            //left to ship
            if (ShippedQuantity.ToNullable<double>() > Quantity)
            {
                HasErrors = true;
                Indicators.LeftToShipColor = IndicatorsModel.Colors.Red.ToString();
            }

            //fully shipped
            if (ShippedQuantity.ToNullable<double>() > Quantity)
            {
                HasErrors = true;
                Indicators.FullyShippedColor = IndicatorsModel.Colors.Fuchsia.ToString();
            }
            else if (!FullyShipped)
            {
                Indicators.FullyShippedColor = IndicatorsModel.Colors.Pink.ToString();
            }
            else if (FullyShipped)
            {
                Indicators.FullyShippedColor = IndicatorsModel.Colors.Purple.ToString();
            }

            //customs value
            if (isCustomsProject && !FullyShipped && !EquipmentName.Equals("hardware", StringComparison.InvariantCultureIgnoreCase) && (!CustomsValue.HasValue || CustomsValue.Value <= 0))
            {
                HasErrors = true;
                Indicators.CustomsValueColor = IndicatorsModel.Colors.Red.ToString();
            }

            //sales price
            if (isCustomsProject && !FullyShipped && !EquipmentName.Equals("hardware", StringComparison.InvariantCultureIgnoreCase) && (!SalePrice.HasValue || SalePrice.Value <= 0))
            {
                HasErrors = true;
                Indicators.SalePriceColor = IndicatorsModel.Colors.Red.ToString();
            }

            //sales order
            if (string.IsNullOrEmpty(WorkOrderNumber) || !WorkOrderNumber.StartsWith(projectNumber.ToString()))
            {
                HasErrors = true;
                Indicators.WorkOrderNumberColor = IndicatorsModel.Colors.Yellow.ToString();
            }
        }
    }
}