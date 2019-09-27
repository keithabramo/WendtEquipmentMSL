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

        IEnumerable<ProjectBO> GetAllForCopy();

        IEnumerable<ProjectBO> GetDeletedAndCompleted();


        ProjectBO GetById(int id);

        ProjectBO GetByProjectNumber(double projectNumber);

        void Undelete(int id);

        void Uncomplete(int id);

    }
}