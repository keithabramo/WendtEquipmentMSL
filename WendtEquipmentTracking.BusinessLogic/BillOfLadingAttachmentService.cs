using System.Collections.Generic;
using System.Linq;
using WendtEquipmentTracking.DataAccess.FileManagement;
using WendtEquipmentTracking.DataAccess.FileManagement.Api;
using WendtEquipmentTracking.DataAccess.SQL;

namespace WendtEquipmentTracking.BusinessLogic
{
    public class BillOfLadingAttachmentService : IBillOfLadingAttachmentService
    {
        private WendtEquipmentTrackingEntities dbContext;
        private IBillOfLadingAttachmentEngine billOfLadingAttachmentEngine;
        private IFileEngine attachmentEngine;

        public BillOfLadingAttachmentService()
        {
            dbContext = new WendtEquipmentTrackingEntities();
            billOfLadingAttachmentEngine = new BillOfLadingAttachmentEngine(dbContext);
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
    }
}
