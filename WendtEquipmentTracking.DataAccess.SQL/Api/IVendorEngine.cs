using System.Collections.Generic;
using System.Linq;

namespace WendtEquipmentTracking.DataAccess.SQL.Api
{
    public interface IVendorEngine
    {
        IQueryable<Vendor> ListAll();

        Vendor Get(Specification<Vendor> specification);

        IQueryable<Vendor> List(Specification<Vendor> specification);

        void AddNewVendor(Vendor vendor);

        void AddNewVendors(IEnumerable<Vendor> vendors);

        void UpdateVendor(Vendor vendor);

        void UpdateVendors(IEnumerable<Vendor> vendors);

        void DeleteVendor(Vendor vendor);

        void SetDBContext(WendtEquipmentTrackingEntities dbContext);
    }
}
