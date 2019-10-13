using System.Collections.Generic;
using WendtEquipmentTracking.BusinessLogic.BO;

namespace WendtEquipmentTracking.BusinessLogic.Api
{
    public interface IBillOfLadingAttachmentService
    {
        void Save(BillOfLadingAttachmentBO billOfLadingAttachmentBOs, string filePath, byte[] file);

        void Delete(int id);

        IEnumerable<BillOfLadingAttachmentBO> GetByBillOfLadingId(int billOfLadingId);

        void PurgeOldAttachments(string attachmentDirectoryLocation, int days);
    }
}