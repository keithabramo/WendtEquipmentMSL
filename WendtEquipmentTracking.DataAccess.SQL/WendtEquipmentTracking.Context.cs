﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class WendtEquipmentTrackingEntities : DbContext
    {
        public WendtEquipmentTrackingEntities()
            : base("name=WendtEquipmentTrackingEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<BillOfLading> BillOfLadings { get; set; }
        public virtual DbSet<BillOfLadingEquipment> BillOfLadingEquipments { get; set; }
        public virtual DbSet<Equipment> Equipments { get; set; }
        public virtual DbSet<HardwareKit> HardwareKits { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
    }
}
