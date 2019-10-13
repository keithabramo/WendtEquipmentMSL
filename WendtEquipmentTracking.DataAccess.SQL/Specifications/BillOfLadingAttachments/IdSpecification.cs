using System;
using System.Linq.Expressions;

namespace WendtEquipmentTracking.DataAccess.SQL.Specifications.BillOfLadingAttachments
{

    internal class IdSpecification : Specification<BillOfLadingAttachment>
    {

        private readonly int id;

        public IdSpecification(int id)
        {
            this.id = id;
        }

        public override Expression<Func<BillOfLadingAttachment, bool>> IsSatisfiedBy()
        {
            return e => e.BillOfLadingAttachmentId == id;
        }
    }
}
