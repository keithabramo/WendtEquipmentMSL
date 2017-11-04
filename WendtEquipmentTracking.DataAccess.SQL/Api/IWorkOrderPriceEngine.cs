using System.Collections.Generic;
using System.Linq;

namespace WendtEquipmentTracking.DataAccess.SQL.Api
{
    public interface IWorkOrderPriceEngine
    {
        IQueryable<WorkOrderPrice> ListAll();

        WorkOrderPrice Get(Specification<WorkOrderPrice> specification);

        IQueryable<WorkOrderPrice> List(Specification<WorkOrderPrice> specification);

        void AddNewWorkOrderPrice(WorkOrderPrice workOrderPrice);

        void AddNewWorkOrderPrices(IEnumerable<WorkOrderPrice> workOrderPrices);

        void UpdateWorkOrderPrice(WorkOrderPrice workOrderPrice);

        void UpdateWorkOrderPrices(IQueryable<WorkOrderPrice> workOrderPrices);

        void DeleteWorkOrderPrice(WorkOrderPrice workOrderPrice);

        void SetDBContext(WendtEquipmentTrackingEntities dbContext);
    }
}
