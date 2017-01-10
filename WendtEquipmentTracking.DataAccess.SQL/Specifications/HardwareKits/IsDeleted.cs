using System;
using System.Linq.Expressions;

namespace WendtEquipmentTracking.DataAccess.SQL.Specifications.HardwareKits
{

    internal class IsDeletedSpecification : Specification<HardwareKit>
    {

        public override Expression<Func<HardwareKit, bool>> IsSatisfiedBy()
        {
            return e => e.IsDeleted;
        }
    }
}
