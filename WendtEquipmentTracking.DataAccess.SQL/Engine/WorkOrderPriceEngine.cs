using System;
using System.Collections.Generic;
using System.Linq;
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

        public IQueryable<WorkOrderPrice> ListAll()
        {
            return this.repository.Find(!WorkOrderPriceSpecs.IsDeleted());
        }

        public WorkOrderPrice Get(Specification<WorkOrderPrice> specification)
        {
            return this.repository.Single(!WorkOrderPriceSpecs.IsDeleted() && specification);
        }

        public IQueryable<WorkOrderPrice> List(Specification<WorkOrderPrice> specification)
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

        public void AddNewWorkOrderPrices(IEnumerable<WorkOrderPrice> workOrderPrices)
        {
            var now = DateTime.Now;

            foreach (var workOrderPrice in workOrderPrices)
            {
                workOrderPrice.CreatedDate = now;
                workOrderPrice.CreatedBy = ActiveDirectoryHelper.CurrentUserUsername();
                workOrderPrice.ModifiedDate = now;
                workOrderPrice.ModifiedBy = ActiveDirectoryHelper.CurrentUserUsername();
                this.repository.Insert(workOrderPrice);

            }
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
            var now = DateTime.Now;

            workOrderPrice.ModifiedDate = now;
            workOrderPrice.ModifiedBy = ActiveDirectoryHelper.CurrentUserUsername();
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
