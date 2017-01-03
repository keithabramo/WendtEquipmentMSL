using System;
using System.Linq.Expressions;

namespace WendtEquipmentTracking.DataAccess.SQL.Specifications.BillOfLandings
{

    internal class CurrentRevisionSpecification : Specification<BillOfLanding>
    {

        public override Expression<Func<BillOfLanding, bool>> IsSatisfiedBy()
        {
            return e => e.IsCurrentRevision;
        }
    }
}
