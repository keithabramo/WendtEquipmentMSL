using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace WendtEquipmentTracking.DataAccess.SQL.Specifications.Vendors
{

    internal class IdsSpecification : Specification<Vendor>
    {

        private readonly IEnumerable<int> ids;

        public IdsSpecification(IEnumerable<int> ids)
        {
            this.ids = ids;
        }

        public override Expression<Func<Vendor, bool>> IsSatisfiedBy()
        {
            return e => ids.Contains(e.VendorId);
        }
    }
}
