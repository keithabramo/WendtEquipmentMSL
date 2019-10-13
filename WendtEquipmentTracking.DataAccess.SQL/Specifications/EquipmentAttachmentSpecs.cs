using System.Collections.Generic;
using WendtEquipmentTracking.DataAccess.SQL.Specifications.EquipmentAttachments;

namespace WendtEquipmentTracking.DataAccess.SQL.Specifications
{

    public static class EquipmentAttachmentSpecs
    {
        public static Specification<EquipmentAttachment> Id(int id)
        {
            return new IdSpecification(id);
        }

        public static Specification<EquipmentAttachment> IsDeleted()
        {
            return new IsDeletedSpecification();
        }

        public static Specification<EquipmentAttachment> EquipmentId(int equipmentId)
        {
            return new EquipmentIdSpecification(equipmentId);
        }
    }
}
