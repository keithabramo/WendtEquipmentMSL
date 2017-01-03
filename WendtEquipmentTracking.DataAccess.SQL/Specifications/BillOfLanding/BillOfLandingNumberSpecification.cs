using System;
using System.Linq.Expressions;

namespace WendtEquipmentTracking.DataAccess.SQL.Specifications.BillOfLandings
{

    internal class BillOfLandingNumberSpecification : Specification<BillOfLanding>
    {

        private readonly string billOfLandingNumber;

        public BillOfLandingNumberSpecification(string billOfLandingNumber)
        {
            this.billOfLandingNumber = billOfLandingNumber;
        }

        public override Expression<Func<BillOfLanding, bool>> IsSatisfiedBy()
        {
            return e => e.BillOfLandingNumber == billOfLandingNumber;
        }
    }
}
