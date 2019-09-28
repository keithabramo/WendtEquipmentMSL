using System.Collections.Generic;
using System.Linq;

namespace WendtEquipmentTracking.DataAccess.SQL.Api
{
    public interface IEquipmentEngine
    {
        IQueryable<Equipment> ListAll();

        Equipment Get(Specification<Equipment> specification);

        IQueryable<Equipment> List(Specification<Equipment> specification);

        void AddNewEquipment(Equipment equipment);

        void AddAllNewEquipment(IEnumerable<Equipment> equipments);

        void UpdateEquipment(Equipment equipment);

        void UpdateEquipments(IEnumerable<Equipment> equipments);

        void DeleteEquipments(IEnumerable<Equipment> equipments);

        void SetDBContext(WendtEquipmentTrackingEntities dbContext);
    }
}
