using WendtEquipmentTracking.DataAccess.SQL.Specifications.BillOfLadings;

namespace WendtEquipmentTracking.DataAccess.SQL.Specifications
{

    public static class BillOfLadingSpecs
    {
        public static Specification<BillOfLading> Id(int id)
        {
            return new IdSpecification(id);
        }

        public static Specification<BillOfLading> ProjectId(int projectId)
        {
            return new ProjectIdSpecification(projectId);
        }

        public static Specification<BillOfLading> BillOfLadingNumber(string billOfLandningNumber)
        {
            return new BillOfLadingNumberSpecification(billOfLandningNumber);
        }

        public static Specification<BillOfLading> CurrentRevision()
        {
            return new CurrentRevisionSpecification();
        }

        public static Specification<BillOfLading> IsDeleted()
        {
            return new IsDeletedSpecification();
        }
    }
}
