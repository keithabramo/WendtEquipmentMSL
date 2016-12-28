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
    public class EquipmentService : IEquipmentService
    {
        private IEquipmentEngine equipmentEngine;

        public EquipmentService()
        {
            equipmentEngine = new EquipmentEngine();
        }

        public void Save(EquipmentBO equipmentBO)
        {
            var equipment = Mapper.Map<Equipment>(equipmentBO);

            equipmentEngine.AddNewEquipment(equipment);
        }

        public void SaveAll(IEnumerable<EquipmentBO> equipmentBOs)
        {
            var equipments = Mapper.Map<IEnumerable<Equipment>>(equipmentBOs);

            equipmentEngine.AddAllNewEquipment(equipments);
        }

        public IEnumerable<EquipmentBO> GetAll()
        {
            var equipments = equipmentEngine.ListAll().ToList();

            var equipmentBOs = Mapper.Map<IEnumerable<EquipmentBO>>(equipments);

            return equipmentBOs;
        }

        public EquipmentBO GetById(int id)
        {
            var equipment = equipmentEngine.Get(EquipmentSpecs.Id(id));

            var equipmentBO = Mapper.Map<EquipmentBO>(equipment);

            return equipmentBO;
        }

        public void Update(EquipmentBO equipmentBO)
        {
            var oldEquipment = equipmentEngine.Get(EquipmentSpecs.Id(equipmentBO.EquipmentId));

            Mapper.Map<EquipmentBO, Equipment>(equipmentBO, oldEquipment);

            equipmentEngine.UpdateEquipment(oldEquipment);
        }

        public void Delete(int id)
        {
            var equipment = equipmentEngine.Get(EquipmentSpecs.Id(id));

            if (equipment != null)
            {
                equipmentEngine.DeleteEquipment(equipment);
            }
        }
    }
}
