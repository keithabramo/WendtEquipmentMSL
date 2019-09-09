using System.Collections.Generic;
using WendtEquipmentTracking.DataAccess.SQL.Specifications.Brokers;

namespace WendtEquipmentTracking.DataAccess.SQL.Specifications
{

    public static class BrokerSpecs
    {
        public static Specification<Broker> Id(int id)
        {
            return new IdSpecification(id);
        }

        public static Specification<Broker> Name(string name)
        {
            return new NameSpecification(name);
        }

        public static Specification<Broker> Ids(IEnumerable<int> ids)
        {
            return new IdsSpecification(ids);
        }

        public static Specification<Broker> IsDeleted()
        {
            return new IsDeletedSpecification();
        }
    }
}
