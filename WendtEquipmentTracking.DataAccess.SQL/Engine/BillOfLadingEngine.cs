using System;
using System.Collections.Generic;
using WendtEquipmentTracking.Common;
using WendtEquipmentTracking.DataAccess.SQL.Api;
using WendtEquipmentTracking.DataAccess.SQL.Specifications;

namespace WendtEquipmentTracking.DataAccess.SQL.Engine
{

    public class BillOfLadingEngine : IBillOfLadingEngine
    {
        private IRepository<BillOfLading> repository = null;

        public BillOfLadingEngine()
        {
            this.repository = new Repository<BillOfLading>();
        }

        public BillOfLadingEngine(WendtEquipmentTrackingEntities dbContext)
        {
            this.repository = new Repository<BillOfLading>(dbContext);
        }

        public BillOfLadingEngine(Repository<BillOfLading> repository)
        {
            this.repository = repository;
        }

        public IEnumerable<BillOfLading> ListAll()
        {
            return this.repository.GetAll();
        }

        public BillOfLading Get(Specification<BillOfLading> specification)
        {
            return this.repository.Single(specification);
        }

        public IEnumerable<BillOfLading> List(Specification<BillOfLading> specification)
        {
            return this.repository.Find(specification);
        }

        public void AddNewBillOfLading(BillOfLading billOfLading)
        {
            var now = DateTime.Now;

            billOfLading.CreatedDate = now;
            billOfLading.CreatedBy = ActiveDirectoryHelper.CurrentUserUsername();
            billOfLading.ModifiedDate = now;
            billOfLading.ModifiedBy = ActiveDirectoryHelper.CurrentUserUsername();

            billOfLading.Revision = 1;
            billOfLading.IsCurrentRevision = true;

            this.repository.Insert(billOfLading);
            this.repository.Save();
        }

        public void UpdateBillOfLading(BillOfLading billOfLading)
        {
            var now = DateTime.Now;

            billOfLading.CreatedDate = now;
            billOfLading.CreatedBy = ActiveDirectoryHelper.CurrentUserUsername();
            billOfLading.ModifiedDate = now;
            billOfLading.ModifiedBy = ActiveDirectoryHelper.CurrentUserUsername();

            var currentBillOfLading = this.Get(BillOfLadingSpecs.CurrentRevision() && BillOfLadingSpecs.BillOfLadingNumber(billOfLading.BillOfLadingNumber));
            currentBillOfLading.IsCurrentRevision = false;

            billOfLading.Revision = currentBillOfLading.Revision + 1;
            billOfLading.IsCurrentRevision = true;

            this.repository.Insert(billOfLading);
            this.repository.Update(currentBillOfLading);
            this.repository.Save();
        }

        public void DeleteBillOfLading(BillOfLading billOfLading)
        {
            this.repository.Delete(billOfLading);
            this.repository.Save();
        }

        public void SetDBContext(WendtEquipmentTrackingEntities dbContext)
        {
            this.repository.Dispose();
            this.repository = new Repository<BillOfLading>(dbContext);
        }
    }
}
