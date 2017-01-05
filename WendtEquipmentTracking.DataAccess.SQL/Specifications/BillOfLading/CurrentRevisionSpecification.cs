using System;
using System.Linq.Expressions;

namespace WendtEquipmentTracking.DataAccess.SQL.Specifications.BillOfLadings
{

    internal class CurrentRevisionSpecification : Specification<BillOfLading>
    {

        public override Expression<Func<BillOfLading, bool>> IsSatisfiedBy()
        {
            return e => e.IsCurrentRevision;
        }
    }
}
