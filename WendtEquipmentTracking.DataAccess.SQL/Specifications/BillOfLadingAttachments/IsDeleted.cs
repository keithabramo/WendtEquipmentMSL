using System;
using System.Linq.Expressions;

namespace WendtEquipmentTracking.DataAccess.SQL.Specifications.BillOfLadingAttachments
{

    internal class IsDeletedSpecification : Specification<BillOfLadingAttachment>
    {

        public override Expression<Func<BillOfLadingAttachment, bool>> IsSatisfiedBy()
        {
            return e => e.IsDeleted;
        }
    }
}
