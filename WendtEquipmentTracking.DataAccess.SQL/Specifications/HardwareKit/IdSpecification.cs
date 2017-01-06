using System;
using System.Linq.Expressions;

namespace WendtEquipmentTracking.DataAccess.SQL.Specifications.HardwareKits
{

    internal class IdSpecification : Specification<HardwareKit>
    {

        private readonly int id;

        public IdSpecification(int id)
        {
            this.id = id;
        }

        public override Expression<Func<HardwareKit, bool>> IsSatisfiedBy()
        {
            return e => e.HardwareKitId == id;
        }
    }
}
