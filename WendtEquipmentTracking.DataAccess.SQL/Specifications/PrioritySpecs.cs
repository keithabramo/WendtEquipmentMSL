using System.Collections.Generic;
using WendtEquipmentTracking.DataAccess.SQL.Specifications.Priorities;

namespace WendtEquipmentTracking.DataAccess.SQL.Specifications
{

    public static class PrioritySpecs
    {
        public static Specification<Priority> Id(int id)
        {
            return new IdSpecification(id);
        }

        public static Specification<Priority> Ids(IEnumerable<int> ids)
        {
            return new IdsSpecification(ids);
        }

        public static Specification<Priority> IsDeleted()
        {
            return new IsDeletedSpecification();
        }

        public static Specification<Priority> ProjectId(int projectId)
        {
            return new ProjectIdSpecification(projectId);
        }
    }
}
