using System.Collections.Generic;
using WendtEquipmentTracking.BusinessLogic.BO;

namespace WendtEquipmentTracking.BusinessLogic.Api
{
    public interface IHardwareKitService
    {
        void Save(HardwareKitBO hardwareKitBO);

        void Update(HardwareKitBO hardwareKitBO);

        void Delete(int id);

        IEnumerable<HardwareKitBO> GetAll(int projectId);

        HardwareKitBO GetById(int id);

        IEnumerable<HardwareKitBO> GetByHardwareKitNumber(int projectId, string hardwareKitNumber);

    }
}