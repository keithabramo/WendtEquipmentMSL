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
    public class BillOfLadingService : IBillOfLadingService
    {
        private WendtEquipmentTrackingEntities dbContext;
        private IBillOfLadingEngine billOfLadingEngine;
        private IEquipmentEngine equipmentEngine;


        public BillOfLadingService()
        {
            dbContext = new WendtEquipmentTrackingEntities();
            billOfLadingEngine = new BillOfLadingEngine(dbContext);
            equipmentEngine = new EquipmentEngine(dbContext);
        }

        public void Save(BillOfLadingBO billOfLadingBO)
        {
            var billOfLading = Mapper.Map<BillOfLading>(billOfLadingBO);

            billOfLadingEngine.AddNewBillOfLading(billOfLading);

            foreach (var billOfLadingEquipment in billOfLadingBO.BillOfLadingEquipments)
            {
                var equipment = equipmentEngine.Get(EquipmentSpecs.Id(billOfLadingEquipment.EquipmentId));
                equipment.HTSCode = (billOfLadingEquipment.HTSCode ?? string.Empty).ToUpper();
                equipment.CountryOfOrigin = (billOfLadingEquipment.CountryOfOrigin ?? string.Empty).ToUpper();
                equipment.ReadyToShip = equipment.ReadyToShip - billOfLadingEquipment.Quantity;

                equipmentEngine.UpdateEquipment(equipment);
            }

            dbContext.SaveChanges();

            //update needs to happen after BOL AND all BOL Equipment have been saved
            //billOfLadingEngine.UpdateRTS(billOfLading.BillOfLadingId);
        }

        public void Update(BillOfLadingBO billOfLadingBO)
        {
            var billOfLading = Mapper.Map<BillOfLading>(billOfLadingBO);

            var oldBillOfLading = billOfLadingEngine.Get(BillOfLadingSpecs.ProjectId(billOfLadingBO.ProjectId) && BillOfLadingSpecs.BillOfLadingNumber(billOfLadingBO.BillOfLadingNumber) && BillOfLadingSpecs.CurrentRevision());

            foreach (var oldBOLE in oldBillOfLading.BillOfLadingEquipments)
            {
                var updatedBOLE = billOfLadingBO.BillOfLadingEquipments.FirstOrDefault(x => x.EquipmentId == oldBOLE.EquipmentId);
                var equipment = equipmentEngine.Get(EquipmentSpecs.Id(oldBOLE.EquipmentId));

                if (updatedBOLE != null)
                {
                    equipment.ReadyToShip = equipment.ReadyToShip - (updatedBOLE.Quantity - oldBOLE.Quantity);
                    equipment.HTSCode = (updatedBOLE.HTSCode ?? string.Empty).ToUpper();
                    equipment.CountryOfOrigin = (updatedBOLE.CountryOfOrigin ?? string.Empty).ToUpper();
                }
                else
                {
                    equipment.ReadyToShip = equipment.ReadyToShip + oldBOLE.Quantity;
                }


                equipmentEngine.UpdateEquipment(equipment);
            }

            var newEquipment = billOfLadingBO.BillOfLadingEquipments.Where(x => !oldBillOfLading.BillOfLadingEquipments.Any(old => old.EquipmentId == x.EquipmentId));
            foreach (var billOfLadingEquipment in newEquipment)
            {
                var equipment = equipmentEngine.Get(EquipmentSpecs.Id(billOfLadingEquipment.EquipmentId));

                equipment.ReadyToShip = equipment.ReadyToShip - billOfLadingEquipment.Quantity;
                equipment.HTSCode = (billOfLadingEquipment.HTSCode ?? string.Empty).ToUpper();
                equipment.CountryOfOrigin = (billOfLadingEquipment.CountryOfOrigin ?? string.Empty).ToUpper();

                equipmentEngine.UpdateEquipment(equipment);
            }


            billOfLadingEngine.UpdateBillOfLading(billOfLading);



            dbContext.SaveChanges();


            //update needs to happen after BOL AND all BOL Equipment have been saved
            //billOfLadingEngine.UpdateRTS(billOfLading.BillOfLadingId);

        }

        public void Delete(int id)
        {
            var billOfLading = billOfLadingEngine.Get(BillOfLadingSpecs.Id(id));

            if (billOfLading != null)
            {
                billOfLadingEngine.DeleteBillOfLading(billOfLading);
            }

            dbContext.SaveChanges();
        }

        public IEnumerable<BillOfLadingBO> GetAll()
        {
            var billOfLadings = billOfLadingEngine.ListAll().ToList();

            var billOfLadingBOs = Mapper.Map<IEnumerable<BillOfLadingBO>>(billOfLadings);

            return billOfLadingBOs;
        }

        public BillOfLadingBO GetById(int id)
        {
            var billOfLading = billOfLadingEngine.Get(BillOfLadingSpecs.Id(id));

            var billOfLadingBO = Mapper.Map<BillOfLadingBO>(billOfLading);

            return billOfLadingBO;
        }

        public IEnumerable<BillOfLadingBO> GetByBillOfLadingNumber(int projectId, string billOfLadingNumber)
        {
            var billOfLadings = billOfLadingEngine.List(BillOfLadingSpecs.ProjectId(projectId) && BillOfLadingSpecs.BillOfLadingNumber(billOfLadingNumber));

            var billOfLadingBOs = Mapper.Map<IEnumerable<BillOfLadingBO>>(billOfLadings);

            return billOfLadingBOs;
        }


    }
}
