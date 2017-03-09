using System;
using System.Linq.Expressions;

namespace WendtEquipmentTracking.DataAccess.SQL.Specifications.BillOfLadings
{

    internal class ProjectIdSpecification : Specification<BillOfLading>
    {

        private readonly int projectId;

        public ProjectIdSpecification(int projectId)
        {
            this.projectId = projectId;
        }

        public override Expression<Func<BillOfLading, bool>> IsSatisfiedBy()
        {
            return e => e.ProjectId == projectId;
        }
    }
}
