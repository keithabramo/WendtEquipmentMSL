using System.Collections.Generic;
using WendtEquipmentTracking.BusinessLogic.BO;

namespace WendtEquipmentTracking.BusinessLogic.Api
{
    public interface IWorkOrderPriceService
    {
        void Save(WorkOrderPriceBO workOrderPriceBO);

        void SaveAll(IEnumerable<WorkOrderPriceBO> workOrderPriceBOs);

        void Update(WorkOrderPriceBO workOrderPriceBO);

        void UpdateAll(IEnumerable<WorkOrderPriceBO> workOrderPriceBOs);

        void Delete(int id);

        IEnumerable<WorkOrderPriceBO> GetAll();

        WorkOrderPriceBO GetById(int id);

    }
}