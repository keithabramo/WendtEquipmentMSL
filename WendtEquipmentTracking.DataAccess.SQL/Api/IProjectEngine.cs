using System.Collections.Generic;

namespace WendtEquipmentTracking.DataAccess.SQL.Api
{
    public interface IProjectEngine
    {
        IEnumerable<Project> ListAll();

        IEnumerable<Project> ListAllLazy();

        Project Get(Specification<Project> specification);

        IEnumerable<Project> List(Specification<Project> specification);

        void AddNewProject(Project project);

        void UpdateProject(Project project);

        void DeleteProject(Project project);

        void SetDBContext(WendtEquipmentTrackingEntities dbContext);
    }
}
