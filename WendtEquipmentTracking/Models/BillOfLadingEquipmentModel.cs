﻿using System.ComponentModel;

namespace WendtEquipmentTracking.App.Models
{
    public class BillOfLadingEquipmentModel
    {
        public int BillOfLadingEquipmentId { get; set; }
        public int BillOfLadingId { get; set; }
        public int EquipmentId { get; set; }


        [DisplayName("Qty Shipped with this BOL")]
        public double Quantity { get; set; }

        [DisplayName("Shipped From")]
        public string ShippedFrom { get; set; }

        [DisplayName("HTS")]
        public string HTSCode { get; set; }

        [DisplayName("COO")]
        public string CountryOfOrigin { get; set; }

        public BillOfLadingModel BillOfLading { get; set; }
        public EquipmentModel Equipment { get; set; }
    }
}