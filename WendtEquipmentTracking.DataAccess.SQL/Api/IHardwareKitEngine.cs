using System.Collections.Generic;

namespace WendtEquipmentTracking.DataAccess.SQL.Api
{
    public interface IHardwareKitEngine
    {
        IEnumerable<HardwareKit> ListAll();

        HardwareKit Get(Specification<HardwareKit> specification);

        IEnumerable<HardwareKit> List(Specification<HardwareKit> specification);

        int AddNewHardwareKit(HardwareKit hardwareKit);

        void UpdateHardwareKit(HardwareKit hardwareKit);

        void DeleteHardwareKit(HardwareKit hardwareKit);

        void SetDBContext(WendtEquipmentTrackingEntities dbContext);
    }
}
