using System;
using System.Linq.Expressions;

namespace WendtEquipmentTracking.DataAccess.SQL.Specifications.TruckingSchedules
{

    internal class IsDeletedSpecification : Specification<TruckingSchedule>
    {

        public override Expression<Func<TruckingSchedule, bool>> IsSatisfiedBy()
        {
            return e => e.IsDeleted;
        }
    }
}
