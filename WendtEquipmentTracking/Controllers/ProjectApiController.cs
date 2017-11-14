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
        private IUserService userService;

        public ProjectApiController()
        {
            projectService = new ProjectService();
            userService = new UserService();
        }


        [HttpGet]
        public IEnumerable<ProjectModel> Table()
        {
            IEnumerable<ProjectModel> projectModels = new List<ProjectModel>();

            var user = userService.GetCurrentUser();

            if (user == null)
            {
                return projectModels;
            }

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
                ShipToCSZ = x.ShipToCSZ
            });

            return projectModels;
        }
    }
}
