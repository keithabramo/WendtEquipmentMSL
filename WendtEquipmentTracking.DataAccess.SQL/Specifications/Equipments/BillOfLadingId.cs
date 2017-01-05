using System;
using System.Linq;
using System.Linq.Expressions;

namespace WendtEquipmentTracking.DataAccess.SQL.Specifications.Equipments
{

    internal class BillOfLadingIdSpecification : Specification<Equipment>
    {

        private readonly int billOfLadingId;

        public BillOfLadingIdSpecification(int billOfLadingId)
        {
            this.billOfLadingId = billOfLadingId;
        }

        public override Expression<Func<Equipment, bool>> IsSatisfiedBy()
        {
            return e => e.BillOfLadingEquipments.Any(be => be.BillOfLadingId == billOfLadingId);
        }
    }
}
