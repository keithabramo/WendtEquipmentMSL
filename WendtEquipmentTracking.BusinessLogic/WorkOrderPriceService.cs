
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
        private WendtEquipmentTrackingEntities dbContext;
        private IWorkOrderPriceEngine workOrderPriceEngine;

        public WorkOrderPriceService()
        {
            dbContext = new WendtEquipmentTrackingEntities();
            workOrderPriceEngine = new WorkOrderPriceEngine(dbContext);
        }

        public void Save(WorkOrderPriceBO workOrderPriceBO)
        {
            var workOrderPrice = new WorkOrderPrice
            {
                CostPrice = workOrderPriceBO.CostPrice,
                ProjectId = workOrderPriceBO.ProjectId,
                ReleasedPercent = workOrderPriceBO.ReleasedPercent,
                SalePrice = workOrderPriceBO.SalePrice,
                ShippedPercent = workOrderPriceBO.ShippedPercent,
                WorkOrderNumber = workOrderPriceBO.WorkOrderNumber,
                WorkOrderPriceId = workOrderPriceBO.WorkOrderPriceId
            };

            workOrderPriceEngine.AddNewWorkOrderPrice(workOrderPrice);

            dbContext.SaveChanges();
        }

        public IEnumerable<int> SaveAll(IEnumerable<WorkOrderPriceBO> workOrderPriceBOs)
        {
            var workOrderPrices = workOrderPriceBOs.Select(x => new WorkOrderPrice
            {
                CostPrice = x.CostPrice,
                ProjectId = x.ProjectId,
                ReleasedPercent = x.ReleasedPercent,
                SalePrice = x.SalePrice,
                ShippedPercent = x.ShippedPercent,
                WorkOrderNumber = x.WorkOrderNumber
            });

            workOrderPriceEngine.AddNewWorkOrderPrices(workOrderPrices);

            dbContext.SaveChanges();

            return workOrderPrices.Select(x => x.WorkOrderPriceId).ToList();

        }

        public IEnumerable<WorkOrderPriceBO> GetAll(int projectId)
        {
            var workOrderPrices = workOrderPriceEngine.List(WorkOrderPriceSpecs.ProjectId(projectId));

            var workOrderPriceBOs = workOrderPrices.Select(x => new WorkOrderPriceBO
            {
                CostPrice = x.CostPrice,
                ProjectId = x.ProjectId,
                ReleasedPercent = x.ReleasedPercent,
                SalePrice = x.SalePrice,
                ShippedPercent = x.ShippedPercent,
                WorkOrderNumber = x.WorkOrderNumber,
                WorkOrderPriceId = x.WorkOrderPriceId
            });

            return workOrderPriceBOs.ToList();
        }

        public WorkOrderPriceBO GetById(int id)
        {
            var workOrderPrice = workOrderPriceEngine.Get(WorkOrderPriceSpecs.Id(id));

            var workOrderPriceBO = new WorkOrderPriceBO
            {
                CostPrice = workOrderPrice.CostPrice,
                ProjectId = workOrderPrice.ProjectId,
                ReleasedPercent = workOrderPrice.ReleasedPercent,
                SalePrice = workOrderPrice.SalePrice,
                ShippedPercent = workOrderPrice.ShippedPercent,
                WorkOrderNumber = workOrderPrice.WorkOrderNumber,
                WorkOrderPriceId = workOrderPrice.WorkOrderPriceId
            };

            return workOrderPriceBO;
        }

        public IEnumerable<WorkOrderPriceBO> GetByIds(IEnumerable<int> ids)
        {
            var workOrderPrices = workOrderPriceEngine.List(WorkOrderPriceSpecs.Ids(ids));

            var workOrderPriceBOs = workOrderPrices.Select(x => new WorkOrderPriceBO
            {
                CostPrice = x.CostPrice,
                ProjectId = x.ProjectId,
                ReleasedPercent = x.ReleasedPercent,
                SalePrice = x.SalePrice,
                ShippedPercent = x.ShippedPercent,
                WorkOrderNumber = x.WorkOrderNumber,
                WorkOrderPriceId = x.WorkOrderPriceId
            });

            return workOrderPriceBOs.ToList();
        }

        public void Update(WorkOrderPriceBO workOrderPriceBO)
        {
            var oldWorkOrderPrice = workOrderPriceEngine.Get(WorkOrderPriceSpecs.Id(workOrderPriceBO.WorkOrderPriceId));
            oldWorkOrderPrice.CostPrice = workOrderPriceBO.CostPrice;
            oldWorkOrderPrice.ProjectId = workOrderPriceBO.ProjectId;
            oldWorkOrderPrice.ReleasedPercent = workOrderPriceBO.ReleasedPercent;
            oldWorkOrderPrice.SalePrice = workOrderPriceBO.SalePrice;
            oldWorkOrderPrice.ShippedPercent = workOrderPriceBO.ShippedPercent;
            oldWorkOrderPrice.WorkOrderNumber = workOrderPriceBO.WorkOrderNumber;

            workOrderPriceEngine.UpdateWorkOrderPrice(oldWorkOrderPrice);

            dbContext.SaveChanges();
        }

        public void UpdateAll(IEnumerable<WorkOrderPriceBO> workOrderPriceBOs)
        {
            //Performance Issue?
            var oldWorkOrderPrices = workOrderPriceEngine.List(WorkOrderPriceSpecs.Ids(workOrderPriceBOs.Select(x => x.WorkOrderPriceId))).ToList();

            foreach (var oldWorkOrderPrice in oldWorkOrderPrices)
            {
                var workOrderPriceBO = workOrderPriceBOs.FirstOrDefault(x => x.WorkOrderPriceId == oldWorkOrderPrice.WorkOrderPriceId);

                if (workOrderPriceBO != null)
                {
                    oldWorkOrderPrice.CostPrice = workOrderPriceBO.CostPrice;
                    oldWorkOrderPrice.ProjectId = workOrderPriceBO.ProjectId;
                    oldWorkOrderPrice.ReleasedPercent = workOrderPriceBO.ReleasedPercent;
                    oldWorkOrderPrice.SalePrice = workOrderPriceBO.SalePrice;
                    oldWorkOrderPrice.ShippedPercent = workOrderPriceBO.ShippedPercent;
                    oldWorkOrderPrice.WorkOrderNumber = workOrderPriceBO.WorkOrderNumber;
                }
            }

            workOrderPriceEngine.UpdateWorkOrderPrices(oldWorkOrderPrices);

            dbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            var workOrderPrice = workOrderPriceEngine.Get(WorkOrderPriceSpecs.Id(id));

            if (workOrderPrice != null)
            {
                workOrderPriceEngine.DeleteWorkOrderPrice(workOrderPrice);
            }

            dbContext.SaveChanges();
        }
    }
}
