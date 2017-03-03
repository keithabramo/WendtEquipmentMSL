using System;
using System.Linq.Expressions;

namespace WendtEquipmentTracking.DataAccess.SQL.Specifications.HardwareCommercialCodes
{

    internal class IdSpecification : Specification<HardwareCommercialCode>
    {

        private readonly int id;

        public IdSpecification(int id)
        {
            this.id = id;
        }

        public override Expression<Func<HardwareCommercialCode, bool>> IsSatisfiedBy()
        {
            return e => e.HardwareCommercialCodeId == id;
        }
    }
}
