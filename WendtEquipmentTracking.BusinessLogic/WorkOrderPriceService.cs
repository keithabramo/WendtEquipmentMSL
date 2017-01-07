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
    public class WorkOrderPriceService : IWorkOrderPriceService
    {
        private IWorkOrderPriceEngine workOrderPriceEngine;

        public WorkOrderPriceService()
        {
            workOrderPriceEngine = new WorkOrderPriceEngine();
        }

        public void Save(WorkOrderPriceBO workOrderPriceBO)
        {
            var workOrderPrice = Mapper.Map<WorkOrderPrice>(workOrderPriceBO);

            workOrderPriceEngine.AddNewWorkOrderPrice(workOrderPrice);
        }

        public void SaveAll(IEnumerable<WorkOrderPriceBO> workOrderPriceBOs)
        {
            var workOrderPrices = Mapper.Map<IEnumerable<WorkOrderPrice>>(workOrderPriceBOs);

            workOrderPriceEngine.AddAllNewWorkOrderPrice(workOrderPrices);
        }

        public IEnumerable<WorkOrderPriceBO> GetAll()
        {
            var workOrderPrices = workOrderPriceEngine.ListAll().ToList();

            var workOrderPriceBOs = Mapper.Map<IEnumerable<WorkOrderPriceBO>>(workOrderPrices);

            return workOrderPriceBOs;
        }

        public WorkOrderPriceBO GetById(int id)
        {
            var workOrderPrice = workOrderPriceEngine.Get(WorkOrderPriceSpecs.Id(id));

            var workOrderPriceBO = Mapper.Map<WorkOrderPriceBO>(workOrderPrice);

            return workOrderPriceBO;
        }

        public void Update(WorkOrderPriceBO workOrderPriceBO)
        {
            var oldWorkOrderPrice = workOrderPriceEngine.Get(WorkOrderPriceSpecs.Id(workOrderPriceBO.WorkOrderPriceId));

            Mapper.Map<WorkOrderPriceBO, WorkOrderPrice>(workOrderPriceBO, oldWorkOrderPrice);

            workOrderPriceEngine.UpdateWorkOrderPrice(oldWorkOrderPrice);
        }

        public void Delete(int id)
        {
            var workOrderPrice = workOrderPriceEngine.Get(WorkOrderPriceSpecs.Id(id));

            if (workOrderPrice != null)
            {
                workOrderPriceEngine.DeleteWorkOrderPrice(workOrderPrice);
            }
        }
    }
}
