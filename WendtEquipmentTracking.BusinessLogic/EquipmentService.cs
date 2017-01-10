﻿using AutoMapper;
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

        public int Save(EquipmentBO equipmentBO)
        {
            var equipment = Mapper.Map<Equipment>(equipmentBO);

            return equipmentEngine.AddNewEquipment(equipment);
        }

        public void SaveAll(IEnumerable<EquipmentBO> equipmentBOs)
        {
            var equipments = Mapper.Map<IEnumerable<Equipment>>(equipmentBOs);

            equipmentEngine.AddAllNewEquipment(equipments);
        }

        public void UpdateReadyToShip(IEnumerable<EquipmentBO> alteredEquipmentBOs)
        {
            //get old equipment
            var oldEquipments = equipmentEngine.List(EquipmentSpecs.Ids(alteredEquipmentBOs.Select(e => e.EquipmentId)));

            //update BOs with adjustments
            var equipmentBOs = Mapper.Map<IEnumerable<EquipmentBO>>(oldEquipments);

            foreach (var equipmentBO in equipmentBOs)
            {
                var alteredEquipmentBO = alteredEquipmentBOs.SingleOrDefault(ae => ae.EquipmentId == equipmentBO.EquipmentId);
                equipmentBO.ReadyToShip = alteredEquipmentBO.ReadyToShip;

                var oldEquipment = oldEquipments.SingleOrDefault(ae => ae.EquipmentId == equipmentBO.EquipmentId);

                Mapper.Map<EquipmentBO, Equipment>(equipmentBO, oldEquipment);
            }

            equipmentEngine.UpdateAllEquipment(oldEquipments.ToList());
        }

        public void Update(EquipmentBO alteredEquipmentBO)
        {
            var oldEquipment = equipmentEngine.Get(EquipmentSpecs.Id(alteredEquipmentBO.EquipmentId));
            Mapper.Map<EquipmentBO, Equipment>(alteredEquipmentBO, oldEquipment);

            //update BOs with adjustments
            var equipmentBO = Mapper.Map<EquipmentBO>(oldEquipment);

            Mapper.Map<EquipmentBO, Equipment>(equipmentBO, oldEquipment);


            equipmentEngine.UpdateEquipment(oldEquipment);
        }

        public void UpdateAll(IEnumerable<EquipmentBO> equipmentBOs)
        {
            //this will dispose and reinstantiate a new context so we don't get transaction errors
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


    }
}
