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

        void SetDBContext(WendtEquipmentTrackingEntities dbContext);
    }
}
