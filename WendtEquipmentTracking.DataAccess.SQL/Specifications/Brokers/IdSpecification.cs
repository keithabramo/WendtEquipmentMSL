using System;
using System.Linq.Expressions;

namespace WendtEquipmentTracking.DataAccess.SQL.Specifications.Brokers
{

    internal class IdSpecification : Specification<Broker>
    {

        private readonly int id;

        public IdSpecification(int id)
        {
            this.id = id;
        }

        public override Expression<Func<Broker, bool>> IsSatisfiedBy()
        {
            return e => e.BrokerId == id;
        }
    }
}
