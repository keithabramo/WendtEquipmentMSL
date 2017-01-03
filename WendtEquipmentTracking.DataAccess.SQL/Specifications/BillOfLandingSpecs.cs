using WendtEquipmentTracking.DataAccess.SQL.Specifications.BillOfLandings;

namespace WendtEquipmentTracking.DataAccess.SQL.Specifications
{

    public static class BillOfLandingSpecs
    {
        public static Specification<BillOfLanding> Id(int id)
        {
            return new IdSpecification(id);
        }

        public static Specification<BillOfLanding> BillOfLandingNumber(string billOfLandningNumber)
        {
            return new BillOfLandingNumberSpecification(billOfLandningNumber);
        }

        public static Specification<BillOfLanding> CurrentRevision()
        {
            return new CurrentRevisionSpecification();
        }
    }
}
