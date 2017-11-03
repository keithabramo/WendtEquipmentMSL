using System.Collections.Generic;
using WendtEquipmentTracking.DataAccess.SQL.Specifications.WorkOrderPrices;

namespace WendtEquipmentTracking.DataAccess.SQL.Specifications
{

    public static class WorkOrderPriceSpecs
    {
        public static Specification<WorkOrderPrice> Id(int id)
        {
            return new IdSpecification(id);
        }

        public static Specification<WorkOrderPrice> Ids(IEnumerable<int> ids)
        {
            return new IdsSpecification(ids);
        }

        public static Specification<WorkOrderPrice> IsDeleted()
        {
            return new IsDeletedSpecification();
        }
    }
}
