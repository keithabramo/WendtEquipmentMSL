using System;
using System.Data.Entity;
using System.Linq;
using WendtEquipmentTracking.Common;
using WendtEquipmentTracking.DataAccess.SQL.Api;
using WendtEquipmentTracking.DataAccess.SQL.Specifications;

namespace WendtEquipmentTracking.DataAccess.SQL.Engine
{

    public class BillOfLadingEngine : IBillOfLadingEngine
    {
        private IRepository<BillOfLading> repository = null;


        public BillOfLadingEngine(WendtEquipmentTrackingEntities dbContext)
        {
            this.repository = new Repository<BillOfLading>(dbContext);
        }

        public BillOfLadingEngine(Repository<BillOfLading> repository)
        {
            this.repository = repository;
        }

        public IQueryable<BillOfLading> ListAll()
        {
            return this.repository.Find(!BillOfLadingSpecs.IsDeleted())
                .Include(x => x.BillOfLadingEquipments);
        }

        public BillOfLading Get(Specification<BillOfLading> specification)
        {
            return this.repository.Single(!BillOfLadingSpecs.IsDeleted() && specification);
        }

        public IQueryable<BillOfLading> List(Specification<BillOfLading> specification)
        {
            return this.repository.Find(!BillOfLadingSpecs.IsDeleted() && specification)
                .Include(x => x.BillOfLadingEquipments);
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

        }

        public void UpdateBillOfLading(BillOfLading billOfLading)
        {
            var now = DateTime.Now;

            billOfLading.CreatedDate = now;
            billOfLading.CreatedBy = ActiveDirectoryHelper.CurrentUserUsername();
            billOfLading.ModifiedDate = now;
            billOfLading.ModifiedBy = ActiveDirectoryHelper.CurrentUserUsername();

            var currentBillOfLading = this.Get(BillOfLadingSpecs.ProjectId(billOfLading.ProjectId) && BillOfLadingSpecs.CurrentRevision() && BillOfLadingSpecs.BillOfLadingNumber(billOfLading.BillOfLadingNumber));
            currentBillOfLading.IsCurrentRevision = false;

            billOfLading.Revision = currentBillOfLading.Revision + 1;
            billOfLading.IsCurrentRevision = true;

            this.repository.Insert(billOfLading);
            this.repository.Update(currentBillOfLading);

        }


        public void UpdateBillOfLadingLock(BillOfLading billOfLading)
        {
            var now = DateTime.Now;

            if (billOfLading.IsLocked)
            {
                billOfLading.LockedDate = now;
                billOfLading.LockedBy = ActiveDirectoryHelper.CurrentUserUsername();
            }
            else
            {
                billOfLading.LockedDate = null;
                billOfLading.LockedBy = null;
            }

            repository.Update(billOfLading);

        }

        public void DeleteBillOfLading(BillOfLading billOfLading)
        {
            var now = DateTime.Now;

            billOfLading.IsDeleted = true;
            billOfLading.ModifiedDate = now;
            billOfLading.ModifiedBy = ActiveDirectoryHelper.CurrentUserUsername();
            billOfLading.BillOfLadingEquipments.ToList().ForEach(ble => ble.IsDeleted = true);

            this.repository.Update(billOfLading);

        }

        public void SetDBContext(WendtEquipmentTrackingEntities dbContext)
        {
            this.repository.Dispose();
            this.repository = new Repository<BillOfLading>(dbContext);
        }
    }
}
