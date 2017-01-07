using System;
using System.Collections.Generic;
using WendtEquipmentTracking.Common;
using WendtEquipmentTracking.DataAccess.SQL.Api;

namespace WendtEquipmentTracking.DataAccess.SQL.Engine
{

    public class WorkOrderPriceEngine : IWorkOrderPriceEngine
    {
        private IRepository<WorkOrderPrice> repository = null;

        public WorkOrderPriceEngine()
        {
            this.repository = new Repository<WorkOrderPrice>();
        }

        public WorkOrderPriceEngine(WendtEquipmentTrackingEntities dbContext)
        {
            this.repository = new Repository<WorkOrderPrice>(dbContext);
        }

        public WorkOrderPriceEngine(Repository<WorkOrderPrice> repository)
        {
            this.repository = repository;
        }

        public IEnumerable<WorkOrderPrice> ListAll()
        {
            return this.repository.GetAll();
        }

        public WorkOrderPrice Get(Specification<WorkOrderPrice> specification)
        {
            return this.repository.Single(specification);
        }

        public IEnumerable<WorkOrderPrice> List(Specification<WorkOrderPrice> specification)
        {
            return this.repository.Find(specification);
        }

        public void AddNewWorkOrderPrice(WorkOrderPrice workOrderPrice)
        {
            var now = DateTime.Now;

            workOrderPrice.CreatedDate = now;
            workOrderPrice.CreatedBy = ActiveDirectoryHelper.CurrentUserUsername();
            workOrderPrice.ModifiedDate = now;
            workOrderPrice.ModifiedBy = ActiveDirectoryHelper.CurrentUserUsername();

            this.repository.Insert(workOrderPrice);
            this.repository.Save();
        }

        public void AddAllNewWorkOrderPrice(IEnumerable<WorkOrderPrice> workOrderPrices)
        {
            var now = DateTime.Now;

            foreach (var workOrderPrice in workOrderPrices)
            {
                workOrderPrice.CreatedDate = now;
                workOrderPrice.CreatedBy = ActiveDirectoryHelper.CurrentUserUsername();
                workOrderPrice.ModifiedDate = now;
                workOrderPrice.ModifiedBy = ActiveDirectoryHelper.CurrentUserUsername();

            }

            this.repository.InsertAll(workOrderPrices);
            this.repository.Save();
        }

        public void UpdateWorkOrderPrice(WorkOrderPrice workOrderPrice)
        {
            var now = DateTime.Now;

            workOrderPrice.ModifiedDate = now;
            workOrderPrice.ModifiedBy = ActiveDirectoryHelper.CurrentUserUsername();

            this.repository.Update(workOrderPrice);
            this.repository.Save();
        }

        public void DeleteWorkOrderPrice(WorkOrderPrice workOrderPrice)
        {
            this.repository.Delete(workOrderPrice);
            this.repository.Save();
        }

        public void SetDBContext(WendtEquipmentTrackingEntities dbContext)
        {
            this.repository.Dispose();
            this.repository = new Repository<WorkOrderPrice>(dbContext);
        }
    }
}
