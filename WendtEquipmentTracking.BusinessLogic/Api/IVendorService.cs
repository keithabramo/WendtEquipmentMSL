using System.Collections.Generic;
using WendtEquipmentTracking.BusinessLogic.BO;

namespace WendtEquipmentTracking.BusinessLogic.Api
{
    public interface IVendorService
    {
        IEnumerable<int> SaveAll(IEnumerable<VendorBO> vendorBOs);

        void Update(VendorBO vendorBO);

        void UpdateAll(IEnumerable<VendorBO> vendorBOs);

        void Delete(int id);

        IEnumerable<VendorBO> GetAll();

        IEnumerable<VendorBO> GetByIds(IEnumerable<int> ids);

        VendorBO GetById(int id);

    }
}