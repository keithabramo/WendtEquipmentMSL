using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using WendtEquipmentTracking.BusinessLogic.Api;
using WendtEquipmentTracking.BusinessLogic.BO;
using WendtEquipmentTracking.DataAccess.SQL;
using WendtEquipmentTracking.DataAccess.SQL.Api;
using WendtEquipmentTracking.DataAccess.SQL.Engine;
using WendtEquipmentTracking.DataAccess.SQL.Specifications;

namespace WendtEquipmentTracking.BusinessLogic
{
    public class PriorityService : IPriorityService
    {
        private IPriorityEngine priorityEngine;

        public PriorityService()
        {
            priorityEngine = new PriorityEngine();
        }

        public void Save(PriorityBO priorityBO)
        {
            var priority = Mapper.Map<Priority>(priorityBO);

            priorityEngine.AddNewPriority(priority);
        }

        public void SaveAll(IEnumerable<PriorityBO> priorityBOs)
        {
            var prioritys = Mapper.Map<IEnumerable<Priority>>(priorityBOs);

            priorityEngine.AddAllNewPriority(prioritys);
        }

        public IEnumerable<PriorityBO> GetAll(int projectId)
        {
            var prioritys = priorityEngine.List(PrioritySpecs.ProjectId(projectId)).ToList();

            var priorityBOs = Mapper.Map<IEnumerable<PriorityBO>>(prioritys);

            return priorityBOs;
        }

        public PriorityBO GetById(int id)
        {
            var priority = priorityEngine.Get(PrioritySpecs.Id(id));

            var priorityBO = Mapper.Map<PriorityBO>(priority);

            return priorityBO;
        }

        public void Update(PriorityBO priorityBO)
        {
            var oldPriority = priorityEngine.Get(PrioritySpecs.Id(priorityBO.PriorityId));

            Mapper.Map<PriorityBO, Priority>(priorityBO, oldPriority);

            priorityEngine.UpdatePriority(oldPriority);
        }

        public void Delete(int id)
        {
            var priority = priorityEngine.Get(PrioritySpecs.Id(id));

            if (priority != null)
            {
                priorityEngine.DeletePriority(priority);
            }
        }
    }
}
