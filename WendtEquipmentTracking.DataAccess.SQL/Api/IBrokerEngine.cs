using System.Collections.Generic;
using System.Linq;

namespace WendtEquipmentTracking.DataAccess.SQL.Api
{
    public interface IBrokerEngine
    {
        IQueryable<Broker> ListAll();

        Broker Get(Specification<Broker> specification);

        IQueryable<Broker> List(Specification<Broker> specification);

        void AddNewBroker(Broker broker);

        void AddNewBrokers(IEnumerable<Broker> brokers);

        void UpdateBroker(Broker broker);

        void UpdateBrokers(IEnumerable<Broker> brokers);

        void DeleteBroker(Broker broker);

        void SetDBContext(WendtEquipmentTrackingEntities dbContext);
    }
}
