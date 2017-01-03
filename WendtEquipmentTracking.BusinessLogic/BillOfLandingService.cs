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
    public class BillOfLandingService : IBillOfLandingService
    {
        private IBillOfLandingEngine billOfLandingEngine;

        public BillOfLandingService()
        {
            billOfLandingEngine = new BillOfLandingEngine();
        }

        public void Save(BillOfLandingBO billOfLandingBO)
        {
            var billOfLanding = Mapper.Map<BillOfLanding>(billOfLandingBO);

            billOfLandingEngine.AddNewBillOfLanding(billOfLanding);
        }

        public IEnumerable<BillOfLandingBO> GetAll()
        {
            var billOfLandings = billOfLandingEngine.ListAll().ToList();

            var billOfLandingBOs = Mapper.Map<IEnumerable<BillOfLandingBO>>(billOfLandings);

            return billOfLandingBOs;
        }

        public BillOfLandingBO GetById(int id)
        {
            var billOfLanding = billOfLandingEngine.Get(BillOfLandingSpecs.Id(id));

            var billOfLandingBO = Mapper.Map<BillOfLandingBO>(billOfLanding);

            return billOfLandingBO;
        }

        public IEnumerable<BillOfLandingBO> GetByBillOfLandingNumber(string billOfLandingNumber)
        {
            var billOfLandings = billOfLandingEngine.List(BillOfLandingSpecs.BillOfLandingNumber(billOfLandingNumber));

            var billOfLandingBOs = Mapper.Map<IEnumerable<BillOfLandingBO>>(billOfLandings);

            return billOfLandingBOs;
        }

        public void Update(BillOfLandingBO billOfLandingBO)
        {
            var billOfLanding = Mapper.Map<BillOfLanding>(billOfLandingBO);

            billOfLandingEngine.UpdateBillOfLanding(billOfLanding);
        }

        public void Delete(int id)
        {
            var billOfLanding = billOfLandingEngine.Get(BillOfLandingSpecs.Id(id));

            if (billOfLanding != null)
            {
                billOfLandingEngine.DeleteBillOfLanding(billOfLanding);
            }
        }
    }
}
