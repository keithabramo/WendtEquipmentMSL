using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using WendtEquipmentTracking.App.Common;

namespace WendtEquipmentTracking.App.Models
{
    public class EquipmentRevisionModel : EquipmentModel
    {

        public int EquipmentRevisionId { get; set; }

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
        public double NewQuantity { get; set; }

        [DataType(DataType.MultilineText)]
        [DisplayName("New Ship Tag #")]
        [Required]
        public string NewShippingTagNumber { get; set; }

        [DataType(DataType.MultilineText)]
        [DisplayName("Description")]
        [Required]
        public string Description { get; set; }


        [DisplayName("Unit Wt")]
        [DisplayFormat(DataFormatString = "{#.##}")]
        [Required]
        public double? UnitWeight { get; set; }

        [DisplayName("Unit Wt")]
        [DataType(DataType.Currency)]
        public string UnitWeightText
        {
            get
            {
                return UnitWeight.HasValue ? UnitWeight.Value.ToString() : string.Empty;
            }
        }

        [DisplayName("Shipped From")]
        public string ShippedFrom { get; set; }

    }
}