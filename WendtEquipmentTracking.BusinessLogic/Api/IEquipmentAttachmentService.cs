using System.Collections.Generic;
using WendtEquipmentTracking.BusinessLogic.BO;

namespace WendtEquipmentTracking.BusinessLogic.Api
{
    public interface IEquipmentAttachmentService
    {
        void Save(EquipmentAttachmentBO equipmentAttachmentBOs, string filePath, byte[] file);

        void Delete(int id);

        IEnumerable<EquipmentAttachmentBO> GetByEquipmentId(int equipmentId);

        void PurgeOldAttachments(string attachmentDirectoryLocation, int days);

    }
}