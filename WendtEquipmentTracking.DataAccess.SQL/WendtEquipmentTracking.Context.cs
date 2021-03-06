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
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
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
        public virtual DbSet<BillOfLadingAttachment> BillOfLadingAttachments { get; set; }
        public virtual DbSet<BillOfLadingEquipment> BillOfLadingEquipments { get; set; }
        public virtual DbSet<Broker> Brokers { get; set; }
        public virtual DbSet<Equipment> Equipments { get; set; }
        public virtual DbSet<EquipmentAttachment> EquipmentAttachments { get; set; }
        public virtual DbSet<HardwareCommercialCode> HardwareCommercialCodes { get; set; }
        public virtual DbSet<HardwareKit> HardwareKits { get; set; }
        public virtual DbSet<HardwareKitEquipment> HardwareKitEquipments { get; set; }
        public virtual DbSet<Priority> Priorities { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<TruckingSchedule> TruckingSchedules { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Vendor> Vendors { get; set; }
        public virtual DbSet<WorkOrderPrice> WorkOrderPrices { get; set; }
    
        public virtual int UpdateRTSAfterBOLRevision(Nullable<int> billOfLadingId)
        {
            var billOfLadingIdParameter = billOfLadingId.HasValue ?
                new ObjectParameter("BillOfLadingId", billOfLadingId) :
                new ObjectParameter("BillOfLadingId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("UpdateRTSAfterBOLRevision", billOfLadingIdParameter);
        }
    }
}
