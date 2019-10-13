using System.Linq;

namespace WendtEquipmentTracking.DataAccess.SQL.Api
{
    public interface IBillOfLadingAttachmentEngine
    {
        IQueryable<BillOfLadingAttachment> List(Specification<BillOfLadingAttachment> specification);

        BillOfLadingAttachment Get(Specification<BillOfLadingAttachment> specification);

        void AddNewBillOfLadingAttachment(BillOfLadingAttachment billOfLadingAttachment);

        void DeleteBillOfLadingAttachment(BillOfLadingAttachment billOfLadingAttachment);

        void SetDBContext(WendtEquipmentTrackingEntities dbContext);
    }
}
