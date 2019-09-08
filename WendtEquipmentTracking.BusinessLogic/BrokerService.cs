
using System.Collections.Generic;
using System.Linq;
using WendtEquipmentTracking.BusinessLogic.Api;
using WendtEquipmentTracking.BusinessLogic.BO;
using WendtEquipmentTracking.DataAccess.SQL;
using WendtEquipmentTracking.DataAccess.SQL.Api;
using WendtEquipmentTracking.DataAccess.SQL.Engine;
using WendtEquipmentTracking.DataAccess.SQL.Specifications;

namespace WendtEquipmentTracking.BusinessLogic
{
    public class BrokerService : IBrokerService
    {
        private WendtEquipmentTrackingEntities dbContext;
        private IBrokerEngine brokerEngine;

        public BrokerService()
        {
            dbContext = new WendtEquipmentTrackingEntities();
            brokerEngine = new BrokerEngine(dbContext);
        }

        public IEnumerable<int> SaveAll(IEnumerable<BrokerBO> brokerBOs)
        {
            var brokers = brokerBOs.Select(x => new Broker
            {
                Name = x.Name,
                Address = x.Address,
                Contact1 = x.Contact1,
                Email = x.Email,
                PhoneFax = x.PhoneFax
            }).ToList();

            brokerEngine.AddNewBrokers(brokers);

            dbContext.SaveChanges();

            return brokers.Select(x => x.BrokerId).ToList();
        }

        public IEnumerable<BrokerBO> GetAll()
        {
            var brokers = brokerEngine.ListAll();

            var brokerBOs = brokers.Select(x => new BrokerBO
            {
                BrokerId = x.BrokerId,
                Name = x.Name,
                Address = x.Address,
                Contact1 = x.Contact1,
                Email = x.Email,
                PhoneFax = x.PhoneFax
            });

            return brokerBOs.ToList();
        }

        public BrokerBO GetById(int id)
        {
            var broker = brokerEngine.Get(BrokerSpecs.Id(id));

            var brokerBO = new BrokerBO
            {
                BrokerId = broker.BrokerId,
                Name = broker.Name,
                Address = broker.Address,
                Contact1 = broker.Contact1,
                Email = broker.Email,
                PhoneFax = broker.PhoneFax
            };

            return brokerBO;
        }

        public IEnumerable<BrokerBO> GetByIds(IEnumerable<int> ids)
        {
            var brokers = brokerEngine.List(BrokerSpecs.Ids(ids));

            var brokerBOs = brokers.Select(x => new BrokerBO
            {
                Name = x.Name,
                Address = x.Address,
                Contact1 = x.Contact1,
                Email = x.Email,
                PhoneFax = x.PhoneFax,
                BrokerId = x.BrokerId
            });

            return brokerBOs.ToList();
        }

        public void Update(BrokerBO brokerBO)
        {
            var oldBroker = brokerEngine.Get(BrokerSpecs.Id(brokerBO.BrokerId));
            oldBroker.Name = brokerBO.Name;
            oldBroker.Address = brokerBO.Address;
            oldBroker.Contact1 = brokerBO.Contact1;
            oldBroker.Email = brokerBO.Email;
            oldBroker.PhoneFax = brokerBO.PhoneFax;

            brokerEngine.UpdateBroker(oldBroker);

            dbContext.SaveChanges();
        }

        public void UpdateAll(IEnumerable<BrokerBO> brokerBOs)
        {
            //Performance Issue?
            var oldBrokers = brokerEngine.List(BrokerSpecs.Ids(brokerBOs.Select(x => x.BrokerId))).ToList();

            foreach (var oldBroker in oldBrokers)
            {
                var brokerBO = brokerBOs.FirstOrDefault(x => x.BrokerId == oldBroker.BrokerId);

                if (brokerBO != null)
                {
                    oldBroker.Name = brokerBO.Name;
                    oldBroker.Address = brokerBO.Address;
                    oldBroker.Contact1 = brokerBO.Contact1;
                    oldBroker.Email = brokerBO.Email;
                    oldBroker.PhoneFax = brokerBO.PhoneFax;
                }
            }

            brokerEngine.UpdateBrokers(oldBrokers);

            dbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            var broker = brokerEngine.Get(BrokerSpecs.Id(id));

            if (broker != null)
            {
                brokerEngine.DeleteBroker(broker);
            }

            dbContext.SaveChanges();
        }
    }
}
