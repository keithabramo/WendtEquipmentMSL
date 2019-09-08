using System;
using System.Linq.Expressions;

namespace WendtEquipmentTracking.DataAccess.SQL.Specifications.Vendors
{

    internal class IsDeletedSpecification : Specification<Vendor>
    {

        public override Expression<Func<Vendor, bool>> IsSatisfiedBy()
        {
            return e => e.IsDeleted;
        }
    }
}
