using System;
using System.Linq.Expressions;

namespace WendtEquipmentTracking.DataAccess.SQL.Specifications.Projects
{

    internal class ProjectNumberSpecification : Specification<Project> {

        private readonly double projectNumber;

        public ProjectNumberSpecification(double projectNumber) {
            this.projectNumber = projectNumber;
        }

        public override Expression<Func<Project, bool>> IsSatisfiedBy()
        {
            return e => e.ProjectNumber == projectNumber;
        }
    }
}
