using System;
using System.Collections.Generic;
using WendtEquipmentTracking.Common;
using WendtEquipmentTracking.DataAccess.SQL.Api;
using WendtEquipmentTracking.DataAccess.SQL.Specifications;

namespace WendtEquipmentTracking.DataAccess.SQL.Engine
{

    public class PriorityEngine : IPriorityEngine
    {
        private IRepository<Priority> repository = null;

        public PriorityEngine()
        {
            this.repository = new Repository<Priority>();
        }

        public PriorityEngine(WendtEquipmentTrackingEntities dbContext)
        {
            this.repository = new Repository<Priority>(dbContext);
        }

        public PriorityEngine(Repository<Priority> repository)
        {
            this.repository = repository;
        }

        public IEnumerable<Priority> ListAll()
        {
            return this.repository.Find(!PrioritySpecs.IsDeleted());
        }

        public Priority Get(Specification<Priority> specification)
        {
            return this.repository.Single(!PrioritySpecs.IsDeleted() && specification);
        }

        public IEnumerable<Priority> List(Specification<Priority> specification)
        {
            return this.repository.Find(!PrioritySpecs.IsDeleted() && specification);
        }

        public void AddNewPriority(Priority priority)
        {
            var now = DateTime.Now;

            priority.CreatedDate = now;
            priority.CreatedBy = ActiveDirectoryHelper.CurrentUserUsername();
            priority.ModifiedDate = now;
            priority.ModifiedBy = ActiveDirectoryHelper.CurrentUserUsername();

            this.repository.Insert(priority);
            this.repository.Save();
        }

        public void AddAllNewPriority(IEnumerable<Priority> prioritys)
        {
            var now = DateTime.Now;

            foreach (var priority in prioritys)
            {
                priority.CreatedDate = now;
                priority.CreatedBy = ActiveDirectoryHelper.CurrentUserUsername();
                priority.ModifiedDate = now;
                priority.ModifiedBy = ActiveDirectoryHelper.CurrentUserUsername();

            }

            this.repository.InsertAll(prioritys);
            this.repository.Save();
        }

        public void UpdatePriority(Priority priority)
        {
            var now = DateTime.Now;

            priority.ModifiedDate = now;
            priority.ModifiedBy = ActiveDirectoryHelper.CurrentUserUsername();

            this.repository.Update(priority);
            this.repository.Save();
        }

        public void DeletePriority(Priority priority)
        {
            priority.IsDeleted = true;

            this.repository.Update(priority);
            this.repository.Save();
        }

        public void SetDBContext(WendtEquipmentTrackingEntities dbContext)
        {
            this.repository.Dispose();
            this.repository = new Repository<Priority>(dbContext);
        }
    }
}
