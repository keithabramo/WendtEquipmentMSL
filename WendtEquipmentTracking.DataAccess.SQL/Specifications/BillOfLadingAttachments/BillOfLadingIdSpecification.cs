using System;
using System.Linq.Expressions;

namespace WendtEquipmentTracking.DataAccess.SQL.Specifications.BillOfLadingAttachments
{

    internal class BillOfLadingIdSpecification : Specification<BillOfLadingAttachment>
    {

        private readonly int billOfLadingId;

        public BillOfLadingIdSpecification(int billOfLadingId)
        {
            this.billOfLadingId = billOfLadingId;
        }

        public override Expression<Func<BillOfLadingAttachment, bool>> IsSatisfiedBy()
        {
            return e => e.BillOfLadingId == billOfLadingId;
        }
    }
}
