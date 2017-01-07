using System;
using System.Linq;
using System.Linq.Expressions;

namespace WendtEquipmentTracking.DataAccess.SQL.Specifications.Equipments
{

    internal class HardwareKitIdSpecification : Specification<Equipment>
    {

        private readonly int hardwareKitId;

        public HardwareKitIdSpecification(int hardwareKitId)
        {
            this.hardwareKitId = hardwareKitId;
        }

        public override Expression<Func<Equipment, bool>> IsSatisfiedBy()
        {
            return e => e.HardwareKitEquipments.Any(be => be.HardwareKitId == hardwareKitId);
        }
    }
}
