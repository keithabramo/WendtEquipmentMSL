using System;
using System.Linq.Expressions;

namespace WendtEquipmentTracking.DataAccess.SQL.Specifications.Brokers
{

    internal class IsDeletedSpecification : Specification<Broker>
    {

        public override Expression<Func<Broker, bool>> IsSatisfiedBy()
        {
            return e => e.IsDeleted;
        }
    }
}
