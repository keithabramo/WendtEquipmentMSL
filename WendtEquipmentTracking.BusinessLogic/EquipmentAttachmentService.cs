using System.Collections.Generic;
using System.IO;
using System.Linq;
using WendtEquipmentTracking.BusinessLogic.Api;
using WendtEquipmentTracking.BusinessLogic.BO;
using WendtEquipmentTracking.DataAccess.FileManagement;
using WendtEquipmentTracking.DataAccess.FileManagement.Api;
using WendtEquipmentTracking.DataAccess.SQL;
using WendtEquipmentTracking.DataAccess.SQL.Api;
using WendtEquipmentTracking.DataAccess.SQL.Engine;
using WendtEquipmentTracking.DataAccess.SQL.Specifications;

namespace WendtEquipmentTracking.BusinessLogic
{
    public class EquipmentAttachmentService : IEquipmentAttachmentService
    {
        private WendtEquipmentTrackingEntities dbContext;
        private IEquipmentAttachmentEngine equipmentAttachmentEngine;
        private IProjectEngine projectEngine;
        private IFileEngine attachmentEngine;

        public EquipmentAttachmentService()
        {
            dbContext = new WendtEquipmentTrackingEntities();
            equipmentAttachmentEngine = new EquipmentAttachmentEngine(dbContext);
            projectEngine = new ProjectEngine(dbContext);
            attachmentEngine = new AttachmentEngine();
        }

        public void Save(EquipmentAttachmentBO equipmentAttachmentBO, string filePath, byte[] file)
        {
            var equipmentAttachment = new EquipmentAttachment
            {
                EquipmentAttachmentId = equipmentAttachmentBO.EquipmentAttachmentId,
                EquipmentId = equipmentAttachmentBO.EquipmentId,
                FileName = equipmentAttachmentBO.FileName,
                FileTitle = equipmentAttachmentBO.FileTitle
            };

            attachmentEngine.SaveFile(filePath, file);

            equipmentAttachmentEngine.AddNewEquipmentAttachment(equipmentAttachment);

            dbContext.SaveChanges();
        }

        public IEnumerable<EquipmentAttachmentBO> GetByEquipmentId(int equipmentId)
        {
            var equipmentAttachments = equipmentAttachmentEngine.List(EquipmentAttachmentSpecs.EquipmentId(equipmentId));

            var equipmentAttachmentBOs = equipmentAttachments.Select(x => new EquipmentAttachmentBO
            {
                EquipmentAttachmentId = x.EquipmentAttachmentId,
                EquipmentId = x.EquipmentId,
                FileName = x.FileName,
                FileTitle = x.FileTitle
            });

            return equipmentAttachmentBOs.ToList();
        }


        public void Delete(int id)
        {
            var equipmentAttachment = equipmentAttachmentEngine.Get(EquipmentAttachmentSpecs.Id(id));

            if (equipmentAttachment != null)
            {
                equipmentAttachmentEngine.DeleteEquipmentAttachment(equipmentAttachment);
            }

            dbContext.SaveChanges();
        }

        public void PurgeOldAttachments(string attachmentDirectoryLocation, int days)
        {

            var projects = projectEngine.ListRaw(ProjectSpecs.IsCompleted() && ProjectSpecs.ModifiedDateGreaterThanDaysAgoSpecification(days));

            if (projects != null)
            {
                var expiredFileNames = projects.SelectMany(x => x.Equipments.SelectMany(y => y.EquipmentAttachments.Select(z => z.FileName))).ToList();
                
                foreach(var fileName in expiredFileNames)
                {
                    var filePath = Path.Combine(attachmentDirectoryLocation, fileName);

                    attachmentEngine.RemoveFile(filePath);
                }
                
            }
        }
    }
}
