using System;
using System.Linq.Expressions;

namespace WendtEquipmentTracking.DataAccess.SQL.Specifications.Vendors
{

    internal class IdSpecification : Specification<Vendor>
    {

        private readonly int id;

        public IdSpecification(int id)
        {
            this.id = id;
        }

        public override Expression<Func<Vendor, bool>> IsSatisfiedBy()
        {
            return e => e.VendorId == id;
        }
    }
}
