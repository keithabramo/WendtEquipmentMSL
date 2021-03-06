﻿using WendtEquipmentTracking.DataAccess.SQL.Specifications.HardwareKits;

namespace WendtEquipmentTracking.DataAccess.SQL.Specifications
{

    public static class HardwareKitSpecs
    {
        public static Specification<HardwareKit> Id(int id)
        {
            return new IdSpecification(id);
        }

        public static Specification<HardwareKit> ProjectId(int projectId)
        {
            return new ProjectIdSpecification(projectId);
        }

        public static Specification<HardwareKit> HardwareKitNumber(string billOfLandningNumber)
        {
            return new HardwareKitNumberSpecification(billOfLandningNumber);
        }

        public static Specification<HardwareKit> CurrentRevision()
        {
            return new CurrentRevisionSpecification();
        }

        public static Specification<HardwareKit> IsDeleted()
        {
            return new IsDeletedSpecification();
        }
    }
}
