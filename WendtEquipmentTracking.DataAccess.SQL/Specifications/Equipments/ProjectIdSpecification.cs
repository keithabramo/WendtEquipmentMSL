using System;
using System.Linq.Expressions;

namespace WendtEquipmentTracking.DataAccess.SQL.Specifications.Equipments
{

    internal class ProjectIdSpecification : Specification<Equipment>
    {

        private readonly int projectId;

        public ProjectIdSpecification(int projectId)
        {
            this.projectId = projectId;
        }

        public override Expression<Func<Equipment, bool>> IsSatisfiedBy()
        {
            return e => e.ProjectId == projectId;
        }
    }
}
