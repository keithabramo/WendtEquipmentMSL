using System;
using System.Linq.Expressions;

namespace WendtEquipmentTracking.DataAccess.SQL.Specifications.Priorities
{

    internal class IsDeletedSpecification : Specification<Priority>
    {

        public override Expression<Func<Priority, bool>> IsSatisfiedBy()
        {
            return e => e.IsDeleted;
        }
    }
}
