using System;
using System.Linq.Expressions;

namespace WendtEquipmentTracking.DataAccess.SQL.Specifications.EquipmentAttachments
{

    internal class IdSpecification : Specification<EquipmentAttachment>
    {

        private readonly int id;

        public IdSpecification(int id)
        {
            this.id = id;
        }

        public override Expression<Func<EquipmentAttachment, bool>> IsSatisfiedBy()
        {
            return e => e.EquipmentAttachmentId == id;
        }
    }
}
