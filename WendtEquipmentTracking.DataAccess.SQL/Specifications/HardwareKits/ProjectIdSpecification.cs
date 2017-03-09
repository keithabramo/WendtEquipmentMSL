using System;
using System.Linq.Expressions;

namespace WendtEquipmentTracking.DataAccess.SQL.Specifications.HardwareKits
{

    internal class ProjectIdSpecification : Specification<HardwareKit>
    {

        private readonly int projectId;

        public ProjectIdSpecification(int projectId)
        {
            this.projectId = projectId;
        }

        public override Expression<Func<HardwareKit, bool>> IsSatisfiedBy()
        {
            return e => e.ProjectId == projectId;
        }
    }
}
