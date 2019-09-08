using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace WendtEquipmentTracking.DataAccess.SQL.Specifications.Brokers
{

    internal class IdsSpecification : Specification<Broker>
    {

        private readonly IEnumerable<int> ids;

        public IdsSpecification(IEnumerable<int> ids)
        {
            this.ids = ids;
        }

        public override Expression<Func<Broker, bool>> IsSatisfiedBy()
        {
            return e => ids.Contains(e.BrokerId);
        }
    }
}
