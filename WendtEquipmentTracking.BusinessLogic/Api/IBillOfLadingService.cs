using System.Collections.Generic;
using WendtEquipmentTracking.BusinessLogic.BO;

namespace WendtEquipmentTracking.BusinessLogic.Api
{
    public interface IBillOfLadingService
    {
        void Save(BillOfLadingBO billOfLadingBO);

        void Update(BillOfLadingBO billOfLadingBO);

        void Delete(int id);

        IEnumerable<BillOfLadingBO> GetAll();

        BillOfLadingBO GetById(int id);

        IEnumerable<BillOfLadingBO> GetByBillOfLadingNumber(int projectId, string billOfLadingNumber);
        IEnumerable<BillOfLadingBO> GetCurrentByProject(int projectId);

    }
}