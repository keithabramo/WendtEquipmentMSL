using System.Collections.Generic;
using WendtEquipmentTracking.BusinessLogic.BO;

namespace WendtEquipmentTracking.BusinessLogic.Api
{
    public interface IHardwareCommercialCodeService
    {
        void Save(HardwareCommercialCodeBO hardwareCommercialCodeBO);

        void SaveAll(IEnumerable<HardwareCommercialCodeBO> hardwareCommercialCodeBOs);

        void Update(HardwareCommercialCodeBO hardwareCommercialCodeBO);

        void Delete(int id);

        IEnumerable<HardwareCommercialCodeBO> GetAll();

        HardwareCommercialCodeBO GetById(int id);

    }
}