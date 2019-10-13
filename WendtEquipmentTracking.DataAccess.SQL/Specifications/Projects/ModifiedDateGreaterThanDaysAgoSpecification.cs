using System;
using System.Linq.Expressions;

namespace WendtEquipmentTracking.DataAccess.SQL.Specifications.Projects
{

    internal class ModifiedDateGreaterThanDaysAgoSpecification : Specification<Project>
    {
        private readonly DateTime expiredDate;

        public ModifiedDateGreaterThanDaysAgoSpecification(int days)
        {
            this.expiredDate = DateTime.Now.Date.AddDays(-1 * days);
        }

        public override Expression<Func<Project, bool>> IsSatisfiedBy()
        {
            return e => e.ModifiedDate < expiredDate;
        }
    }
}
