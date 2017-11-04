
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
    public class ProjectService : IProjectService
    {
        private WendtEquipmentTrackingEntities dbContext;
        private IProjectEngine projectEngine;

        public ProjectService()
        {
            dbContext = new WendtEquipmentTrackingEntities();
            projectEngine = new ProjectEngine(dbContext);
        }

        public void Save(ProjectBO projectBO)
        {
            var project = new Project
            {
                FreightTerms = projectBO.FreightTerms,
                IncludeSoftCosts = projectBO.IncludeSoftCosts,
                IsCustomsProject = projectBO.IsCustomsProject,
                ProjectId = projectBO.ProjectId,
                ProjectNumber = projectBO.ProjectNumber,
                ShipToAddress = projectBO.ShipToAddress,
                ShipToBroker = projectBO.ShipToBroker,
                ShipToBrokerEmail = projectBO.ShipToBrokerEmail,
                ShipToBrokerPhoneFax = projectBO.ShipToBrokerPhoneFax,
                ShipToCompany = projectBO.ShipToCompany,
                ShipToContact1 = projectBO.ShipToContact1,
                ShipToContact1Email = projectBO.ShipToContact1Email,
                ShipToContact1PhoneFax = projectBO.ShipToContact1PhoneFax,
                ShipToContact2 = projectBO.ShipToContact2,
                ShipToContact2Email = projectBO.ShipToContact2Email,
                ShipToContact2PhoneFax = projectBO.ShipToContact2PhoneFax,
                ShipToCSZ = projectBO.ShipToCSZ
            };

            projectEngine.AddNewProject(project);

            dbContext.SaveChanges();
        }

        public IEnumerable<ProjectBO> GetAll()
        {
            var projects = projectEngine.ListAll();
            var projectBOs = projects.Select(x => new ProjectBO
            {
                FreightTerms = x.FreightTerms,
                IncludeSoftCosts = x.IncludeSoftCosts,
                IsCustomsProject = x.IsCustomsProject,
                ProjectId = x.ProjectId,
                ProjectNumber = x.ProjectNumber,
                ShipToAddress = x.ShipToAddress,
                ShipToBroker = x.ShipToBroker,
                ShipToBrokerEmail = x.ShipToBrokerEmail,
                ShipToBrokerPhoneFax = x.ShipToBrokerPhoneFax,
                ShipToCompany = x.ShipToCompany,
                ShipToContact1 = x.ShipToContact1,
                ShipToContact1Email = x.ShipToContact1Email,
                ShipToContact1PhoneFax = x.ShipToContact1PhoneFax,
                ShipToContact2 = x.ShipToContact2,
                ShipToContact2Email = x.ShipToContact2Email,
                ShipToContact2PhoneFax = x.ShipToContact2PhoneFax,
                ShipToCSZ = x.ShipToCSZ
            });

            return projectBOs;
        }

        public IEnumerable<ProjectBO> GetAllForNavigation()
        {
            var projects = projectEngine.ListAll();
            var projectBOs = projects.Select(p => new ProjectBO
            {
                ProjectId = p.ProjectId,
                ProjectNumber = p.ProjectNumber
            });

            return projectBOs;
        }

        public ProjectBO GetById(int id)
        {
            var project = projectEngine.Get(ProjectSpecs.Id(id));

            var projectBO = new ProjectBO
            {
                FreightTerms = project.FreightTerms,
                IncludeSoftCosts = project.IncludeSoftCosts,
                IsCustomsProject = project.IsCustomsProject,
                ProjectId = project.ProjectId,
                ProjectNumber = project.ProjectNumber,
                ShipToAddress = project.ShipToAddress,
                ShipToBroker = project.ShipToBroker,
                ShipToBrokerEmail = project.ShipToBrokerEmail,
                ShipToBrokerPhoneFax = project.ShipToBrokerPhoneFax,
                ShipToCompany = project.ShipToCompany,
                ShipToContact1 = project.ShipToContact1,
                ShipToContact1Email = project.ShipToContact1Email,
                ShipToContact1PhoneFax = project.ShipToContact1PhoneFax,
                ShipToContact2 = project.ShipToContact2,
                ShipToContact2Email = project.ShipToContact2Email,
                ShipToContact2PhoneFax = project.ShipToContact2PhoneFax,
                ShipToCSZ = project.ShipToCSZ
            };

            return projectBO;
        }

        public void Update(ProjectBO projectBO)
        {
            var oldProject = projectEngine.Get(ProjectSpecs.Id(projectBO.ProjectId));

            oldProject.FreightTerms = projectBO.FreightTerms;
            oldProject.ProjectNumber = projectBO.ProjectNumber;
            oldProject.ShipToAddress = projectBO.ShipToAddress;
            oldProject.ShipToBroker = projectBO.ShipToBroker;
            oldProject.ShipToBrokerEmail = projectBO.ShipToBrokerEmail;
            oldProject.ShipToBrokerPhoneFax = projectBO.ShipToBrokerPhoneFax;
            oldProject.ShipToCompany = projectBO.ShipToCompany;
            oldProject.ShipToContact1 = projectBO.ShipToContact1;
            oldProject.ShipToContact1Email = projectBO.ShipToContact1Email;
            oldProject.ShipToContact1PhoneFax = projectBO.ShipToContact1PhoneFax;
            oldProject.ShipToContact2 = projectBO.ShipToContact2;
            oldProject.ShipToContact2Email = projectBO.ShipToContact2Email;
            oldProject.ShipToContact2PhoneFax = projectBO.ShipToContact2PhoneFax;
            oldProject.ShipToCSZ = projectBO.ShipToCSZ;
            oldProject.IsCustomsProject = projectBO.IsCustomsProject;
            oldProject.IncludeSoftCosts = projectBO.IncludeSoftCosts;

            projectEngine.UpdateProject(oldProject);

            dbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            var project = projectEngine.Get(ProjectSpecs.Id(id));

            if (project != null)
            {
                projectEngine.DeleteProject(project);
            }

            dbContext.SaveChanges();
        }
    }
}
