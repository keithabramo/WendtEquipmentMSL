using System;
using System.Linq.Expressions;

namespace WendtEquipmentTracking.DataAccess.SQL.Specifications.Priorities
{

    internal class ProjectIdSpecification : Specification<Priority>
    {

        private readonly int projectId;

        public ProjectIdSpecification(int projectId)
        {
            this.projectId = projectId;
        }

        public override Expression<Func<Priority, bool>> IsSatisfiedBy()
        {
            return e => e.ProjectId == projectId;
        }
    }
}
