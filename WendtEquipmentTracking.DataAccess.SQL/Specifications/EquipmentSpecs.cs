using System.Collections.Generic;
using WendtEquipmentTracking.DataAccess.SQL.Specifications.Equipments;

namespace WendtEquipmentTracking.DataAccess.SQL.Specifications
{

    public static class EquipmentSpecs
    {
        public static Specification<Equipment> Id(int id)
        {
            return new IdSpecification(id);
        }

        public static Specification<Equipment> Ids(IEnumerable<int> ids)
        {
            return new IdsSpecification(ids);
        }

        public static Specification<Equipment> BillOfLadingId(int billOfLadingId)
        {
            return new BillOfLadingIdSpecification(billOfLadingId);
        }

        public static Specification<Equipment> IsDeleted()
        {
            return new IsDeletedSpecification();
        }

        public static Specification<Equipment> IsHardware()
        {
            return new IsHardwareSpecification();
        }

        public static Specification<Equipment> IsAttachedToHardwareKit()
        {
            return new IsAttachedToHardwareKitSpecification();
        }

        public static Specification<Equipment> ShippingTagNumber(string workOrderNumber)
        {
            return new ShippingTagNumberSpecification(workOrderNumber);
        }

        public static Specification<Equipment> ProjectId(int projectId)
        {
            return new ProjectIdSpecification(projectId);
        }

        public static Specification<Equipment> HardwareKitId(int hardwareKitId)
        {
            return new HardwareKitIdSpecification(hardwareKitId);
        }
    }
}
