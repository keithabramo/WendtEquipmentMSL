using System;
using System.Collections.Generic;
using WendtEquipmentTracking.Common;
using WendtEquipmentTracking.DataAccess.SQL.Api;
using WendtEquipmentTracking.DataAccess.SQL.Specifications;

namespace WendtEquipmentTracking.DataAccess.SQL.Engine
{

    public class BillOfLandingEngine : IBillOfLandingEngine
    {
        private IRepository<BillOfLanding> repository = null;

        public BillOfLandingEngine()
        {
            this.repository = new Repository<BillOfLanding>();
        }

        public BillOfLandingEngine(WendtEquipmentTrackingEntities dbContext)
        {
            this.repository = new Repository<BillOfLanding>(dbContext);
        }

        public BillOfLandingEngine(Repository<BillOfLanding> repository)
        {
            this.repository = repository;
        }

        public IEnumerable<BillOfLanding> ListAll()
        {
            return this.repository.GetAll();
        }

        public BillOfLanding Get(Specification<BillOfLanding> specification)
        {
            return this.repository.Single(specification);
        }

        public IEnumerable<BillOfLanding> List(Specification<BillOfLanding> specification)
        {
            return this.repository.Find(specification);
        }

        public void AddNewBillOfLanding(BillOfLanding billOfLanding)
        {
            var now = DateTime.Now;

            billOfLanding.CreatedDate = now;
            billOfLanding.CreatedBy = ActiveDirectoryHelper.CurrentUserUsername();
            billOfLanding.ModifiedDate = now;
            billOfLanding.ModifiedBy = ActiveDirectoryHelper.CurrentUserUsername();

            billOfLanding.Revision = 1;
            billOfLanding.IsCurrentRevision = true;

            this.repository.Insert(billOfLanding);
            this.repository.Save();
        }

        public void UpdateBillOfLanding(BillOfLanding billOfLanding)
        {
            var now = DateTime.Now;

            billOfLanding.CreatedDate = now;
            billOfLanding.CreatedBy = ActiveDirectoryHelper.CurrentUserUsername();
            billOfLanding.ModifiedDate = now;
            billOfLanding.ModifiedBy = ActiveDirectoryHelper.CurrentUserUsername();

            var currentBillOfLanding = this.Get(BillOfLandingSpecs.CurrentRevision() && BillOfLandingSpecs.BillOfLandingNumber(billOfLanding.BillOfLandingNumber));
            currentBillOfLanding.IsCurrentRevision = false;

            billOfLanding.Revision = currentBillOfLanding.Revision + 1;
            billOfLanding.IsCurrentRevision = true;

            this.repository.Insert(billOfLanding);
            this.repository.Update(currentBillOfLanding);
            this.repository.Save();
        }

        public void DeleteBillOfLanding(BillOfLanding billOfLanding)
        {
            this.repository.Delete(billOfLanding);
            this.repository.Save();
        }

        public void SetDBContext(WendtEquipmentTrackingEntities dbContext)
        {
            this.repository = new Repository<BillOfLanding>(dbContext);
        }
    }
}
