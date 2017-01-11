using System;
using System.Linq.Expressions;

namespace WendtEquipmentTracking.DataAccess.SQL.Specifications.Equipments
{

    internal class IsHardwareSpecification : Specification<Equipment>
    {

        public override Expression<Func<Equipment, bool>> IsSatisfiedBy()
        {
            return e => e.IsHardware;
        }
    }
}
