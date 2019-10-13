using System;
using System.Linq;
using WendtEquipmentTracking.Common;
using WendtEquipmentTracking.DataAccess.SQL.Api;
using WendtEquipmentTracking.DataAccess.SQL.Specifications;

namespace WendtEquipmentTracking.DataAccess.SQL.Engine
{

    public class BillOfLadingAttachmentEngine : IBillOfLadingAttachmentEngine
    {
        private IRepository<BillOfLadingAttachment> repository = null;

        public BillOfLadingAttachmentEngine(WendtEquipmentTrackingEntities dbContext)
        {
            this.repository = new Repository<BillOfLadingAttachment>(dbContext);
        }

        public BillOfLadingAttachmentEngine(Repository<BillOfLadingAttachment> repository)
        {
            this.repository = repository;
        }

        public IQueryable<BillOfLadingAttachment> List(Specification<BillOfLadingAttachment> specification)
        {
            return this.repository.Find(!BillOfLadingAttachmentSpecs.IsDeleted() && specification);
        }

        public BillOfLadingAttachment Get(Specification<BillOfLadingAttachment> specification)
        {
            return this.repository.Single(!BillOfLadingAttachmentSpecs.IsDeleted() && specification);
        }

        public void AddNewBillOfLadingAttachment(BillOfLadingAttachment billOfLadingAttachment)
        {
            var now = DateTime.Now;

            billOfLadingAttachment.CreatedDate = now;
            billOfLadingAttachment.CreatedBy = ActiveDirectoryHelper.CurrentUserUsername();
            billOfLadingAttachment.ModifiedDate = now;
            billOfLadingAttachment.ModifiedBy = ActiveDirectoryHelper.CurrentUserUsername();

            this.repository.Insert(billOfLadingAttachment);

        }

        public void DeleteBillOfLadingAttachment(BillOfLadingAttachment billOfLadingAttachment)
        {
            var now = DateTime.Now;

            billOfLadingAttachment.ModifiedDate = now;
            billOfLadingAttachment.ModifiedBy = ActiveDirectoryHelper.CurrentUserUsername();
            billOfLadingAttachment.IsDeleted = true;

            this.repository.Update(billOfLadingAttachment);

        }

        public void SetDBContext(WendtEquipmentTrackingEntities dbContext)
        {
            this.repository.Dispose();
            this.repository = new Repository<BillOfLadingAttachment>(dbContext);
        }
    }
}
