using System;
using System.Collections.Generic;
using System.Linq;
using WendtEquipmentTracking.Common;
using WendtEquipmentTracking.DataAccess.SQL.Api;
using WendtEquipmentTracking.DataAccess.SQL.Specifications;

namespace WendtEquipmentTracking.DataAccess.SQL.Engine
{

    public class BrokerEngine : IBrokerEngine
    {
        private IRepository<Broker> repository = null;

        public BrokerEngine(WendtEquipmentTrackingEntities dbContext)
        {
            this.repository = new Repository<Broker>(dbContext);
        }

        public BrokerEngine(Repository<Broker> repository)
        {
            this.repository = repository;
        }

        public IQueryable<Broker> ListAll()
        {
            return this.repository.Find(!BrokerSpecs.IsDeleted());
        }

        public Broker Get(Specification<Broker> specification)
        {
            return this.repository.Single(!BrokerSpecs.IsDeleted() && specification);
        }

        public IQueryable<Broker> List(Specification<Broker> specification)
        {
            return this.repository.Find(!BrokerSpecs.IsDeleted() && specification);
        }

        public void AddNewBroker(Broker broker)
        {
            var now = DateTime.Now;

            broker.CreatedDate = now;
            broker.CreatedBy = ActiveDirectoryHelper.CurrentUserUsername();
            broker.ModifiedDate = now;
            broker.ModifiedBy = ActiveDirectoryHelper.CurrentUserUsername();

            this.repository.Insert(broker);

        }

        public void AddNewBrokers(IEnumerable<Broker> brokers)
        {
            var now = DateTime.Now;

            foreach (var broker in brokers)
            {
                broker.CreatedDate = now;
                broker.CreatedBy = ActiveDirectoryHelper.CurrentUserUsername();
                broker.ModifiedDate = now;
                broker.ModifiedBy = ActiveDirectoryHelper.CurrentUserUsername();
                this.repository.Insert(broker);

            }
        }

        public void UpdateBroker(Broker broker)
        {
            var now = DateTime.Now;

            broker.ModifiedDate = now;
            broker.ModifiedBy = ActiveDirectoryHelper.CurrentUserUsername();

            this.repository.Update(broker);

        }

        public void UpdateBrokers(IEnumerable<Broker> brokers)
        {
            var now = DateTime.Now;

            foreach (var broker in brokers)
            {
                broker.ModifiedDate = now;
                broker.ModifiedBy = ActiveDirectoryHelper.CurrentUserUsername();

                this.repository.Update(broker);
            }


        }

        public void DeleteBroker(Broker broker)
        {
            var now = DateTime.Now;

            broker.ModifiedDate = now;
            broker.ModifiedBy = ActiveDirectoryHelper.CurrentUserUsername();
            broker.IsDeleted = true;

            this.repository.Update(broker);

        }

        public void SetDBContext(WendtEquipmentTrackingEntities dbContext)
        {
            this.repository.Dispose();
            this.repository = new Repository<Broker>(dbContext);
        }
    }
}
