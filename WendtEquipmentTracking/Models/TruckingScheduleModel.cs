using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using WendtEquipmentTracking.App.Common;

namespace WendtEquipmentTracking.App.Models
{
    public class TruckingScheduleModel
    {
        public int TruckingScheduleId { get; set; }

        [DisplayName("Request Date")]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:M/d/yy}")]
        public DateTime? RequestDate { get; set; }

        [DisplayName("Project #")]
        [Required]
        public double? ProjectNumber { get; set; }

        [DisplayName("Work Order")]
        public string WorkOrder { get; set; }

        [DisplayName("Purchase Order")]
        public string PurchaseOrder { get; set; }

        [DisplayName("Requested By")]
        public string RequestedBy { get; set; }

        [DisplayName("Ship From")]
        public string ShipFrom { get; set; }

        [DisplayName("Ship To")]
        public string ShipTo { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("# Pieces")]
        public double? NumPieces { get; set; }

        [DisplayName("Dimensions")]
        public string Dimensions { get; set; }

        [DisplayName("Weight")]
        public double? Weight { get; set; }

        [DisplayName("Carrier")]
        public string Carrier { get; set; }

        [DisplayName("Pick-Up-Date")]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:M/d/yy}")]
        public DateTime? PickUpDate { get; set; }

        [DisplayName("Comments")]
        public string Comments { get; set; }

        [DisplayName("Status")]
        [Required]
        public string Status { get; set; }

        [DisplayName("Weight")]
        public string WeightText
        {
            get
            {
                return Weight.HasValue ? Weight.Value.ToString() : string.Empty;
            }
        }

        [DisplayName("# Pieces")]
        public string NumPiecesText
        {
            get
            {
                return NumPieces.HasValue ? NumPieces.Value.ToString() : string.Empty;
            }
        }

        public IEnumerable<string> Statuses;
    }
}