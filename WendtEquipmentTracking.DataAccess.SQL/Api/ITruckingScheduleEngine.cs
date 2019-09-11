using System.Collections.Generic;
using System.Linq;

namespace WendtEquipmentTracking.DataAccess.SQL.Api
{
    public interface ITruckingScheduleEngine
    {
        IQueryable<TruckingSchedule> ListAll();

        TruckingSchedule Get(Specification<TruckingSchedule> specification);

        IQueryable<TruckingSchedule> List(Specification<TruckingSchedule> specification);

        void AddNewTruckingSchedule(TruckingSchedule truckingSchedule);

        void AddNewTruckingSchedules(IEnumerable<TruckingSchedule> truckingSchedules);

        void UpdateTruckingSchedule(TruckingSchedule truckingSchedule);

        void UpdateTruckingSchedules(IEnumerable<TruckingSchedule> truckingSchedules);

        void DeleteTruckingSchedule(TruckingSchedule truckingSchedule);

        void SetDBContext(WendtEquipmentTrackingEntities dbContext);
    }
}
