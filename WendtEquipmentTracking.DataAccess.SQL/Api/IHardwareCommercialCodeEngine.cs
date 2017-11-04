using System.Collections.Generic;
using System.Linq;

namespace WendtEquipmentTracking.DataAccess.SQL.Api
{
    public interface IHardwareCommercialCodeEngine
    {
        IQueryable<HardwareCommercialCode> ListAll();

        HardwareCommercialCode Get(Specification<HardwareCommercialCode> specification);

        IQueryable<HardwareCommercialCode> List(Specification<HardwareCommercialCode> specification);

        void AddNewHardwareCommercialCode(HardwareCommercialCode hardwareCommercialCode);

        void AddAllNewHardwareCommercialCode(IEnumerable<HardwareCommercialCode> hardwareCommercialCodes);

        void UpdateHardwareCommercialCode(HardwareCommercialCode hardwareCommercialCode);

        void DeleteHardwareCommercialCode(HardwareCommercialCode hardwareCommercialCode);

        void SetDBContext(WendtEquipmentTrackingEntities dbContext);
    }
}
