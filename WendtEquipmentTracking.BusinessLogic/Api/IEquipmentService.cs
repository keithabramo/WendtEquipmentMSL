using System.Collections.Generic;
using WendtEquipmentTracking.BusinessLogic.BO;

namespace WendtEquipmentTracking.BusinessLogic.Api
{
    public interface IEquipmentService
    {
        void Save(EquipmentBO equipmentBO);
        void SaveAll(IEnumerable<EquipmentBO> equipmentBO);

        void Update(EquipmentBO equipmentBO);

        void Delete(int id);

        IEnumerable<EquipmentBO> GetAll();
        IEnumerable<EquipmentBO> GetByBillOfLadingId(int billOfLadingId);

        EquipmentBO GetById(int id);

    }
}