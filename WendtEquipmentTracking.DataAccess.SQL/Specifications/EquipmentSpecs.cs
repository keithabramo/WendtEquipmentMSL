using WendtEquipmentTracking.DataAccess.SQL.Specifications.Equipments;

namespace WendtEquipmentTracking.DataAccess.SQL.Specifications
{

    public static class EquipmentSpecs {
        public static Specification<Equipment> Id(int id) {
            return new IdSpecification(id);
        }
    }
}
