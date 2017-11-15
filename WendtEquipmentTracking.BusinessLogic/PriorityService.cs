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
        private WendtEquipmentTrackingEntities dbContext;
        private IPriorityEngine priorityEngine;
        private IEquipmentEngine equipmentEngine;

        public PriorityService()
        {
            dbContext = new WendtEquipmentTrackingEntities();
            priorityEngine = new PriorityEngine(dbContext);
            equipmentEngine = new EquipmentEngine(dbContext);
        }

        public void Save(PriorityBO priorityBO)
        {
            var priority = new Priority
            {
                DueDate = priorityBO.DueDate,
                EquipmentName = priorityBO.EquipmentName,
                PriorityId = priorityBO.PriorityId,
                PriorityNumber = priorityBO.PriorityNumber,
                ProjectId = priorityBO.ProjectId
            };

            priorityEngine.AddNewPriority(priority);

            dbContext.SaveChanges();
        }

        public void SaveAll(IEnumerable<PriorityBO> priorityBOs)
        {
            var prioritys = priorityBOs.Select(x => new Priority
            {
                DueDate = x.DueDate,
                EquipmentName = x.EquipmentName,
                PriorityNumber = x.PriorityNumber,
                ProjectId = x.ProjectId
            });

            priorityEngine.AddNewPrioritys(prioritys);

            dbContext.SaveChanges();
        }

        public IEnumerable<PriorityBO> GetAll(int projectId)
        {
            var prioritys = priorityEngine.List(PrioritySpecs.ProjectId(projectId));

            var priorityBOs = prioritys.Select(x => new PriorityBO
            {
                DueDate = x.DueDate,
                EquipmentName = x.EquipmentName,
                PriorityId = x.PriorityId,
                PriorityNumber = x.PriorityNumber,
                ProjectId = x.ProjectId
            });

            return priorityBOs.ToList();
        }

        public PriorityBO GetById(int id)
        {
            var priority = priorityEngine.Get(PrioritySpecs.Id(id));

            var priorityBO = new PriorityBO
            {
                DueDate = priority.DueDate,
                EquipmentName = priority.EquipmentName,
                PriorityId = priority.PriorityId,
                PriorityNumber = priority.PriorityNumber,
                ProjectId = priority.ProjectId
            };

            return priorityBO;
        }

        public void Update(PriorityBO priorityBO)
        {
            var oldPriority = priorityEngine.Get(PrioritySpecs.Id(priorityBO.PriorityId));
            var oldPriorityNumber = oldPriority.PriorityNumber;

            oldPriority.DueDate = priorityBO.DueDate;
            oldPriority.EquipmentName = priorityBO.EquipmentName;
            oldPriority.PriorityNumber = priorityBO.PriorityNumber;

            priorityEngine.UpdatePriority(oldPriority);


            var equipments = equipmentEngine.List(EquipmentSpecs.ProjectId(priorityBO.ProjectId) && EquipmentSpecs.Priority(oldPriorityNumber)).ToList();
            foreach (var equipment in equipments)
            {
                equipment.Priority = priorityBO.PriorityNumber;
                equipmentEngine.UpdateEquipment(equipment);
            }


            dbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            var priority = priorityEngine.Get(PrioritySpecs.Id(id));

            if (priority != null)
            {
                priorityEngine.DeletePriority(priority);
            }

            dbContext.SaveChanges();
        }
    }
}
