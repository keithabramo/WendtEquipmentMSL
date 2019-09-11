using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace WendtEquipmentTracking.DataAccess.SQL.Specifications.TruckingSchedules
{

    internal class IdsSpecification : Specification<TruckingSchedule>
    {

        private readonly IEnumerable<int> ids;

        public IdsSpecification(IEnumerable<int> ids)
        {
            this.ids = ids;
        }

        public override Expression<Func<TruckingSchedule, bool>> IsSatisfiedBy()
        {
            return e => ids.Contains(e.TruckingScheduleId);
        }
    }
}
