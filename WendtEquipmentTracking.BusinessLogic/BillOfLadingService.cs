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
        private IBillOfLadingEngine billOfLadingEngine;
        private IEquipmentEngine equipmentEngine;


        public BillOfLadingService()
        {
            billOfLadingEngine = new BillOfLadingEngine();
            equipmentEngine = new EquipmentEngine();
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

                equipmentEngine.UpdateEquipment(equipment);
            }

            //update needs to happen after BOL AND all BOL Equipment have been saved
            billOfLadingEngine.UpdateRTS(billOfLading.BillOfLadingId);
        }

        public void Update(BillOfLadingBO billOfLadingBO)
        {
            var billOfLading = Mapper.Map<BillOfLading>(billOfLadingBO);

            billOfLadingEngine.UpdateBillOfLading(billOfLading);

            foreach (var billOfLadingEquipment in billOfLadingBO.BillOfLadingEquipments)
            {
                var equipment = equipmentEngine.Get(EquipmentSpecs.Id(billOfLadingEquipment.EquipmentId));
                equipment.HTSCode = (billOfLadingEquipment.HTSCode ?? string.Empty).ToUpper();
                equipment.CountryOfOrigin = (billOfLadingEquipment.CountryOfOrigin ?? string.Empty).ToUpper();

                equipmentEngine.UpdateEquipment(equipment);
            }

            //update needs to happen after BOL AND all BOL Equipment have been saved
            billOfLadingEngine.UpdateRTS(billOfLading.BillOfLadingId);

        }

        public void Delete(int id)
        {
            var billOfLading = billOfLadingEngine.Get(BillOfLadingSpecs.Id(id));

            if (billOfLading != null)
            {
                billOfLadingEngine.DeleteBillOfLading(billOfLading);
            }
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
