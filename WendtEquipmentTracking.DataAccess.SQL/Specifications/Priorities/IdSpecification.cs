using System;
using System.Linq.Expressions;

namespace WendtEquipmentTracking.DataAccess.SQL.Specifications.Priorities
{

    internal class IdSpecification : Specification<Priority>
    {

        private readonly int id;

        public IdSpecification(int id)
        {
            this.id = id;
        }

        public override Expression<Func<Priority, bool>> IsSatisfiedBy()
        {
            return e => e.PriorityId == id;
        }
    }
}
