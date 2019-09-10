using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace WendtEquipmentTracking.App.Models
{
    public class BillOfLadingModel : BaseModel
    {
        public int BillOfLadingId { get; set; }
        public int ProjectId { get; set; }

        [DisplayName("Revision")]
        public int Revision { get; set; }

        [DisplayName("Is Current Revision")]
        public bool IsCurrentRevision { get; set; }

        [DisplayName("BOL #")]
        [Required]
        [Remote("ValidBOLNumber", "Validate", AdditionalFields = "BillOfLadingId", ErrorMessage = "This bill of lading number already exists")]
        public string BillOfLadingNumber { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:M/d/yy}")]
        [DisplayName("Shipped")]
        [Required]
        public DateTime? DateShipped { get; set; }

        [DisplayName("Freight Terms")]
        public string FreightTerms { get; set; }

        [DisplayName("Carrier")]
        public string Carrier { get; set; }

        [DisplayName("Trailer #")]
        public string TrailerNumber { get; set; }

        [DisplayName("Shipped From")]
        public string ShippedFrom { get; set; }

        [DisplayName("Shipped To")]
        public string ShippedTo { get; set; }

        [DisplayName("To Storage")]
        public bool ToStorage { get; set; }

        public IList<BillOfLadingEquipmentModel> BillOfLadingEquipments { get; set; }

    }
}