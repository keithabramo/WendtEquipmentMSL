using System;
using System.Linq.Expressions;

namespace WendtEquipmentTracking.DataAccess.SQL.Specifications.Brokers
{

    internal class NameSpecification : Specification<Broker>
    {

        private readonly string name;

        public NameSpecification(string name)
        {
            this.name = name.ToLower();
        }

        public override Expression<Func<Broker, bool>> IsSatisfiedBy()
        {
            return e => e.Name.ToLower() == name;
        }
    }
}
