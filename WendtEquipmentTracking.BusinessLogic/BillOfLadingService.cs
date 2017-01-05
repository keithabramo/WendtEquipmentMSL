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
    public class BillOfLadingService : IBillOfLadingService
    {
        private IBillOfLadingEngine billOfLadingEngine;
        private EquipmentLogic equipmentLogic;


        public BillOfLadingService()
        {
            billOfLadingEngine = new BillOfLadingEngine();
            equipmentLogic = new EquipmentLogic();
        }

        public void Save(BillOfLadingBO billOfLadingBO)
        {
            var billOfLading = Mapper.Map<BillOfLading>(billOfLadingBO);

            billOfLadingEngine.AddNewBillOfLading(billOfLading);

            equipmentLogic.BOLAdjustment(billOfLading.BillOfLadingId);
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

        public IEnumerable<BillOfLadingBO> GetByBillOfLadingNumber(string billOfLadingNumber)
        {
            var billOfLadings = billOfLadingEngine.List(BillOfLadingSpecs.BillOfLadingNumber(billOfLadingNumber));

            var billOfLadingBOs = Mapper.Map<IEnumerable<BillOfLadingBO>>(billOfLadings);

            return billOfLadingBOs;
        }

        public void Update(BillOfLadingBO billOfLadingBO)
        {
            var billOfLading = Mapper.Map<BillOfLading>(billOfLadingBO);

            billOfLadingEngine.UpdateBillOfLading(billOfLading);

            equipmentLogic.BOLAdjustment(billOfLading.BillOfLadingId);
        }

        public void Delete(int id)
        {
            var billOfLading = billOfLadingEngine.Get(BillOfLadingSpecs.Id(id));

            if (billOfLading != null)
            {
                billOfLadingEngine.DeleteBillOfLading(billOfLading);
            }
        }
    }
}
