using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using WendtEquipmentTracking.App.Common;

namespace WendtEquipmentTracking.App.Models
{
    public class EquipmentRevisionModel : BaseModel
    {
        public EquipmentRevisionModel()
        {
            RevisionIndicators = new RevisionIndicatorsModel();
        }

        // EXISTING EQUIPMENT FIELDS

        public int EquipmentId { get; set; }
        public int ProjectId { get; set; }

        [DisplayName("Equipment")]
        [Required]
        public string EquipmentName { get; set; }

        [DisplayName("Prty")]
        [Required]
        public int? PriorityNumber { get; set; }

        [JsonConverter(typeof(CustomDateTimeConverter))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:M/d/yy}")]
        [DisplayName("Released")]
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


        [DisplayName("Ship Qty")]
        public string ShippedQuantity { get; set; }

        [DisplayName("Shipped From")]
        public string ShippedFrom { get; set; }

        // NEW REVISION FIELDS

        public int NewEquipmentId { get; set; }

        [DisplayName("New Equipment")]
        [Required]
        public string NewEquipmentName { get; set; }

        [DisplayName("New Prty")]
        [Required]
        public int? NewPriorityNumber { get; set; }

        [JsonConverter(typeof(CustomDateTimeConverter))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:M/d/yy}")]
        [DisplayName("New Released")]
        [Required]
        public DateTime? NewReleaseDate { get; set; }


        [DataType(DataType.MultilineText)]
        [DisplayName("New Drawing #")]
        [Required]
        public string NewDrawingNumber { get; set; }


        [DisplayName("New Work Order #")]
        [Required]
        public string NewWorkOrderNumber { get; set; }


        [DisplayName("New Qty")]
        public double? NewQuantity { get; set; }

        [DataType(DataType.MultilineText)]
        [DisplayName("New Ship Tag #")]
        [Required]
        public string NewShippingTagNumber { get; set; }

        [DataType(DataType.MultilineText)]
        [DisplayName("Description")]
        [Required]
        public string NewDescription { get; set; }


        [DisplayName("Unit Wt")]
        [DisplayFormat(DataFormatString = "{#.##}")]
        [Required]
        public double? NewUnitWeight { get; set; }

        [DisplayName("Unit Wt")]
        [DataType(DataType.Currency)]
        public string NewUnitWeightText
        {
            get
            {
                return NewUnitWeight.HasValue ? NewUnitWeight.Value.ToString() : string.Empty;
            }
        }

        [DisplayName("Shipped From")]
        public string NewShippedFrom { get; set; }

        public int Order { get; set; }


        // Calculated Fields
        public bool HasNewEquipment { get; set; }
        public bool HasExistingEquipment { get; set; }

        [DisplayName("Unit Wt")]
        [DataType(DataType.Currency)]
        public string UnitWeightText
        {
            get
            {
                return UnitWeight.HasValue ? UnitWeight.Value.ToString() : string.Empty;
            }
        }

        public bool EquipmentNameChanged
        {
            get
            {
                return !string.IsNullOrEmpty(EquipmentName) && !EquipmentName.Equals(NewEquipmentName);
            }
        }

        public bool DescriptionChanged
        {
            get
            {
                return !string.IsNullOrEmpty(Description) && !Description.Equals(NewDescription);
            }
        }

        public bool PriorityNumberChanged
        {
            get
            {
                return !PriorityNumber.Equals(NewPriorityNumber);
            }
        }

        public bool WorkOrderNumberChanged
        {
            get
            {
                return !string.IsNullOrEmpty(WorkOrderNumber) && !WorkOrderNumber.Equals(NewWorkOrderNumber);
            }
        }

        public bool QuantityChanged
        {
            get
            {
                return !Quantity.Equals(NewQuantity);
            }
        }

        public bool UnitWeightChanged
        {
            get
            {
                return !UnitWeight.Equals(NewUnitWeight);
            }
        }

        public bool HasChanged
        {
            get
            {
                var dataChanged = EquipmentNameChanged || DescriptionChanged || PriorityNumberChanged || WorkOrderNumberChanged || QuantityChanged || UnitWeightChanged;

                return !HasNewEquipment || !HasExistingEquipment || dataChanged;
            }
        }

        public RevisionIndicatorsModel RevisionIndicators { get; set; }

        public void SetRevisionIndicators()
        {
            RevisionIndicators = new RevisionIndicatorsModel();

            if (HasChanged)
            {
                if (HasExistingEquipment && HasNewEquipment)
                {

                    if (EquipmentNameChanged)
                    {
                        RevisionIndicators.NewEquipmentNameColor = IndicatorColors.Green.ToString();
                    }

                    if (DescriptionChanged)
                    {
                        RevisionIndicators.NewDescriptionColor = IndicatorColors.Green.ToString();
                    }

                    if (PriorityNumberChanged)
                    {
                        RevisionIndicators.NewPriorityNumberColor = IndicatorColors.Green.ToString();
                    }

                    if (WorkOrderNumberChanged)
                    {
                        RevisionIndicators.NewWorkOrderNumberColor = IndicatorColors.Green.ToString();
                    }

                    if (QuantityChanged)
                    {
                        RevisionIndicators.NewQuantityColor = IndicatorColors.Green.ToString();
                    }

                    if (UnitWeightChanged)
                    {
                        RevisionIndicators.NewUnitWeightColor = IndicatorColors.Green.ToString();
                    }

                }
                else if (HasExistingEquipment && !HasNewEquipment)
                {
                    if (!string.IsNullOrWhiteSpace(ShippedQuantity) && Int32.Parse(ShippedQuantity) > 0)
                    {
                        RevisionIndicators.ShippedQtyColor = IndicatorColors.Green.ToString();
                    }

                    RevisionIndicators.NewDescriptionColor = IndicatorColors.Red.ToString();
                    RevisionIndicators.NewEquipmentNameColor = IndicatorColors.Red.ToString();
                    RevisionIndicators.NewPriorityNumberColor = IndicatorColors.Red.ToString();
                    RevisionIndicators.NewQuantityColor = IndicatorColors.Red.ToString();
                    RevisionIndicators.NewUnitWeightColor = IndicatorColors.Red.ToString();
                    RevisionIndicators.NewWorkOrderNumberColor = IndicatorColors.Red.ToString();
                    RevisionIndicators.NewReleaseDateColor = IndicatorColors.Red.ToString();
                    RevisionIndicators.NewShippedFromColor = IndicatorColors.Red.ToString();
                    RevisionIndicators.NewShippingTagNumberColor = IndicatorColors.Red.ToString();
                    RevisionIndicators.NewDrawingNumberColor = IndicatorColors.Red.ToString();

                }
                else if (HasNewEquipment && !HasExistingEquipment)
                {
                    RevisionIndicators.ShippedQtyColor = IndicatorColors.Red.ToString();
                    RevisionIndicators.DescriptionColor = IndicatorColors.Red.ToString();
                    RevisionIndicators.EquipmentNameColor = IndicatorColors.Red.ToString();
                    RevisionIndicators.PriorityNumberColor = IndicatorColors.Red.ToString();
                    RevisionIndicators.QuantityColor = IndicatorColors.Red.ToString();
                    RevisionIndicators.UnitWeightColor = IndicatorColors.Red.ToString();
                    RevisionIndicators.WorkOrderNumberColor = IndicatorColors.Red.ToString();
                    RevisionIndicators.ReleaseDateColor = IndicatorColors.Red.ToString();
                    RevisionIndicators.ShippedFromColor = IndicatorColors.Red.ToString();
                    RevisionIndicators.ShippingTagNumberColor = IndicatorColors.Red.ToString();
                    RevisionIndicators.DrawingNumberColor = IndicatorColors.Red.ToString();

                }
            }
        }
    }
}