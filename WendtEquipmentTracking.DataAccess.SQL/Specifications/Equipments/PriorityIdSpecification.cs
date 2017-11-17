using System;
using System.Linq.Expressions;

namespace WendtEquipmentTracking.DataAccess.SQL.Specifications.Equipments
{

    internal class PriorityIdSpecification : Specification<Equipment>
    {

        private readonly int priorityId;

        public PriorityIdSpecification(int priorityId)
        {
            this.priorityId = priorityId;
        }

        public override Expression<Func<Equipment, bool>> IsSatisfiedBy()
        {
            return e => e.PriorityId == priorityId;
        }
    }
}
