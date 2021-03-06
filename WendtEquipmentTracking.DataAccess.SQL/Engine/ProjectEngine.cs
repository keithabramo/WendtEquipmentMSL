﻿using System;
using System.Linq;
using WendtEquipmentTracking.Common;
using WendtEquipmentTracking.DataAccess.SQL.Api;
using WendtEquipmentTracking.DataAccess.SQL.Specifications;

namespace WendtEquipmentTracking.DataAccess.SQL.Engine
{

    public class ProjectEngine : IProjectEngine
    {
        private IRepository<Project> repository = null;

        public ProjectEngine(WendtEquipmentTrackingEntities dbContext)
        {
            this.repository = new Repository<Project>(dbContext);
        }

        public IQueryable<Project> ListAll()
        {
            return this.repository.Find(!ProjectSpecs.IsDeleted() && !ProjectSpecs.IsCompleted());
        }


        public Project Get(Specification<Project> specification)
        {
            return this.repository.Single(!ProjectSpecs.IsDeleted() && !ProjectSpecs.IsCompleted() && specification);
        }

        public IQueryable<Project> List(Specification<Project> specification)
        {
            return this.repository.Find(!ProjectSpecs.IsDeleted() && !ProjectSpecs.IsCompleted() && specification);
        }

        public void AddNewProject(Project project)
        {
            var now = DateTime.Now;

            project.CreatedDate = now;
            project.CreatedBy = ActiveDirectoryHelper.CurrentUserUsername();
            project.ModifiedDate = now;
            project.ModifiedBy = ActiveDirectoryHelper.CurrentUserUsername();

            this.repository.Insert(project);

        }

        public void UpdateProject(Project project)
        {
            var now = DateTime.Now;

            project.ModifiedDate = now;
            project.ModifiedBy = ActiveDirectoryHelper.CurrentUserUsername();

            this.repository.Update(project);

        }

        public void DeleteProject(Project project)
        {
            var now = DateTime.Now;

            project.ModifiedDate = now;
            project.ModifiedBy = ActiveDirectoryHelper.CurrentUserUsername();
            project.IsDeleted = true;
            project.BillOfLadings.ToList().ForEach(ble => ble.IsDeleted = true);
            project.Equipments.ToList().ForEach(ble => ble.IsDeleted = true);
            project.HardwareKits.ToList().ForEach(ble => ble.IsDeleted = true);
            project.WorkOrderPrices.ToList().ForEach(ble => ble.IsDeleted = true);

            this.repository.Update(project);

        }

        public IQueryable<Project> ListRaw(Specification<Project> specification)
        {
            return this.repository.Find(specification);
        }

        public Project GetRaw(Specification<Project> specification)
        {
            return this.repository.Single(specification);
        }

        public void UndeleteProject(Project project)
        {
            var now = DateTime.Now;

            project.ModifiedDate = now;
            project.ModifiedBy = ActiveDirectoryHelper.CurrentUserUsername();
            project.IsDeleted = false;
            project.BillOfLadings.ToList().ForEach(ble => ble.IsDeleted = false);
            project.Equipments.ToList().ForEach(ble => ble.IsDeleted = false);
            project.HardwareKits.ToList().ForEach(ble => ble.IsDeleted = false);
            project.WorkOrderPrices.ToList().ForEach(ble => ble.IsDeleted = false);

            this.repository.Update(project);

        }

        public void UncompleteProject(Project project)
        {
            var now = DateTime.Now;

            project.ModifiedDate = now;
            project.ModifiedBy = ActiveDirectoryHelper.CurrentUserUsername();
            project.IsCompleted = false;

            this.repository.Update(project);

        }

        public void SetDBContext(WendtEquipmentTrackingEntities dbContext)
        {
            this.repository.Dispose();
            this.repository = new Repository<Project>(dbContext);
        }
    }
}
