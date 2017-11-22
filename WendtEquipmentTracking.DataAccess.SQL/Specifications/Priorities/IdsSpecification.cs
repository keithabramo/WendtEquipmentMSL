using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace WendtEquipmentTracking.DataAccess.SQL.Specifications.Priorities
{

    internal class IdsSpecification : Specification<Priority>
    {

        private readonly IEnumerable<int> ids;

        public IdsSpecification(IEnumerable<int> ids)
        {
            this.ids = ids;
        }

        public override Expression<Func<Priority, bool>> IsSatisfiedBy()
        {
            return e => ids.Contains(e.PriorityId);
        }
    }
}
