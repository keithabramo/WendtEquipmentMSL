using System;
using System.Linq.Expressions;

namespace WendtEquipmentTracking.DataAccess.SQL.Specifications.HardwareKits
{

    internal class HardwareKitNumberSpecification : Specification<HardwareKit>
    {

        private readonly string hardwareKitNumber;

        public HardwareKitNumberSpecification(string hardwareKitNumber)
        {
            this.hardwareKitNumber = hardwareKitNumber;
        }

        public override Expression<Func<HardwareKit, bool>> IsSatisfiedBy()
        {
            return e => e.HardwareKitNumber == hardwareKitNumber;
        }
    }
}
