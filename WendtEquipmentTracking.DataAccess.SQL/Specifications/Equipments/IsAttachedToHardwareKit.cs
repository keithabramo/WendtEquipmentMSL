﻿using System;
using System.Linq.Expressions;

namespace WendtEquipmentTracking.DataAccess.SQL.Specifications.Equipments
{

    internal class IsAttachedToHardwareKitSpecification : Specification<Equipment>
    {

        public override Expression<Func<Equipment, bool>> IsSatisfiedBy()
        {
            return e => e.HardwareKitEquipments.Count > 0;
        }
    }
}
