using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WendtEquipmentTracking.App.Models
{
    public class HardwareCommercialCodeModel
    {
        public int HardwareCommercialCodeId { get; set; }
        public string PartNumber { get; set; }
        public string Description { get; set; }
        public string CommodityCode { get; set; }
    }
}