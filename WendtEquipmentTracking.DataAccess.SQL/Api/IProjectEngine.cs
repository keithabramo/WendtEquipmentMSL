using System.Linq;

namespace WendtEquipmentTracking.DataAccess.SQL.Api
{
    public interface IProjectEngine
    {
        IQueryable<Project> ListAll();


        Project Get(Specification<Project> specification);

        IQueryable<Project> List(Specification<Project> specification);

        void AddNewProject(Project project);

        void UpdateProject(Project project);

        void DeleteProject(Project project);

        IQueryable<Project> ListRaw(Specification<Project> specification);

        Project GetRaw(Specification<Project> specification);

        void UndeleteProject(Project project);

        void UncompleteProject(Project project);

        void SetDBContext(WendtEquipmentTrackingEntities dbContext);
    }
}
