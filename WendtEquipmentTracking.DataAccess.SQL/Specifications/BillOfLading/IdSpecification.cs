using System;
using System.Linq.Expressions;

namespace WendtEquipmentTracking.DataAccess.SQL.Specifications.BillOfLadings
{

    internal class IdSpecification : Specification<BillOfLading>
    {

        private readonly int id;

        public IdSpecification(int id)
        {
            this.id = id;
        }

        public override Expression<Func<BillOfLading, bool>> IsSatisfiedBy()
        {
            return e => e.BillOfLadingId == id;
        }
    }
}
