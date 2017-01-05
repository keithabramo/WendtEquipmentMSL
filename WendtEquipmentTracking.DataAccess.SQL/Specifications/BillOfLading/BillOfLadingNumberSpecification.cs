using System;
using System.Linq.Expressions;

namespace WendtEquipmentTracking.DataAccess.SQL.Specifications.BillOfLadings
{

    internal class BillOfLadingNumberSpecification : Specification<BillOfLading>
    {

        private readonly string billOfLadingNumber;

        public BillOfLadingNumberSpecification(string billOfLadingNumber)
        {
            this.billOfLadingNumber = billOfLadingNumber;
        }

        public override Expression<Func<BillOfLading, bool>> IsSatisfiedBy()
        {
            return e => e.BillOfLadingNumber == billOfLadingNumber;
        }
    }
}
