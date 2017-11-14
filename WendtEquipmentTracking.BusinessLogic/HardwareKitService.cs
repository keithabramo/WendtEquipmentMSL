
using System;
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
    public class HardwareKitService : IHardwareKitService
    {
        private WendtEquipmentTrackingEntities dbContext;
        private IHardwareKitEngine hardwareKitEngine;
        private IEquipmentEngine equipmentEngine;
        private IProjectEngine projectEngine;
        private IPriorityEngine priorityEngine;


        public HardwareKitService()
        {
            dbContext = new WendtEquipmentTrackingEntities();
            hardwareKitEngine = new HardwareKitEngine(dbContext);
            equipmentEngine = new EquipmentEngine(dbContext);
            projectEngine = new ProjectEngine(dbContext);
            priorityEngine = new PriorityEngine(dbContext);
        }

        public void Save(HardwareKitBO hardwareKitBO)
        {
            var hardwareKit = new HardwareKit
            {
                ExtraQuantityPercentage = hardwareKitBO.ExtraQuantityPercentage,
                HardwareKitId = hardwareKitBO.HardwareKitId,
                HardwareKitNumber = hardwareKitBO.HardwareKitNumber,
                IsCurrentRevision = hardwareKitBO.IsCurrentRevision,
                ProjectId = hardwareKitBO.ProjectId,
                Revision = hardwareKitBO.Revision,
                HardwareKitEquipments = hardwareKitBO.HardwareKitEquipments.Select(x => new HardwareKitEquipment
                {
                    EquipmentId = x.EquipmentId,
                    HardwareKitEquipmentId = x.HardwareKitEquipmentId,
                    HardwareKitId = x.HardwareKitId,
                    QuantityToShip = x.QuantityToShip
                }).ToList()
            };

            hardwareKitEngine.AddNewHardwareKit(hardwareKit);

            dbContext.SaveChanges();

            var project = projectEngine.Get(ProjectSpecs.Id(hardwareKitBO.ProjectId));
            var priority = priorityEngine.ListAll().FirstOrDefault();

            var equipment = new Equipment();
            equipment.HardwareKitId = hardwareKit.HardwareKitId;
            equipment.EquipmentName = "Hardware Kit " + hardwareKitBO.HardwareKitNumber;
            equipment.Description = "Misc";
            equipment.DrawingNumber = project.ProjectNumber.ToString();
            equipment.Priority = priority.PriorityNumber;
            equipment.ShippingTagNumber = hardwareKitBO.HardwareKitNumber;
            equipment.UnitWeight = 1;
            equipment.ProjectId = hardwareKitBO.ProjectId;
            equipment.Quantity = 1; //hardwareKitBO.HardwareKitEquipments.Sum(hke => hke.Quantity);
            equipment.ReleaseDate = DateTime.Now;

            equipmentEngine.AddNewEquipment(equipment);

            dbContext.SaveChanges();
        }

        public IEnumerable<HardwareKitBO> GetAll(int projectId)
        {
            var hardwareKits = hardwareKitEngine.List(HardwareKitSpecs.ProjectId(projectId));

            var hardwareKitBOs = hardwareKits.Select(x => new HardwareKitBO
            {
                ExtraQuantityPercentage = x.ExtraQuantityPercentage,
                HardwareKitId = x.HardwareKitId,
                HardwareKitNumber = x.HardwareKitNumber,
                IsCurrentRevision = x.IsCurrentRevision,
                ProjectId = x.ProjectId,
                Revision = x.Revision,
                HardwareKitEquipments = x.HardwareKitEquipments.Where(e => !e.Equipment.IsDeleted).Select(e => new HardwareKitEquipmentBO
                {
                    EquipmentId = e.EquipmentId,
                    HardwareKitEquipmentId = e.HardwareKitEquipmentId,
                    HardwareKitId = e.HardwareKitId,
                    QuantityToShip = e.QuantityToShip
                }).ToList()
            });


            return hardwareKitBOs.ToList();
        }

        public HardwareKitBO GetById(int id)
        {
            var hardwareKit = hardwareKitEngine.Get(HardwareKitSpecs.Id(id));

            var hardwareKitBO = new HardwareKitBO
            {
                ExtraQuantityPercentage = hardwareKit.ExtraQuantityPercentage,
                HardwareKitId = hardwareKit.HardwareKitId,
                HardwareKitNumber = hardwareKit.HardwareKitNumber,
                IsCurrentRevision = hardwareKit.IsCurrentRevision,
                ProjectId = hardwareKit.ProjectId,
                Revision = hardwareKit.Revision,
                HardwareKitEquipments = hardwareKit.HardwareKitEquipments.Select(e => new HardwareKitEquipmentBO
                {
                    EquipmentId = e.EquipmentId,
                    HardwareKitEquipmentId = e.HardwareKitEquipmentId,
                    HardwareKitId = e.HardwareKitId,
                    QuantityToShip = e.QuantityToShip,
                    Equipment = new EquipmentBO
                    {
                        Description = e.Equipment.Description,
                        ShippingTagNumber = e.Equipment.ShippingTagNumber,
                        Quantity = e.Equipment.Quantity,
                    }
                }).ToList()
            };

            return hardwareKitBO;
        }

        public IEnumerable<HardwareKitBO> GetCurrentByProject(int projectId)
        {
            var hardwareKits = hardwareKitEngine.List(HardwareKitSpecs.ProjectId(projectId) && HardwareKitSpecs.CurrentRevision());

            var hardwareKitBOs = hardwareKits.Select(x => new HardwareKitBO
            {
                ExtraQuantityPercentage = x.ExtraQuantityPercentage,
                HardwareKitId = x.HardwareKitId,
                HardwareKitNumber = x.HardwareKitNumber,
                IsCurrentRevision = x.IsCurrentRevision,
                ProjectId = x.ProjectId,
                Revision = x.Revision,
                HardwareKitEquipments = x.HardwareKitEquipments.Where(e => !e.Equipment.IsDeleted).Select(e => new HardwareKitEquipmentBO
                {
                    EquipmentId = e.EquipmentId,
                    HardwareKitEquipmentId = e.HardwareKitEquipmentId,
                    HardwareKitId = e.HardwareKitId,
                    QuantityToShip = e.QuantityToShip
                }).ToList()
            });

            return hardwareKitBOs.ToList();
        }

        public IEnumerable<HardwareKitBO> GetByHardwareKitNumber(int projectId, string hardwareKitNumber)
        {
            var hardwareKits = hardwareKitEngine.List(HardwareKitSpecs.ProjectId(projectId) && HardwareKitSpecs.HardwareKitNumber(hardwareKitNumber));

            var hardwareKitBOs = hardwareKits.Select(x => new HardwareKitBO
            {
                ExtraQuantityPercentage = x.ExtraQuantityPercentage,
                HardwareKitId = x.HardwareKitId,
                HardwareKitNumber = x.HardwareKitNumber,
                IsCurrentRevision = x.IsCurrentRevision,
                ProjectId = x.ProjectId,
                Revision = x.Revision,
                HardwareKitEquipments = x.HardwareKitEquipments.Where(e => !e.Equipment.IsDeleted).Select(e => new HardwareKitEquipmentBO
                {
                    EquipmentId = e.EquipmentId,
                    HardwareKitEquipmentId = e.HardwareKitEquipmentId,
                    HardwareKitId = e.HardwareKitId,
                    QuantityToShip = e.QuantityToShip,
                    Equipment = new EquipmentBO
                    {
                        Description = e.Equipment.Description,
                        ShippingTagNumber = e.Equipment.ShippingTagNumber,
                        Quantity = e.Equipment.Quantity,
                    }
                }).ToList()
            });

            return hardwareKitBOs.ToList();
        }

        public void Update(HardwareKitBO hardwareKitBO)
        {
            var hardwareKit = new HardwareKit
            {
                ExtraQuantityPercentage = hardwareKitBO.ExtraQuantityPercentage,
                HardwareKitId = hardwareKitBO.HardwareKitId,
                HardwareKitNumber = hardwareKitBO.HardwareKitNumber,
                IsCurrentRevision = hardwareKitBO.IsCurrentRevision,
                ProjectId = hardwareKitBO.ProjectId,
                Revision = hardwareKitBO.Revision,
                HardwareKitEquipments = hardwareKitBO.HardwareKitEquipments.Select(x => new HardwareKitEquipment
                {
                    EquipmentId = x.EquipmentId,
                    HardwareKitEquipmentId = x.HardwareKitEquipmentId,
                    HardwareKitId = x.HardwareKitId,
                    QuantityToShip = x.QuantityToShip
                }).ToList()
            };

            hardwareKitEngine.UpdateHardwareKit(hardwareKit);

            dbContext.SaveChanges();


            var equipment = equipmentEngine.Get(EquipmentSpecs.HardwareKitId(hardwareKitBO.HardwareKitId));

            if (equipment != null)
            {
                equipment.HardwareKitId = hardwareKit.HardwareKitId;
                equipment.EquipmentName = "Hardware Kit " + hardwareKitBO.HardwareKitNumber;

                equipmentEngine.UpdateEquipment(equipment);

                dbContext.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            var hardwareKit = hardwareKitEngine.Get(HardwareKitSpecs.Id(id));

            if (hardwareKit != null)
            {
                hardwareKitEngine.DeleteHardwareKit(hardwareKit);
            }

            dbContext.SaveChanges();
        }
    }
}
