﻿using System.Collections.Generic;
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
                EndDate = priorityBO.EndDate,
                ContractualShipDate = priorityBO.ContractualShipDate,
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
                EndDate = x.EndDate,
                ContractualShipDate = x.ContractualShipDate,
                EquipmentName = x.EquipmentName,
                PriorityNumber = x.PriorityNumber,
                ProjectId = x.ProjectId
            }).ToList();

            priorityEngine.AddNewPrioritys(prioritys);

            dbContext.SaveChanges();
        }

        public IEnumerable<PriorityBO> GetAll(int projectId)
        {
            var prioritys = priorityEngine.List(PrioritySpecs.ProjectId(projectId));

            var priorityBOs = prioritys.Select(x => new PriorityBO
            {
                DueDate = x.DueDate,
                EndDate = x.EndDate,
                ContractualShipDate = x.ContractualShipDate,
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
                EndDate = priority.EndDate,
                ContractualShipDate = priority.ContractualShipDate,
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
            oldPriority.EndDate = priorityBO.EndDate;
            oldPriority.ContractualShipDate = priorityBO.ContractualShipDate;
            oldPriority.EquipmentName = priorityBO.EquipmentName;
            oldPriority.PriorityNumber = priorityBO.PriorityNumber;

            priorityEngine.UpdatePriority(oldPriority);


            dbContext.SaveChanges();
        }

        public void UpdateAll(IEnumerable<PriorityBO> priorityBOs)
        {
            //Performance Issue?
            var oldPriorities = priorityEngine.List(PrioritySpecs.Ids(priorityBOs.Select(x => x.PriorityId))).ToList();

            foreach (var oldPriority in oldPriorities)
            {
                var priorityBO = priorityBOs.FirstOrDefault(x => x.PriorityId == oldPriority.PriorityId);

                if (priorityBO != null)
                {
                    oldPriority.DueDate = priorityBO.DueDate;
                    oldPriority.EndDate = priorityBO.EndDate;
                    oldPriority.ContractualShipDate = priorityBO.ContractualShipDate;
                    oldPriority.EquipmentName = priorityBO.EquipmentName;
                    oldPriority.PriorityNumber = priorityBO.PriorityNumber;
                }


            }

            priorityEngine.UpdatePriorities(oldPriorities);

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
