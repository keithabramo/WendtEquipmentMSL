using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace WendtEquipmentTracking.DataAccess.SQL.Specifications.WorkOrderPrices
{

    internal class IdsSpecification : Specification<WorkOrderPrice>
    {

        private readonly IEnumerable<int> ids;

        public IdsSpecification(IEnumerable<int> ids)
        {
            this.ids = ids;
        }

        public override Expression<Func<WorkOrderPrice, bool>> IsSatisfiedBy()
        {
            return e => ids.Contains(e.WorkOrderPriceId);
        }
    }
}
