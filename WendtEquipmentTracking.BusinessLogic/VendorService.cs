
using System.Collections.Generic;
using System.Linq;
using WendtEquipmentTracking.BusinessLogic.Api;
using WendtEquipmentTracking.BusinessLogic.BO;
using WendtEquipmentTracking.DataAccess.SQL;
using WendtEquipmentTracking.DataAccess.SQL.Api;
using WendtEquipmentTracking.DataAccess.SQL.Engine;
using WendtEquipmentTracking.DataAccess.SQL.Specifications;

namespace WendtEquipmentTracking.BusinessLogic
{
    public class VendorService : IVendorService
    {
        private WendtEquipmentTrackingEntities dbContext;
        private IVendorEngine vendorEngine;

        public VendorService()
        {
            dbContext = new WendtEquipmentTrackingEntities();
            vendorEngine = new VendorEngine(dbContext);
        }

        public IEnumerable<int> SaveAll(IEnumerable<VendorBO> vendorBOs)
        {
            var vendors = vendorBOs.Select(x => new Vendor
            {
                Name = x.Name,
                ProjectId = x.ProjectId,
                Address = x.Address,
                Contact1 = x.Contact1,
                Email = x.Email,
                PhoneFax = x.PhoneFax
            });

            vendorEngine.AddNewVendors(vendors);

            dbContext.SaveChanges();

            return vendors.Select(x => x.VendorId).ToList();

        }

        public IEnumerable<VendorBO> GetAll(int projectId)
        {
            var vendors = vendorEngine.List(VendorSpecs.ProjectId(projectId));

            var vendorBOs = vendors.Select(x => new VendorBO
            {
                Name = x.Name,
                ProjectId = x.ProjectId,
                Address = x.Address,
                Contact1 = x.Contact1,
                Email = x.Email,
                PhoneFax = x.PhoneFax,
                VendorId = x.VendorId
            });

            return vendorBOs.ToList();
        }

        public VendorBO GetById(int id)
        {
            var vendor = vendorEngine.Get(VendorSpecs.Id(id));

            var vendorBO = new VendorBO
            {
                Name = vendor.Name,
                ProjectId = vendor.ProjectId,
                Address = vendor.Address,
                Contact1 = vendor.Contact1,
                Email = vendor.Email,
                PhoneFax = vendor.PhoneFax,
                VendorId = vendor.VendorId
            };

            return vendorBO;
        }

        public IEnumerable<VendorBO> GetByIds(IEnumerable<int> ids)
        {
            var vendors = vendorEngine.List(VendorSpecs.Ids(ids));

            var vendorBOs = vendors.Select(x => new VendorBO
            {
                Name = x.Name,
                ProjectId = x.ProjectId,
                Address = x.Address,
                Contact1 = x.Contact1,
                Email = x.Email,
                PhoneFax = x.PhoneFax,
                VendorId = x.VendorId
            });

            return vendorBOs.ToList();
        }

        public void Update(VendorBO vendorBO)
        {
            var oldVendor = vendorEngine.Get(VendorSpecs.Id(vendorBO.VendorId));
            oldVendor.Name = vendorBO.Name;
            oldVendor.ProjectId = vendorBO.ProjectId;
            oldVendor.Address = vendorBO.Address;
            oldVendor.Contact1 = vendorBO.Contact1;
            oldVendor.Email = vendorBO.Email;
            oldVendor.PhoneFax = vendorBO.PhoneFax;

            vendorEngine.UpdateVendor(oldVendor);

            dbContext.SaveChanges();
        }

        public void UpdateAll(IEnumerable<VendorBO> vendorBOs)
        {
            //Performance Issue?
            var oldVendors = vendorEngine.List(VendorSpecs.Ids(vendorBOs.Select(x => x.VendorId))).ToList();

            foreach (var oldVendor in oldVendors)
            {
                var vendorBO = vendorBOs.FirstOrDefault(x => x.VendorId == oldVendor.VendorId);

                if (vendorBO != null)
                {
                    oldVendor.Name = vendorBO.Name;
                    oldVendor.ProjectId = vendorBO.ProjectId;
                    oldVendor.Address = vendorBO.Address;
                    oldVendor.Contact1 = vendorBO.Contact1;
                    oldVendor.Email = vendorBO.Email;
                    oldVendor.PhoneFax = vendorBO.PhoneFax;
                }
            }

            vendorEngine.UpdateVendors(oldVendors);

            dbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            var vendor = vendorEngine.Get(VendorSpecs.Id(id));

            if (vendor != null)
            {
                vendorEngine.DeleteVendor(vendor);
            }

            dbContext.SaveChanges();
        }
    }
}
