using System;
using System.Configuration;
using WendtEquipmentTracking.BusinessLogic;
using WendtEquipmentTracking.BusinessLogic.Api;

namespace WendtEquipmentTracking.AttachmentCleanup
{
    public class Program
    {
        private static IEquipmentAttachmentService equipmentAttachmentService;
        private static IBillOfLadingAttachmentService billOfLadingAttachmentService;

        public static void Main(string[] args)
        {
            string attachmentLocation = ConfigurationManager.AppSettings["AttachmentLocation"];
            int expiredDays = Int32.Parse(ConfigurationManager.AppSettings["ExpiredDays"]);

            equipmentAttachmentService = new EquipmentAttachmentService();
            billOfLadingAttachmentService = new BillOfLadingAttachmentService();

            equipmentAttachmentService.PurgeOldAttachments(attachmentLocation, expiredDays);
            billOfLadingAttachmentService.PurgeOldAttachments(attachmentLocation, expiredDays);
        }
    }
}
