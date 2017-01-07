using WendtEquipmentTracking.DataAccess.SQL.Specifications.WorkOrderPrices;

namespace WendtEquipmentTracking.DataAccess.SQL.Specifications
{

    public static class WorkOrderPriceSpecs
    {
        public static Specification<WorkOrderPrice> Id(int id)
        {
            return new IdSpecification(id);
        }
    }
}
