//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WendtEquipmentTracking.DataAccess.SQL
{
    using System;
    using System.Collections.Generic;
    
    public partial class BillOfLadingEquipment
    {
        public int BillOfLadingEquipmentId { get; set; }
        public int BillOfLadingId { get; set; }
        public int EquipmentId { get; set; }
        public double Quantity { get; set; }
        public string ShippedFrom { get; set; }
        public bool IsDeleted { get; set; }
    
        public virtual BillOfLading BillOfLading { get; set; }
        public virtual Equipment Equipment { get; set; }
    }
}
