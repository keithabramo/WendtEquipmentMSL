﻿using System;
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
            return e => e.HardwareKitId.HasValue && e.HardwareKitId.Value == hardwareKitId;
        }
    }
}
