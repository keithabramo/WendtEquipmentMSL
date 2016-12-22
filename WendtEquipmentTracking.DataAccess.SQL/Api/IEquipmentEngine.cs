using System.Collections.Generic;

namespace WendtEquipmentTracking.DataAccess.SQL.Api
{
    public interface IEquipmentEngine
    {
        IEnumerable<Equipment> ListAll();

        Equipment Get(Specification<Equipment> specification);

        IEnumerable<Equipment> List(Specification<Equipment> specification);

        void AddNewEquipment(Equipment equipment);

        void UpdateEquipment(Equipment equipment);

        void DeleteEquipment(Equipment equipment);

        void SetDBContext(WendtEquipmentTrackingEntities dbContext);
    }
}
