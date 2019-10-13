using System.Collections.Generic;
using WendtBillOfLadingTracking.BusinessLogic.BO;

namespace WendtBillOfLadingTracking.BusinessLogic.Api
{
    public interface IBillOfLadingAttachmentService
    {
        void Save(BillOfLadingAttachmentBO billOfLadingAttachmentBOs, string filePath, byte[] file);

        void Delete(int id);

        IEnumerable<BillOfLadingAttachmentBO> GetByBillOfLadingId(int billOfLadingId);
    }
}