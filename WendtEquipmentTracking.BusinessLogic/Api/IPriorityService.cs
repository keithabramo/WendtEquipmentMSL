using System.Collections.Generic;
using WendtEquipmentTracking.BusinessLogic.BO;

namespace WendtEquipmentTracking.BusinessLogic.Api
{
    public interface IPriorityService
    {
        void Save(PriorityBO priorityBO);

        void SaveAll(IEnumerable<PriorityBO> priorityBOs);

        void Update(PriorityBO priorityBO);

        void UpdateAll(IEnumerable<PriorityBO> priorityBOs);

        void Delete(int id);

        IEnumerable<PriorityBO> GetAll(int projectId);

        PriorityBO GetById(int id);

    }
}