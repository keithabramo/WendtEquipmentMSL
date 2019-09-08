using System.Collections.Generic;
using WendtEquipmentTracking.BusinessLogic.BO;

namespace WendtEquipmentTracking.BusinessLogic.Api
{
    public interface IWorkOrderPriceService
    {
        void Save(WorkOrderPriceBO workOrderPriceBO);

        IEnumerable<int> SaveAll(IEnumerable<WorkOrderPriceBO> workOrderPriceBOs);

        void Update(WorkOrderPriceBO workOrderPriceBO);

        void UpdateAll(IEnumerable<WorkOrderPriceBO> workOrderPriceBOs);

        void Delete(int id);

        IEnumerable<WorkOrderPriceBO> GetAll(int projectId);

        IEnumerable<WorkOrderPriceBO> GetByIds(IEnumerable<int> ids);

        WorkOrderPriceBO GetById(int id);

    }
}