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
    
    public partial class BillOfLading
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BillOfLading()
        {
            this.BillOfLadingEquipments = new HashSet<BillOfLadingEquipment>();
            this.HardwareKits = new HashSet<HardwareKit>();
        }
    
        public int BillOfLadingId { get; set; }
        public int ProjectId { get; set; }
        public int Revision { get; set; }
        public bool IsCurrentRevision { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string BillOfLadingNumber { get; set; }
        public Nullable<System.DateTime> DateShipped { get; set; }
        public string ShippedFrom { get; set; }
        public bool ToStorage { get; set; }
        public bool IsDeleted { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BillOfLadingEquipment> BillOfLadingEquipments { get; set; }
        public virtual Project Project { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HardwareKit> HardwareKits { get; set; }
    }
}
