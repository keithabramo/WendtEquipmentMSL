using System;
using System.Linq.Expressions;

namespace WendtEquipmentTracking.DataAccess.SQL.Specifications.TruckingSchedules
{

    internal class IdSpecification : Specification<TruckingSchedule>
    {

        private readonly int id;

        public IdSpecification(int id)
        {
            this.id = id;
        }

        public override Expression<Func<TruckingSchedule, bool>> IsSatisfiedBy()
        {
            return e => e.TruckingScheduleId == id;
        }
    }
}
