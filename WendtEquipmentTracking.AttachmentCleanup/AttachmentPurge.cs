using WendtEquipmentTracking.BusinessLogic;
using WendtEquipmentTracking.BusinessLogic.Api;

namespace WendtEquipmentTracking.AttachmentCleanup
{
    public class AttachmentPurge
    {
        private static IEquipmentAttachmentService equipmentAttachmentService;
        private static IBillOfLadingAttachmentService billOfLadingAttachmentService;

        public static void Main(string[] args)
        {
            equipmentAttachmentService = new EquipmentAttachmentService();
            billOfLadingAttachmentService = new BillOfLadingAttachmentService();

            equipmentAttachmentService.PurgeOldAttachments();
            billOfLadingAttachmentService.PurgeOldAttachments();
        }
    }
}
