﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using WendtEquipmentTracking.Common;
using WendtEquipmentTracking.DataAccess.SQL.Api;

namespace WendtEquipmentTracking.DataAccess.SQL.Engine
{

    public class ProjectEngine : IProjectEngine
    {
        private IRepository<Project> repository = null;

        public ProjectEngine() {
            this.repository = new Repository<Project>();
        }

        public ProjectEngine(WendtEquipmentTrackingEntities dbContext) {
            this.repository = new Repository<Project>(dbContext);
        }

        public ProjectEngine(Repository<Project> repository)
        {
            this.repository = repository;
        }

        public IEnumerable<Project> ListAll() {
            return this.repository.GetAll()
                .Include(x => x.Equipments)
                .Include(x => x.HardwareKits);
        }

        public Project Get(Specification<Project> specification) {
            return this.repository.Single(specification);
        }

        public IEnumerable<Project> List(Specification<Project> specification) {
            return this.repository.Find(specification)
                .Include(x => x.Equipments)
                .Include(x => x.HardwareKits);
        }

        public void AddNewProject(Project project) {
            var now = DateTime.Now;

            project.CreatedDate = now;
            project.CreatedBy = ActiveDirectoryHelper.CurrentUserUsername();
            project.ModifiedDate = now;
            project.ModifiedBy = ActiveDirectoryHelper.CurrentUserUsername();

            foreach(var equipment in project.Equipments)
            {
                equipment.CreatedDate = project.CreatedDate;
                equipment.CreatedBy = project.CreatedBy;
                equipment.ModifiedDate = project.ModifiedDate;
                equipment.ModifiedBy = project.ModifiedBy;
            }

            foreach (var hardwareKit in project.HardwareKits)
            {
                hardwareKit.CreatedDate = project.CreatedDate;
                hardwareKit.CreatedBy = project.CreatedBy;
                hardwareKit.ModifiedDate = project.ModifiedDate;
                hardwareKit.ModifiedBy = project.ModifiedBy;
            }

            this.repository.Insert(project);
            this.repository.Save();
        }

        public void UpdateProject(Project project) {
            var now = DateTime.Now;

            project.ModifiedDate = now;
            project.ModifiedBy = ActiveDirectoryHelper.CurrentUserUsername();

            foreach (var equipment in project.Equipments)
            {
                if (equipment.EquipmentId == 0)
                {
                    equipment.CreatedDate = project.CreatedDate;
                    equipment.CreatedBy = project.CreatedBy;
                }
                equipment.ModifiedDate = project.ModifiedDate;
                equipment.ModifiedBy = project.ModifiedBy;
            }

            foreach (var hardwareKit in project.HardwareKits)
            {
                if (hardwareKit.HardwareKitId == 0)
                {
                    hardwareKit.CreatedDate = project.CreatedDate;
                    hardwareKit.CreatedBy = project.CreatedBy;
                }
                hardwareKit.ModifiedDate = project.ModifiedDate;
                hardwareKit.ModifiedBy = project.ModifiedBy;
            }

            this.repository.Update(project);
            this.repository.Save();
        }

        public void DeleteProject(Project project) {
            this.repository.Delete(project);
            this.repository.Save();
        }

        public void SetDBContext(WendtEquipmentTrackingEntities dbContext) {
            this.repository = new Repository<Project>(dbContext);
        }
    }
}
