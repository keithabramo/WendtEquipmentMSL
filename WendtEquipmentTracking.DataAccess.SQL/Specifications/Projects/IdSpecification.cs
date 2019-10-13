using System;
using System.Linq.Expressions;

namespace WendtEquipmentTracking.DataAccess.SQL.Specifications.Projects
{

    internal class IdSpecification : Specification<Project>
    {

        private readonly int id;

        public IdSpecification(int id)
        {
            this.id = id;
        }

        public override Expression<Func<Project, bool>> IsSatisfiedBy()
        {
            return e => e.ProjectId == id;
        }
    }
}
