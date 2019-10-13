using System.Collections.Generic;
using System.Linq;

namespace WendtEquipmentTracking.DataAccess.SQL.Api
{
    public interface IEquipmentAttachmentEngine
    {
        IQueryable<EquipmentAttachment> List(Specification<EquipmentAttachment> specification);

        EquipmentAttachment Get(Specification<EquipmentAttachment> specification);

        void AddNewEquipmentAttachment(EquipmentAttachment equipmentAttachment);

        void DeleteEquipmentAttachment(EquipmentAttachment equipmentAttachment);

        void SetDBContext(WendtEquipmentTrackingEntities dbContext);
    }
}
