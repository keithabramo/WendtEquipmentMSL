using System;
using System.Linq.Expressions;

namespace WendtEquipmentTracking.DataAccess.SQL.Specifications.WorkOrderPrices
{

    internal class IsDeletedSpecification : Specification<WorkOrderPrice>
    {

        public override Expression<Func<WorkOrderPrice, bool>> IsSatisfiedBy()
        {
            return e => e.IsDeleted;
        }
    }
}
