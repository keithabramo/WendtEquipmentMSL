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
    public class BillOfLadingAttachmentService : IBillOfLadingAttachmentService
    {
        private WendtEquipmentTrackingEntities dbContext;
        private IBillOfLadingAttachmentEngine billOfLadingAttachmentEngine;
        private IProjectEngine projectEngine;
        private IFileEngine attachmentEngine;

        public BillOfLadingAttachmentService()
        {
            dbContext = new WendtEquipmentTrackingEntities();
            billOfLadingAttachmentEngine = new BillOfLadingAttachmentEngine(dbContext);
            projectEngine = new ProjectEngine(dbContext);
            attachmentEngine = new AttachmentEngine();
        }

        public void Save(BillOfLadingAttachmentBO billOfLadingAttachmentBO, string filePath, byte[] file)
        {
            var billOfLadingAttachment = new BillOfLadingAttachment
            {
                BillOfLadingAttachmentId = billOfLadingAttachmentBO.BillOfLadingAttachmentId,
                BillOfLadingId = billOfLadingAttachmentBO.BillOfLadingId,
                FileName = billOfLadingAttachmentBO.FileName,
                FileTitle = billOfLadingAttachmentBO.FileTitle
            };

            attachmentEngine.SaveFile(filePath, file);

            billOfLadingAttachmentEngine.AddNewBillOfLadingAttachment(billOfLadingAttachment);

            dbContext.SaveChanges();
        }

        public IEnumerable<BillOfLadingAttachmentBO> GetByBillOfLadingId(int billOfLadingId)
        {
            var billOfLadingAttachments = billOfLadingAttachmentEngine.List(BillOfLadingAttachmentSpecs.BillOfLadingId(billOfLadingId));

            var billOfLadingAttachmentBOs = billOfLadingAttachments.Select(x => new BillOfLadingAttachmentBO
            {
                BillOfLadingAttachmentId = x.BillOfLadingAttachmentId,
                BillOfLadingId = x.BillOfLadingId,
                FileName = x.FileName,
                FileTitle = x.FileTitle
            });

            return billOfLadingAttachmentBOs.ToList();
        }


        public void Delete(int id)
        {
            var billOfLadingAttachment = billOfLadingAttachmentEngine.Get(BillOfLadingAttachmentSpecs.Id(id));

            if (billOfLadingAttachment != null)
            {
                billOfLadingAttachmentEngine.DeleteBillOfLadingAttachment(billOfLadingAttachment);
            }

            dbContext.SaveChanges();
        }

        public void PurgeOldAttachments(string attachmentDirectoryLocation, int days)
        {

            var projects = projectEngine.ListRaw(ProjectSpecs.IsCompleted() && ProjectSpecs.ModifiedDateGreaterThanDaysAgoSpecification(days));

            if (projects != null)
            {
                var expiredFileNames = projects.SelectMany(x => x.Equipments.SelectMany(y => y.EquipmentAttachments.Select(z => z.FileName))).ToList();

                foreach (var fileName in expiredFileNames)
                {
                    var filePath = Path.Combine(attachmentDirectoryLocation, fileName);

                    attachmentEngine.RemoveFile(filePath);
                }

            }
        }
    }
}
