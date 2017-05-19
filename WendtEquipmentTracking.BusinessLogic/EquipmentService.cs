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

        public int Save(EquipmentBO equipmentBO)
        {
            var equipment = Mapper.Map<Equipment>(equipmentBO);

            equipment.CountryOfOrigin = (equipment.CountryOfOrigin?? string.Empty).ToUpperInvariant();
            equipment.Description = (equipment.Description?? string.Empty).ToUpperInvariant();
            equipment.DrawingNumber = (equipment.DrawingNumber?? string.Empty).ToUpperInvariant();
            equipment.EquipmentName = (equipment.EquipmentName?? string.Empty).ToUpperInvariant();
            equipment.HTSCode = (equipment.HTSCode?? string.Empty).ToUpperInvariant();
            equipment.Notes = (equipment.Notes?? string.Empty).ToUpperInvariant();
            equipment.ShippedFrom = (equipment.ShippedFrom?? string.Empty).ToUpperInvariant();
            equipment.ShippingTagNumber = (equipment.ShippingTagNumber?? string.Empty).ToUpperInvariant();
            equipment.WorkOrderNumber = (equipment.WorkOrderNumber?? string.Empty).ToUpperInvariant();


            var equipmentId = equipmentEngine.AddNewEquipment(equipment);

            //this will dispose and reinstantiate a new context so we get the latest updates
            //needed for ajax call of create equipment not getting the newest data from trigger update
            equipmentEngine.SetDBContext(new WendtEquipmentTrackingEntities());

            return equipmentId;
        }

        public void SaveAll(IEnumerable<EquipmentBO> equipmentBOs)
        {
            var equipments = Mapper.Map<IEnumerable<Equipment>>(equipmentBOs);
            equipments.ToList().ForEach(e =>
            {
                e.CountryOfOrigin = (e.CountryOfOrigin ?? string.Empty).ToUpperInvariant();
                e.Description = (e.Description ?? string.Empty).ToUpperInvariant();
                e.DrawingNumber = (e.DrawingNumber ?? string.Empty).ToUpperInvariant();
                e.EquipmentName = (e.EquipmentName ?? string.Empty).ToUpperInvariant();
                e.HTSCode = (e.HTSCode ?? string.Empty).ToUpperInvariant();
                e.Notes = (e.Notes ?? string.Empty).ToUpperInvariant();
                e.ShippedFrom = (e.ShippedFrom ?? string.Empty).ToUpperInvariant();
                e.ShippingTagNumber = (e.ShippingTagNumber ?? string.Empty).ToUpperInvariant();
                e.WorkOrderNumber = (e.WorkOrderNumber ?? string.Empty).ToUpperInvariant();
            });


            equipmentEngine.AddAllNewEquipment(equipments);
        }

        public void Update(EquipmentBO alteredEquipmentBO)
        {
            var oldEquipment = equipmentEngine.Get(EquipmentSpecs.Id(alteredEquipmentBO.EquipmentId));
            Mapper.Map<EquipmentBO, Equipment>(alteredEquipmentBO, oldEquipment);

            //update BOs with adjustments
            var equipmentBO = Mapper.Map<EquipmentBO>(oldEquipment);

            Mapper.Map<EquipmentBO, Equipment>(equipmentBO, oldEquipment);

            oldEquipment.CountryOfOrigin = (oldEquipment.CountryOfOrigin ?? string.Empty).ToUpperInvariant();
            oldEquipment.Description = (oldEquipment.Description ?? string.Empty).ToUpperInvariant();
            oldEquipment.DrawingNumber = (oldEquipment.DrawingNumber ?? string.Empty).ToUpperInvariant();
            oldEquipment.EquipmentName = (oldEquipment.EquipmentName ?? string.Empty).ToUpperInvariant();
            oldEquipment.HTSCode = (oldEquipment.HTSCode ?? string.Empty).ToUpperInvariant();
            oldEquipment.Notes = (oldEquipment.Notes ?? string.Empty).ToUpperInvariant();
            oldEquipment.ShippedFrom = (oldEquipment.ShippedFrom ?? string.Empty).ToUpperInvariant();
            oldEquipment.ShippingTagNumber = (oldEquipment.ShippingTagNumber ?? string.Empty).ToUpperInvariant();
            oldEquipment.WorkOrderNumber = (oldEquipment.WorkOrderNumber ?? string.Empty).ToUpperInvariant();

            equipmentEngine.UpdateEquipment(oldEquipment);

            //this will dispose and reinstantiate a new context so we get the latest updates
            //needed for ajax call of edit equipment not getting the newest data from trigger update
            equipmentEngine.SetDBContext(new WendtEquipmentTrackingEntities());
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

            oldEquipments.ToList().ForEach(e =>
            {
                e.CountryOfOrigin = (e.CountryOfOrigin ?? string.Empty).ToUpperInvariant();
                e.Description = (e.Description ?? string.Empty).ToUpperInvariant();
                e.DrawingNumber = (e.DrawingNumber ?? string.Empty).ToUpperInvariant();
                e.EquipmentName = (e.EquipmentName ?? string.Empty).ToUpperInvariant();
                e.HTSCode = (e.HTSCode ?? string.Empty).ToUpperInvariant();
                e.Notes = (e.Notes ?? string.Empty).ToUpperInvariant();
                e.ShippedFrom = (e.ShippedFrom ?? string.Empty).ToUpperInvariant();
                e.ShippingTagNumber = (e.ShippingTagNumber ?? string.Empty).ToUpperInvariant();
                e.WorkOrderNumber = (e.WorkOrderNumber ?? string.Empty).ToUpperInvariant();
            });

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

        public IEnumerable<EquipmentBO> GetAll(int projectId)
        {
            var equipments = equipmentEngine.List(EquipmentSpecs.ProjectId(projectId)).ToList();

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

        public IEnumerable<EquipmentBO> GetHardwareByShippingTagNumber(int projectId, string shippingTagNumber)
        {
            return GetHardwareByShippingTagNumber(projectId, shippingTagNumber, null);
        }

        public IEnumerable<EquipmentBO> GetHardwareByShippingTagNumber(int projectId, string shippingTagNumber, int? hardwareKitId)
        {
            var hardwareKitSpec = !EquipmentSpecs.IsAttachedToHardwareKit();
            if (hardwareKitId.HasValue)
            {
                hardwareKitSpec = hardwareKitSpec || EquipmentSpecs.HardwareKitId(hardwareKitId.Value);
            }


            var equipments = equipmentEngine.List(EquipmentSpecs.ProjectId(projectId) && EquipmentSpecs.IsHardware() && hardwareKitSpec && EquipmentSpecs.ShippingTagNumber(shippingTagNumber));

            var equipmentBOs = Mapper.Map<IEnumerable<EquipmentBO>>(equipments);

            return equipmentBOs;
        }
    }
}
