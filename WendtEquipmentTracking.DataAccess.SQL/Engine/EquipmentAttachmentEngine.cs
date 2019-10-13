using System;
using System.Linq;
using WendtEquipmentTracking.Common;
using WendtEquipmentTracking.DataAccess.SQL.Api;
using WendtEquipmentTracking.DataAccess.SQL.Specifications;

namespace WendtEquipmentTracking.DataAccess.SQL.Engine
{

    public class EquipmentAttachmentEngine : IEquipmentAttachmentEngine
    {
        private IRepository<EquipmentAttachment> repository = null;

        public EquipmentAttachmentEngine(WendtEquipmentTrackingEntities dbContext)
        {
            this.repository = new Repository<EquipmentAttachment>(dbContext);
        }

        public EquipmentAttachmentEngine(Repository<EquipmentAttachment> repository)
        {
            this.repository = repository;
        }

        public IQueryable<EquipmentAttachment> List(Specification<EquipmentAttachment> specification)
        {
            return this.repository.Find(!EquipmentAttachmentSpecs.IsDeleted() && specification);
        }

        public EquipmentAttachment Get(Specification<EquipmentAttachment> specification)
        {
            return this.repository.Single(!EquipmentAttachmentSpecs.IsDeleted() && specification);
        }

        public void AddNewEquipmentAttachment(EquipmentAttachment equipmentAttachment)
        {
            var now = DateTime.Now;

            equipmentAttachment.CreatedDate = now;
            equipmentAttachment.CreatedBy = ActiveDirectoryHelper.CurrentUserUsername();
            equipmentAttachment.ModifiedDate = now;
            equipmentAttachment.ModifiedBy = ActiveDirectoryHelper.CurrentUserUsername();

            this.repository.Insert(equipmentAttachment);

        }

        public void DeleteEquipmentAttachment(EquipmentAttachment equipmentAttachment)
        {
            var now = DateTime.Now;

            equipmentAttachment.ModifiedDate = now;
            equipmentAttachment.ModifiedBy = ActiveDirectoryHelper.CurrentUserUsername();
            equipmentAttachment.IsDeleted = true;

            this.repository.Update(equipmentAttachment);

        }

        public void SetDBContext(WendtEquipmentTrackingEntities dbContext)
        {
            this.repository.Dispose();
            this.repository = new Repository<EquipmentAttachment>(dbContext);
        }
    }
}
