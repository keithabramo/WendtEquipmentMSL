using System.Collections.Generic;

namespace WendtEquipmentTracking.DataAccess.SQL.Api
{
    public interface IHardwareCommercialCodeEngine
    {
        IEnumerable<HardwareCommercialCode> ListAll();

        HardwareCommercialCode Get(Specification<HardwareCommercialCode> specification);

        IEnumerable<HardwareCommercialCode> List(Specification<HardwareCommercialCode> specification);

        void AddNewHardwareCommercialCode(HardwareCommercialCode hardwareCommercialCode);

        void AddAllNewHardwareCommercialCode(IEnumerable<HardwareCommercialCode> hardwareCommercialCodes);

        void UpdateHardwareCommercialCode(HardwareCommercialCode hardwareCommercialCode);

        void DeleteHardwareCommercialCode(HardwareCommercialCode hardwareCommercialCode);

        void SetDBContext(WendtEquipmentTrackingEntities dbContext);
    }
}
