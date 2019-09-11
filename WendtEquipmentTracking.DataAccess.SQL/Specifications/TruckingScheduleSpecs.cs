using System.Collections.Generic;
using WendtEquipmentTracking.DataAccess.SQL.Specifications.TruckingSchedules;

namespace WendtEquipmentTracking.DataAccess.SQL.Specifications
{

    public static class TruckingScheduleSpecs
    {
        public static Specification<TruckingSchedule> Id(int id)
        {
            return new IdSpecification(id);
        }

        public static Specification<TruckingSchedule> Ids(IEnumerable<int> ids)
        {
            return new IdsSpecification(ids);
        }

        public static Specification<TruckingSchedule> IsDeleted()
        {
            return new IsDeletedSpecification();
        }
    }
}
