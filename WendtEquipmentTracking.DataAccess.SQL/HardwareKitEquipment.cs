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
    
    public partial class HardwareKitEquipment
    {
        public int HardwareKitEquipmentId { get; set; }
        public int HardwareKitId { get; set; }
        public int EquipmentId { get; set; }
        public double Quantity { get; set; }
    
        public virtual Equipment Equipment { get; set; }
        public virtual HardwareKit HardwareKit { get; set; }
    }
}
