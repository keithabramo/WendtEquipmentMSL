using System;
using System.Linq.Expressions;

namespace WendtEquipmentTracking.DataAccess.SQL.Specifications.Equipments
{

    internal class PrioritySpecification : Specification<Equipment>
    {

        private readonly int priority;

        public PrioritySpecification(int priority)
        {
            this.priority = priority;
        }

        public override Expression<Func<Equipment, bool>> IsSatisfiedBy()
        {
            return e => e.Priority == priority;
        }
    }
}
