using System.Collections.Generic;
using WendtEquipmentTracking.BusinessLogic.BO;

namespace WendtEquipmentTracking.BusinessLogic.Api
{
    public interface IProjectService
    {
        void Save(ProjectBO projectBO);

        void Update(ProjectBO projectBO);

        void Delete(int id);

        IEnumerable<ProjectBO> GetAll();

        IEnumerable<ProjectBO> GetAllForNavigation();

        ProjectBO GetById(int id);

    }
}