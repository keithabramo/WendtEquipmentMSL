using System.Collections.Generic;

namespace WendtEquipmentTracking.DataAccess.SQL.Api
{
    public interface IWorkOrderPriceEngine
    {
        IEnumerable<WorkOrderPrice> ListAll();

        WorkOrderPrice Get(Specification<WorkOrderPrice> specification);

        IEnumerable<WorkOrderPrice> List(Specification<WorkOrderPrice> specification);

        void AddNewWorkOrderPrice(WorkOrderPrice workOrderPrice);

        void AddAllNewWorkOrderPrice(IEnumerable<WorkOrderPrice> workOrderPrices);

        void UpdateWorkOrderPrice(WorkOrderPrice workOrderPrice);

        void UpdateWorkOrderPrices(IEnumerable<WorkOrderPrice> workOrderPrices);

        void DeleteWorkOrderPrice(WorkOrderPrice workOrderPrice);

        void SetDBContext(WendtEquipmentTrackingEntities dbContext);
    }
}
