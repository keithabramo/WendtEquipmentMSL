using System.Collections.Generic;
using WendtEquipmentTracking.BusinessLogic.BO;

namespace WendtEquipmentTracking.BusinessLogic.Api
{
    public interface IBillOfLandingService
    {
        void Save(BillOfLandingBO billOfLandingBO);

        void Update(BillOfLandingBO billOfLandingBO);

        void Delete(int id);

        IEnumerable<BillOfLandingBO> GetAll();

        BillOfLandingBO GetById(int id);

        IEnumerable<BillOfLandingBO> GetByBillOfLandingNumber(string billOfLandingNumber);

    }
}