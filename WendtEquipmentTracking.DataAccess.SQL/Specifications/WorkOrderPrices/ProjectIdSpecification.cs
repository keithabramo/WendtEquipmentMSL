using System;
using System.Linq.Expressions;

namespace WendtEquipmentTracking.DataAccess.SQL.Specifications.WorkOrderPrices
{

    internal class ProjectIdSpecification : Specification<WorkOrderPrice>
    {

        private readonly int projectId;

        public ProjectIdSpecification(int projectId)
        {
            this.projectId = projectId;
        }

        public override Expression<Func<WorkOrderPrice, bool>> IsSatisfiedBy()
        {
            return e => e.ProjectId == projectId;
        }
    }
}
