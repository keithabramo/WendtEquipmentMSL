using System;
using System.Linq.Expressions;

namespace WendtEquipmentTracking.DataAccess.SQL.Specifications.HardwareCommercialCodes
{

    internal class IsDeletedSpecification : Specification<HardwareCommercialCode>
    {

        public override Expression<Func<HardwareCommercialCode, bool>> IsSatisfiedBy()
        {
            return e => e.IsDeleted;
        }
    }
}
