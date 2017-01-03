using System;
using System.Linq.Expressions;

namespace WendtEquipmentTracking.DataAccess.SQL.Specifications.BillOfLandings
{

    internal class IdSpecification : Specification<BillOfLanding>
    {

        private readonly int id;

        public IdSpecification(int id)
        {
            this.id = id;
        }

        public override Expression<Func<BillOfLanding, bool>> IsSatisfiedBy()
        {
            return e => e.BillOfLandingId == id;
        }
    }
}
