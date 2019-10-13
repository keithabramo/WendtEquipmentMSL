using System;
using System.Linq.Expressions;

namespace WendtEquipmentTracking.DataAccess.SQL.Specifications.EquipmentAttachments
{

    internal class EquipmentIdSpecification : Specification<EquipmentAttachment>
    {

        private readonly int equipmentId;

        public EquipmentIdSpecification(int equipmentId)
        {
            this.equipmentId = equipmentId;
        }

        public override Expression<Func<EquipmentAttachment, bool>> IsSatisfiedBy()
        {
            return e => e.EquipmentId == equipmentId;
        }
    }
}
