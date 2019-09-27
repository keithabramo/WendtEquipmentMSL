using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using WendtEquipmentTracking.App.Models;
using WendtEquipmentTracking.BusinessLogic;
using WendtEquipmentTracking.BusinessLogic.Api;

namespace WendtEquipmentTracking.App.Controllers
{
    public class ProjectApiController : BaseApiController
    {
        private IProjectService projectService;

        public ProjectApiController()
        {
            projectService = new ProjectService();
        }


        [HttpGet]
        public IEnumerable<ProjectModel> Table()
        {
            IEnumerable<ProjectModel> projectModels = new List<ProjectModel>();

            var projectBOs = projectService.GetAll();

            projectModels = projectBOs.Select(x => new ProjectModel
            {
                FreightTerms = x.FreightTerms,
                IncludeSoftCosts = x.IncludeSoftCosts,
                IsCustomsProject = x.IsCustomsProject,
                IsCompleted = x.IsCompleted,
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
                ShipToCSZ = x.ShipToCSZ,
                PM = x.PM,
                ReceivingHours = x.ReceivingHours,
                Notes = x.Notes
            });

            return projectModels;
        }

        [HttpGet]
        public ProjectModel Get(int id)
        {
            var projectBO = projectService.GetById(id);

            var model = new ProjectModel
            {
                FreightTerms = projectBO.FreightTerms,
                IncludeSoftCosts = projectBO.IncludeSoftCosts,
                IsCustomsProject = projectBO.IsCustomsProject,
                IsCompleted = projectBO.IsCompleted,
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
                ShipToCSZ = projectBO.ShipToCSZ,
                PM = projectBO.PM,
                ReceivingHours = projectBO.ReceivingHours,
                Notes = projectBO.Notes
            };

            return model;
        }

        [HttpGet]
        public ProjectModel FindByProjectNumber(double id)
        {
            var projectBO = projectService.GetByProjectNumber(id);

            var model = new ProjectModel
            {
                FreightTerms = projectBO.FreightTerms,
                IncludeSoftCosts = projectBO.IncludeSoftCosts,
                IsCustomsProject = projectBO.IsCustomsProject,
                IsCompleted = projectBO.IsCompleted,
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
                ShipToCSZ = projectBO.ShipToCSZ,
                PM = projectBO.PM,
                ReceivingHours = projectBO.ReceivingHours,
                Notes = projectBO.Notes
            };

            return model;
        }
    }
}
