﻿using System;
using System.Collections.Generic;
using System.Linq;
using WendtEquipmentTracking.Common;

namespace WendtEquipmentTracking.BusinessLogic.BO
{
    public class BillOfLandingBO
    {
        public int BillOfLandingId { get; set; }
        public int EquipmentId { get; set; }
        public int Revision { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string BillOfLandingNumber { get; set; }
        public string ShipToCompany { get; set; }
        public string ShipToAddress { get; set; }
        public string ShipToCSZ { get; set; }
        public string ShipToPhoneFax { get; set; }
        public string ShipToContact1 { get; set; }
        public string ShipToContact2 { get; set; }
    }
}