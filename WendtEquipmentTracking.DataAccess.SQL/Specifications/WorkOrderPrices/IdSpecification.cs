using System;
using System.Linq.Expressions;

namespace WendtEquipmentTracking.DataAccess.SQL.Specifications.WorkOrderPrices
{

    internal class IdSpecification : Specification<WorkOrderPrice>
    {

        private readonly int id;

        public IdSpecification(int id)
        {
            this.id = id;
        }

        public override Expression<Func<WorkOrderPrice, bool>> IsSatisfiedBy()
        {
            return e => e.WorkOrderPriceId == id;
        }
    }
}
