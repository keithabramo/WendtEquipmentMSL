using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using WendtEquipmentTracking.Common;
using WendtEquipmentTracking.DataAccess.SQL.Api;
using WendtEquipmentTracking.DataAccess.SQL.Specifications;

namespace WendtEquipmentTracking.DataAccess.SQL.Engine
{

    public class ProjectEngine : IProjectEngine
    {
        private IRepository<Project> repository = null;

        public ProjectEngine()
        {
            this.repository = new Repository<Project>();
        }

        public ProjectEngine(WendtEquipmentTrackingEntities dbContext)
        {
            this.repository = new Repository<Project>(dbContext);
        }

        public ProjectEngine(Repository<Project> repository)
        {
            this.repository = repository;
        }

        public IEnumerable<Project> ListAll()
        {
            return this.repository.Find(!ProjectSpecs.IsDeleted())
                .Include(x => x.Equipments)
                .Include(x => x.HardwareKits)
                .Include(x => x.BillOfLadings)
                .Include(x => x.WorkOrderPrices);
        }

        public Project Get(Specification<Project> specification)
        {
            return this.repository.Single(!ProjectSpecs.IsDeleted() && specification);
        }

        public IEnumerable<Project> List(Specification<Project> specification)
        {
            return this.repository.Find(!ProjectSpecs.IsDeleted() && specification)
                .Include(x => x.Equipments)
                .Include(x => x.HardwareKits)
                .Include(x => x.BillOfLadings)
                .Include(x => x.WorkOrderPrices);
        }

        public void AddNewProject(Project project)
        {
            var now = DateTime.Now;

            project.CreatedDate = now;
            project.CreatedBy = ActiveDirectoryHelper.CurrentUserUsername();
            project.ModifiedDate = now;
            project.ModifiedBy = ActiveDirectoryHelper.CurrentUserUsername();

            this.repository.Insert(project);
            this.repository.Save();
        }

        public void UpdateProject(Project project)
        {
            var now = DateTime.Now;

            project.ModifiedDate = now;
            project.ModifiedBy = ActiveDirectoryHelper.CurrentUserUsername();

            this.repository.Update(project);
            this.repository.Save();
        }

        public void DeleteProject(Project project)
        {
            project.IsDeleted = true;
            project.BillOfLadings.ToList().ForEach(ble => ble.IsDeleted = true);
            project.Equipments.ToList().ForEach(ble => ble.IsDeleted = true);
            project.HardwareKits.ToList().ForEach(ble => ble.IsDeleted = true);
            project.WorkOrderPrices.ToList().ForEach(ble => ble.IsDeleted = true);

            this.repository.Update(project);
            this.repository.Save();
        }

        public void SetDBContext(WendtEquipmentTrackingEntities dbContext)
        {
            this.repository.Dispose();
            this.repository = new Repository<Project>(dbContext);
        }
    }
}
