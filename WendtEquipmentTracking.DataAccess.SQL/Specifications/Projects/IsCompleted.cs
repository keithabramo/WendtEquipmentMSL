using System;
using System.Linq.Expressions;

namespace WendtEquipmentTracking.DataAccess.SQL.Specifications.Projects
{

    internal class IsCompletedSpecification : Specification<Project>
    {

        public override Expression<Func<Project, bool>> IsSatisfiedBy()
        {
            return e => e.IsCompleted;
        }
    }
}
