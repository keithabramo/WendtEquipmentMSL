using System;
using System.Linq.Expressions;

namespace WendtEquipmentTracking.DataAccess.SQL.Specifications.Projects
{

    internal class IsDeletedSpecification : Specification<Project>
    {

        public override Expression<Func<Project, bool>> IsSatisfiedBy()
        {
            return e => e.IsDeleted;
        }
    }
}
