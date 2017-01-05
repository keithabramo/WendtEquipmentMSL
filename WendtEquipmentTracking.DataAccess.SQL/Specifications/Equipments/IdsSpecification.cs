using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace WendtEquipmentTracking.DataAccess.SQL.Specifications.Equipments
{

    internal class IdsSpecification : Specification<Equipment>
    {

        private readonly IEnumerable<int> ids;

        public IdsSpecification(IEnumerable<int> ids)
        {
            this.ids = ids;
        }

        public override Expression<Func<Equipment, bool>> IsSatisfiedBy()
        {
            return e => ids.Contains(e.EquipmentId);
        }
    }
}
