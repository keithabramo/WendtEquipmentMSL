﻿using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using WendtEquipmentTracking.BusinessLogic.Api;
using WendtEquipmentTracking.BusinessLogic.BO;
using WendtEquipmentTracking.Common;
using WendtEquipmentTracking.DataAccess.SQL;
using WendtEquipmentTracking.DataAccess.SQL.Api;
using WendtEquipmentTracking.DataAccess.SQL.Engine;
using WendtEquipmentTracking.DataAccess.SQL.Specifications;

namespace WendtEquipmentTracking.BusinessLogic
{
    public class ProjectService : IProjectService
    {
        private IProjectEngine projectEngine;

        public ProjectService()
        {
            projectEngine = new ProjectEngine();
        }

        public void Save(ProjectBO projectBO)
        {
            var project = Mapper.Map<Project>(projectBO);

            projectEngine.AddNewProject(project);
        }

        public IEnumerable<ProjectBO> GetAll()
        {
            var projects = projectEngine.ListAll().ToList();

            var projectBOs = Mapper.Map<IEnumerable<ProjectBO>>(projects);

            return projectBOs;
        }
        
        public ProjectBO GetById(int id)
        {
            var project = projectEngine.Get(ProjectSpecs.Id(id));

            var projectBO = Mapper.Map<ProjectBO>(project);

            return projectBO;
        }
        
        public void Update(ProjectBO projectBO)
        {
            var oldProject = projectEngine.Get(ProjectSpecs.Id(projectBO.ProjectId));

            Mapper.Map<ProjectBO, Project>(projectBO, oldProject);

            projectEngine.UpdateProject(oldProject);
        }

        public void Delete(int id)
        {
            var project = projectEngine.Get(ProjectSpecs.Id(id));

            if(project != null)
            {
                projectEngine.DeleteProject(project);
            }
        }
    }
}
