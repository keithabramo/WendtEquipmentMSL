using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using WendtEquipmentTracking.Common;
using WendtEquipmentTracking.DataAccess.SQL.Api;
using WendtEquipmentTracking.DataAccess.SQL.Specifications;

namespace WendtEquipmentTracking.DataAccess.SQL.Engine
{

    public class VendorEngine : IVendorEngine
    {
        private IRepository<Vendor> repository = null;

        public VendorEngine(WendtEquipmentTrackingEntities dbContext)
        {
            this.repository = new Repository<Vendor>(dbContext);
        }

        public VendorEngine(Repository<Vendor> repository)
        {
            this.repository = repository;
        }

        public IQueryable<Vendor> ListAll()
        {
            return this.repository.Find(!VendorSpecs.IsDeleted());
        }

        public Vendor Get(Specification<Vendor> specification)
        {
            return this.repository.Single(!VendorSpecs.IsDeleted() && specification);
        }

        public IQueryable<Vendor> List(Specification<Vendor> specification)
        {
            return this.repository.Find(!VendorSpecs.IsDeleted() && specification);
        }

        public void AddNewVendor(Vendor vendor)
        {
            var now = DateTime.Now;

            vendor.CreatedDate = now;
            vendor.CreatedBy = ActiveDirectoryHelper.CurrentUserUsername();
            vendor.ModifiedDate = now;
            vendor.ModifiedBy = ActiveDirectoryHelper.CurrentUserUsername();

            this.repository.Insert(vendor);

        }

        public void AddNewVendors(IEnumerable<Vendor> vendors)
        {
            var now = DateTime.Now;

            foreach (var vendor in vendors)
            {
                vendor.CreatedDate = now;
                vendor.CreatedBy = ActiveDirectoryHelper.CurrentUserUsername();
                vendor.ModifiedDate = now;
                vendor.ModifiedBy = ActiveDirectoryHelper.CurrentUserUsername();
                this.repository.Insert(vendor);

            }
        }

        public void UpdateVendor(Vendor vendor)
        {
            var now = DateTime.Now;

            vendor.ModifiedDate = now;
            vendor.ModifiedBy = ActiveDirectoryHelper.CurrentUserUsername();

            this.repository.Update(vendor);

        }

        public void UpdateVendors(IEnumerable<Vendor> vendors)
        {
            var now = DateTime.Now;

            foreach (var vendor in vendors)
            {
                vendor.ModifiedDate = now;
                vendor.ModifiedBy = ActiveDirectoryHelper.CurrentUserUsername();

                this.repository.Update(vendor);
            }


        }

        public void DeleteVendor(Vendor vendor)
        {
            var now = DateTime.Now;

            vendor.ModifiedDate = now;
            vendor.ModifiedBy = ActiveDirectoryHelper.CurrentUserUsername();
            vendor.IsDeleted = true;

            this.repository.Update(vendor);

        }

        public void SetDBContext(WendtEquipmentTrackingEntities dbContext)
        {
            this.repository.Dispose();
            this.repository = new Repository<Vendor>(dbContext);
        }
    }
}
