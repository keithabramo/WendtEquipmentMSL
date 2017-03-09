using AutoMapper;
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
        private IHardwareKitEngine hardwareKitEngine;
        private IEquipmentEngine equipmentEngine;


        public HardwareKitService()
        {
            hardwareKitEngine = new HardwareKitEngine();
            equipmentEngine = new EquipmentEngine();
        }

        public void Save(HardwareKitBO hardwareKitBO)
        {
            var hardwareKit = Mapper.Map<HardwareKit>(hardwareKitBO);

            var hardwareKitId = hardwareKitEngine.AddNewHardwareKit(hardwareKit);

            var equipment = new Equipment();
            equipment.HardwareKitId = hardwareKitId;
            equipment.EquipmentName = "Hardware Kit " + hardwareKitBO.HardwareKitNumber;
            equipment.Description = "Misc";
            equipment.ProjectId = hardwareKitBO.ProjectId;
            equipment.Quantity = 1; //hardwareKitBO.HardwareKitEquipments.Sum(hke => hke.Quantity);
            equipment.ReleaseDate = DateTime.Now;

            equipmentEngine.AddNewEquipment(equipment);
        }

        public IEnumerable<HardwareKitBO> GetAll()
        {
            var hardwareKits = hardwareKitEngine.ListAll().ToList();

            var hardwareKitBOs = Mapper.Map<IEnumerable<HardwareKitBO>>(hardwareKits);

            return hardwareKitBOs;
        }

        public HardwareKitBO GetById(int id)
        {
            var hardwareKit = hardwareKitEngine.Get(HardwareKitSpecs.Id(id));

            var hardwareKitBO = Mapper.Map<HardwareKitBO>(hardwareKit);

            return hardwareKitBO;
        }

        public IEnumerable<HardwareKitBO> GetByHardwareKitNumber(int projectId, string hardwareKitNumber)
        {
            var hardwareKits = hardwareKitEngine.List(HardwareKitSpecs.ProjectId(projectId) && HardwareKitSpecs.HardwareKitNumber(hardwareKitNumber));

            var hardwareKitBOs = Mapper.Map<IEnumerable<HardwareKitBO>>(hardwareKits);

            return hardwareKitBOs;
        }

        public void Update(HardwareKitBO hardwareKitBO)
        {
            var hardwareKit = Mapper.Map<HardwareKit>(hardwareKitBO);

            hardwareKitEngine.UpdateHardwareKit(hardwareKit);
        }

        public void Delete(int id)
        {
            var hardwareKit = hardwareKitEngine.Get(HardwareKitSpecs.Id(id));

            if (hardwareKit != null)
            {
                hardwareKitEngine.DeleteHardwareKit(hardwareKit);
            }
        }
    }
}
