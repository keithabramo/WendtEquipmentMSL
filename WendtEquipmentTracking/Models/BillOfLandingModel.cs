using System;
using System.ComponentModel;

namespace WendtEquipmentTracking.App.Models
{
    public class BillOfLandingModel
    {
        public int BillOfLandingId { get; set; }
        public int EquipmentId { get; set; }

        [DisplayName("Revision")]
        public int Revision { get; set; }

        [DisplayName("BOL #")]
        public string BillOfLandingNumber { get; set; }

        [DisplayName("Quantity")]
        public double? Quantity { get; set; }

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

        public EquipmentModel Equipment { get; set; }
    }
}