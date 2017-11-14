using WendtEquipmentTracking.DataAccess.SQL.Specifications.Projects;

namespace WendtEquipmentTracking.DataAccess.SQL.Specifications
{

    public static class ProjectSpecs
    {
        public static Specification<Project> Id(int id)
        {
            return new IdSpecification(id);
        }

        public static Specification<Project> IsDeleted()
        {
            return new IsDeletedSpecification();
        }

        public static Specification<Project> IsCompleted()
        {
            return new IsCompletedSpecification();
        }
    }
}
