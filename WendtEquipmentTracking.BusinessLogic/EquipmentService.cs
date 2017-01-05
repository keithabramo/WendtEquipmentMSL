using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using WendtEquipmentTracking.BusinessLogic.Api;
using WendtEquipmentTracking.BusinessLogic.BO;
using WendtEquipmentTracking.BusinessLogic.Utils;
using WendtEquipmentTracking.DataAccess.SQL;
using WendtEquipmentTracking.DataAccess.SQL.Api;
using WendtEquipmentTracking.DataAccess.SQL.Engine;
using WendtEquipmentTracking.DataAccess.SQL.Specifications;

namespace WendtEquipmentTracking.BusinessLogic
{
    public class EquipmentService : IEquipmentService
    {
        private IEquipmentEngine equipmentEngine;
        private EquipmentLogic equipmentLogic;

        public EquipmentService()
        {
            equipmentEngine = new EquipmentEngine();
            equipmentLogic = new EquipmentLogic();
        }

        public void Save(EquipmentBO equipmentBO)
        {
            equipmentLogic.EquipmentNew(equipmentBO);

            var equipment = Mapper.Map<Equipment>(equipmentBO);

            equipmentEngine.AddNewEquipment(equipment);
        }

        public void SaveAll(IEnumerable<EquipmentBO> equipmentBOs)
        {
            equipmentBOs.ToList().ForEach(e => equipmentLogic.EquipmentNew(e));

            var equipments = Mapper.Map<IEnumerable<Equipment>>(equipmentBOs);

            equipmentEngine.AddAllNewEquipment(equipments);
        }

        public void UpdateReadyToShip(IEnumerable<EquipmentBO> equipmentBOs)
        {
            var oldEquipments = equipmentEngine.List(EquipmentSpecs.Ids(equipmentBOs.Select(e => e.EquipmentId)));

            oldEquipments.ToList().ForEach(e => e.ReadyToShip = equipmentBOs.SingleOrDefault().ReadyToShip);

            equipmentEngine.UpdateAllEquipment(oldEquipments.ToList());
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

        public IEnumerable<EquipmentBO> GetByBillOfLadingId(int billOfLadingId)
        {
            var equipments = equipmentEngine.List(EquipmentSpecs.BillOfLadingId(billOfLadingId));

            var equipmentBOs = Mapper.Map<IEnumerable<EquipmentBO>>(equipments);

            return equipmentBOs;
        }

        public void Update(EquipmentBO equipmentBO)
        {

            var oldEquipment = equipmentEngine.Get(EquipmentSpecs.Id(equipmentBO.EquipmentId));

            Mapper.Map<EquipmentBO, Equipment>(equipmentBO, oldEquipment);

            equipmentEngine.UpdateEquipment(oldEquipment);
        }

        public void UpdateAll(IEnumerable<EquipmentBO> equipmentBOs)
        {
            equipmentEngine.SetDBContext(new WendtEquipmentTrackingEntities());

            var oldEquipments = equipmentEngine.List(EquipmentSpecs.Ids(equipmentBOs.Select(e => e.EquipmentId))).ToList();

            foreach (var oldEquipment in oldEquipments)
            {
                Mapper.Map<EquipmentBO, Equipment>(equipmentBOs.SingleOrDefault(e => e.EquipmentId == oldEquipment.EquipmentId), oldEquipment);
            }

            equipmentEngine.UpdateAllEquipment(oldEquipments.ToList());
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
