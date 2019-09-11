using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using WendtEquipmentTracking.Common;
using WendtEquipmentTracking.DataAccess.SQL.Api;
using WendtEquipmentTracking.DataAccess.SQL.Specifications;

namespace WendtEquipmentTracking.DataAccess.SQL.Engine
{

    public class TruckingScheduleEngine : ITruckingScheduleEngine
    {
        private IRepository<TruckingSchedule> repository = null;

        public TruckingScheduleEngine(WendtEquipmentTrackingEntities dbContext)
        {
            this.repository = new Repository<TruckingSchedule>(dbContext);
        }

        public TruckingScheduleEngine(Repository<TruckingSchedule> repository)
        {
            this.repository = repository;
        }

        public IQueryable<TruckingSchedule> ListAll()
        {
            return this.repository.Find(!TruckingScheduleSpecs.IsDeleted())
                .Include(x => x.Project);
        }

        public TruckingSchedule Get(Specification<TruckingSchedule> specification)
        {
            return this.repository.Single(!TruckingScheduleSpecs.IsDeleted() && specification);
        }

        public IQueryable<TruckingSchedule> List(Specification<TruckingSchedule> specification)
        {
            return this.repository.Find(!TruckingScheduleSpecs.IsDeleted() && specification)
                .Include(x => x.Project);
        }

        public void AddNewTruckingSchedule(TruckingSchedule broker)
        {
            var now = DateTime.Now;

            broker.CreatedDate = now;
            broker.CreatedBy = ActiveDirectoryHelper.CurrentUserUsername();
            broker.ModifiedDate = now;
            broker.ModifiedBy = ActiveDirectoryHelper.CurrentUserUsername();

            this.repository.Insert(broker);

        }

        public void AddNewTruckingSchedules(IEnumerable<TruckingSchedule> brokers)
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

        public void UpdateTruckingSchedule(TruckingSchedule broker)
        {
            var now = DateTime.Now;

            broker.ModifiedDate = now;
            broker.ModifiedBy = ActiveDirectoryHelper.CurrentUserUsername();

            this.repository.Update(broker);

        }

        public void UpdateTruckingSchedules(IEnumerable<TruckingSchedule> brokers)
        {
            var now = DateTime.Now;

            foreach (var broker in brokers)
            {
                broker.ModifiedDate = now;
                broker.ModifiedBy = ActiveDirectoryHelper.CurrentUserUsername();

                this.repository.Update(broker);
            }


        }

        public void DeleteTruckingSchedule(TruckingSchedule broker)
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
            this.repository = new Repository<TruckingSchedule>(dbContext);
        }
    }
}
