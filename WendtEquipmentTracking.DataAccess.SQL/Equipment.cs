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
    
    public partial class Equipment
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Equipment()
        {
            this.BillOfLadingEquipments = new HashSet<BillOfLadingEquipment>();
            this.HardwareKitEquipments = new HashSet<HardwareKitEquipment>();
        }
    
        public int EquipmentId { get; set; }
        public int ProjectId { get; set; }
        public string EquipmentName { get; set; }
        public string Priority { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<System.DateTime> ReleaseDate { get; set; }
        public string DrawingNumber { get; set; }
        public string WorkOrderNumber { get; set; }
        public Nullable<double> Quantity { get; set; }
        public string ShippingTagNumber { get; set; }
        public string Description { get; set; }
        public Nullable<double> UnitWeight { get; set; }
        public Nullable<double> TotalWeight { get; set; }
        public Nullable<double> TotalWeightShipped { get; set; }
        public Nullable<double> ReadyToShip { get; set; }
        public Nullable<double> ShippedQuantity { get; set; }
        public Nullable<double> LeftToShip { get; set; }
        public Nullable<bool> FullyShipped { get; set; }
        public Nullable<double> CustomsValue { get; set; }
        public Nullable<double> SalePrice { get; set; }
        public string HTSCode { get; set; }
        public string CountryOfOrigin { get; set; }
        public string Notes { get; set; }
        public string AutoShipFile { get; set; }
        public string SalesOrderNumber { get; set; }
        public bool IsHardware { get; set; }
        public bool IsDeleted { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BillOfLadingEquipment> BillOfLadingEquipments { get; set; }
        public virtual Project Project { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HardwareKitEquipment> HardwareKitEquipments { get; set; }
    }
}
