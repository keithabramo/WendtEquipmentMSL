using System;
using System.Collections.Generic;
using System.Linq;
using WendtEquipmentTracking.Common;
using WendtEquipmentTracking.DataAccess.SQL.Api;
using WendtEquipmentTracking.DataAccess.SQL.Specifications;

namespace WendtEquipmentTracking.DataAccess.SQL.Engine
{

    public class PriorityEngine : IPriorityEngine
    {
        private IRepository<Priority> repository = null;

        public PriorityEngine(WendtEquipmentTrackingEntities dbContext)
        {
            this.repository = new Repository<Priority>(dbContext);
        }

        public PriorityEngine(Repository<Priority> repository)
        {
            this.repository = repository;
        }

        public IQueryable<Priority> ListAll()
        {
            return this.repository.Find(!PrioritySpecs.IsDeleted());
        }

        public Priority Get(Specification<Priority> specification)
        {
            return this.repository.Single(!PrioritySpecs.IsDeleted() && specification);
        }

        public IQueryable<Priority> List(Specification<Priority> specification)
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

        }

        public void AddNewPrioritys(IEnumerable<Priority> prioritys)
        {
            var now = DateTime.Now;

            foreach (var priority in prioritys)
            {
                priority.CreatedDate = now;
                priority.CreatedBy = ActiveDirectoryHelper.CurrentUserUsername();
                priority.ModifiedDate = now;
                priority.ModifiedBy = ActiveDirectoryHelper.CurrentUserUsername();
                this.repository.Insert(priority);
            }

        }

        public void UpdatePriority(Priority priority)
        {
            var now = DateTime.Now;

            priority.ModifiedDate = now;
            priority.ModifiedBy = ActiveDirectoryHelper.CurrentUserUsername();

            this.repository.Update(priority);

        }

        public void DeletePriority(Priority priority)
        {
            var now = DateTime.Now;

            priority.ModifiedDate = now;
            priority.ModifiedBy = ActiveDirectoryHelper.CurrentUserUsername();
            priority.IsDeleted = true;

            this.repository.Update(priority);

        }

        public void SetDBContext(WendtEquipmentTrackingEntities dbContext)
        {
            this.repository.Dispose();
            this.repository = new Repository<Priority>(dbContext);
        }
    }
}
