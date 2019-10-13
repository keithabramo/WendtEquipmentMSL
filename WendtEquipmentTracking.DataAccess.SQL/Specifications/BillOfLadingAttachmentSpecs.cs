using WendtEquipmentTracking.DataAccess.SQL.Specifications.BillOfLadingAttachments;

namespace WendtEquipmentTracking.DataAccess.SQL.Specifications
{

    public static class BillOfLadingAttachmentSpecs
    {
        public static Specification<BillOfLadingAttachment> Id(int id)
        {
            return new IdSpecification(id);
        }

        public static Specification<BillOfLadingAttachment> IsDeleted()
        {
            return new IsDeletedSpecification();
        }

        public static Specification<BillOfLadingAttachment> BillOfLadingId(int billOfLadingId)
        {
            return new BillOfLadingIdSpecification(billOfLadingId);
        }
    }
}
