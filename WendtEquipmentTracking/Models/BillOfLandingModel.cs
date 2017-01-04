using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WendtEquipmentTracking.App.Models
{
    public class BillOfLandingModel : BaseModel
    {
        public int BillOfLandingId { get; set; }
        public int ProjectId { get; set; }

        [DisplayName("Revision")]
        public int Revision { get; set; }

        [DisplayName("Is Current Revision")]
        public bool IsCurrentRevision { get; set; }

        [DisplayName("BOL #")]
        public string BillOfLandingNumber { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [DisplayName("Date Shipped")]
        public DateTime? DateShipped { get; set; }

        [DisplayName("Shipped From")]
        public string ShippedFrom { get; set; }

        [DisplayName("Ship To Company")]
        public string ShipToCompany { get; set; }

        [DisplayName("Ship To Address")]
        public string ShipToAddress { get; set; }

        [DisplayName("Ship To CSZ")]
        public string ShipToCSZ { get; set; }

        [DisplayName("Ship To Phone\\Fax")]
        public string ShipToPhoneFax { get; set; }

        [DisplayName("Ship To Contact 1")]
        public string ShipToContact1 { get; set; }

        [DisplayName("Ship To Contact 2")]
        public string ShipToContact2 { get; set; }

        public IList<BillOfLandingEquipmentModel> BillOfLandingEquipments { get; set; }
    }
}