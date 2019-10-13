using System;
using System.Linq.Expressions;

namespace WendtEquipmentTracking.DataAccess.SQL.Specifications.EquipmentAttachments
{

    internal class IsDeletedSpecification : Specification<EquipmentAttachment>
    {

        public override Expression<Func<EquipmentAttachment, bool>> IsSatisfiedBy()
        {
            return e => e.IsDeleted;
        }
    }
}
