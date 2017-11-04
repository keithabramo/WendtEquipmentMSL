using System.Collections.Generic;
using System.Linq;

namespace WendtEquipmentTracking.DataAccess.SQL.Api
{
    public interface IHardwareKitEngine
    {
        IQueryable<HardwareKit> ListAll();

        HardwareKit Get(Specification<HardwareKit> specification);

        IQueryable<HardwareKit> List(Specification<HardwareKit> specification);

        void AddNewHardwareKit(HardwareKit hardwareKit);

        void UpdateHardwareKit(HardwareKit hardwareKit);

        void DeleteHardwareKit(HardwareKit hardwareKit);

        void SetDBContext(WendtEquipmentTrackingEntities dbContext);
    }
}
