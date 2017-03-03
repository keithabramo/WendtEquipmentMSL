using WendtEquipmentTracking.DataAccess.SQL.Specifications.HardwareCommercialCodes;

namespace WendtEquipmentTracking.DataAccess.SQL.Specifications
{

    public static class HardwareCommercialCodeSpecs
    {
        public static Specification<HardwareCommercialCode> Id(int id)
        {
            return new IdSpecification(id);
        }

        public static Specification<HardwareCommercialCode> IsDeleted()
        {
            return new IsDeletedSpecification();
        }
    }
}
