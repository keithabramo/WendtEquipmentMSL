using System.Collections.Generic;
using WendtEquipmentTracking.BusinessLogic.BO;

namespace WendtEquipmentTracking.BusinessLogic.Api
{
    public interface IEquipmentService
    {
        IEnumerable<int> SaveAll(IEnumerable<EquipmentBO> equipmentBO);
        void UpdateAll(IEnumerable<EquipmentBO> equipmentBO);
        void UpdateRevisions(IEnumerable<EquipmentBO> equipmentBOs);

        void DeleteAll(IEnumerable<int> ids);
        IEnumerable<EquipmentBO> GetAll(int projectId);
        IEnumerable<EquipmentBO> GetForDuplicateCheck(int projectId);
        IEnumerable<EquipmentBO> GetByIds(IEnumerable<int> ids);
        IEnumerable<EquipmentBO> GetByDrawingNumbers(int projectId, IEnumerable<string> drawingNumbers);
        IEnumerable<EquipmentBO> GetHardwareByShippingTagNumberAndDescription(int projectId, string shipTagNumber, string description);
        IEnumerable<EquipmentBO> GetHardwareByShippingTagNumberAndDescription(int projectId, string shipTagNumber, string description, int? hardwareKitId);

    }
}