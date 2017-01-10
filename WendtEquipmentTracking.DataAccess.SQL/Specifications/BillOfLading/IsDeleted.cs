using System;
using System.Linq.Expressions;

namespace WendtEquipmentTracking.DataAccess.SQL.Specifications.BillOfLadings
{

    internal class IsDeletedSpecification : Specification<BillOfLading>
    {

        public override Expression<Func<BillOfLading, bool>> IsSatisfiedBy()
        {
            return e => e.IsDeleted;
        }
    }
}
