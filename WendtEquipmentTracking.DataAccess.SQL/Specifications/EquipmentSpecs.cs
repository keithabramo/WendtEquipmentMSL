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

        public static Specification<Equipment> DrawingNumbers(IEnumerable<string> drawingNumbers)
        {
            return new DrawingNumbersSpecification(drawingNumbers);
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

        public static Specification<Equipment> ShippingTagNumber(string shippingTageNumber)
        {
            return new ShippingTagNumberSpecification(shippingTageNumber);
        }

        public static Specification<Equipment> Description(string description)
        {
            return new DescriptionSpecification(description);
        }

        public static Specification<Equipment> ProjectId(int projectId)
        {
            return new ProjectIdSpecification(projectId);
        }

        // Checks if this equipment is one of the hardware kit equipments
        public static Specification<Equipment> IsAssociatedToHardwareKit(int hardwareKitId)
        {
            return new IsAssociatedToHardwareKitSpecification(hardwareKitId);
        }

        // Checks if this equipment is the actual hardware kit equipment record 
        // Where equipment.hardwarekitid points to the hardware kit record itself
        public static Specification<Equipment> HardwareKitId(int hardwareKitId)
        {
            return new HardwareKitIdSpecification(hardwareKitId);
        }

        public static Specification<Equipment> PriorityId(int priorityId)
        {
            return new PriorityIdSpecification(priorityId);
        }

    }
}
