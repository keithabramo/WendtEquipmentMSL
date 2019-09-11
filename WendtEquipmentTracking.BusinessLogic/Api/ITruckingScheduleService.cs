using System.Collections.Generic;
using WendtEquipmentTracking.BusinessLogic.BO;

namespace WendtEquipmentTracking.BusinessLogic.Api
{
    public interface ITruckingScheduleService
    {
        IEnumerable<int> SaveAll(IEnumerable<TruckingScheduleBO> truckingScheduleBOs);

        void Update(TruckingScheduleBO truckingScheduleBO);

        void UpdateAll(IEnumerable<TruckingScheduleBO> truckingScheduleBOs);

        void Delete(int id);

        IEnumerable<TruckingScheduleBO> GetAll();

        IEnumerable<TruckingScheduleBO> GetByIds(IEnumerable<int> ids);

    }
}