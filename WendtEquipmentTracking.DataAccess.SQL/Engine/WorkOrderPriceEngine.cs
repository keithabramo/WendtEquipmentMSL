using System;
using System.Collections.Generic;
using WendtEquipmentTracking.Common;
using WendtEquipmentTracking.DataAccess.SQL.Api;
using WendtEquipmentTracking.DataAccess.SQL.Specifications;

namespace WendtEquipmentTracking.DataAccess.SQL.Engine
{

    public class WorkOrderPriceEngine : IWorkOrderPriceEngine
    {
        private IRepository<WorkOrderPrice> repository = null;

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
            return this.repository.Find(!WorkOrderPriceSpecs.IsDeleted());
        }

        public WorkOrderPrice Get(Specification<WorkOrderPrice> specification)
        {
            return this.repository.Single(!WorkOrderPriceSpecs.IsDeleted() && specification);
        }

        public IEnumerable<WorkOrderPrice> List(Specification<WorkOrderPrice> specification)
        {
            return this.repository.Find(!WorkOrderPriceSpecs.IsDeleted() && specification);
        }

        public void AddNewWorkOrderPrice(WorkOrderPrice workOrderPrice)
        {
            var now = DateTime.Now;

            workOrderPrice.CreatedDate = now;
            workOrderPrice.CreatedBy = ActiveDirectoryHelper.CurrentUserUsername();
            workOrderPrice.ModifiedDate = now;
            workOrderPrice.ModifiedBy = ActiveDirectoryHelper.CurrentUserUsername();

            this.repository.Insert(workOrderPrice);

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

        }

        public void UpdateWorkOrderPrice(WorkOrderPrice workOrderPrice)
        {
            var now = DateTime.Now;

            workOrderPrice.ModifiedDate = now;
            workOrderPrice.ModifiedBy = ActiveDirectoryHelper.CurrentUserUsername();

            this.repository.Update(workOrderPrice);

        }

        public void UpdateWorkOrderPrices(IEnumerable<WorkOrderPrice> workOrderPrices)
        {
            var now = DateTime.Now;

            foreach (var workOrderPrice in workOrderPrices)
            {
                workOrderPrice.ModifiedDate = now;
                workOrderPrice.ModifiedBy = ActiveDirectoryHelper.CurrentUserUsername();

                this.repository.Update(workOrderPrice);
            }


        }

        public void DeleteWorkOrderPrice(WorkOrderPrice workOrderPrice)
        {
            workOrderPrice.IsDeleted = true;

            this.repository.Update(workOrderPrice);

        }

        public void SetDBContext(WendtEquipmentTrackingEntities dbContext)
        {
            this.repository.Dispose();
            this.repository = new Repository<WorkOrderPrice>(dbContext);
        }
    }
}
