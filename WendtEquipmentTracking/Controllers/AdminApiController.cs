using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using WendtEquipmentTracking.App.Common;
using WendtEquipmentTracking.App.Models;
using WendtEquipmentTracking.BusinessLogic;
using WendtEquipmentTracking.BusinessLogic.Api;

namespace WendtEquipmentTracking.App.Controllers
{
    public class AdminApiController : BaseApiController
    {
        private IProjectService projectService;
        private IUserService userService;

        public AdminApiController()
        {
            projectService = new ProjectService();
            userService = new UserService();
        }


        [HttpGet]
        public IEnumerable<ProjectModel> ProjectTable()
        {
            IEnumerable<ProjectModel> projectModels = new List<ProjectModel>();

            var user = userService.GetCurrentUser();

            if (user == null)
            {
                return projectModels;
            }

            var projectBOs = projectService.GetDeletedAndCompleted();

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
        [HttpPost]
        public DtResponse ProjectEditor()
        {
            var user = userService.GetCurrentUser();


            if (user != null)
            {
                var httpData = DatatableHelpers.HttpData();
                Dictionary<string, object> data = httpData["data"] as Dictionary<string, object>;

                var toUndelete = new List<int>();
                var toUncompleted = new List<int>();
                foreach (var projectIdString in data.Keys)
                {
                    var row = data[projectIdString];
                    var equipmentProperties = row as Dictionary<string, object>;

                    var projectId = !string.IsNullOrEmpty(projectIdString) ? Convert.ToInt32(projectIdString) : 0;
                    var isCompleted = equipmentProperties["IsCompleted"].ToString() == "true";

                    if (projectId != 0)
                    {
                        if (isCompleted)
                        {
                            toUncompleted.Add(projectId);
                        }
                        else
                        {
                            toUndelete.Add(projectId);
                        }
                    }
                }

                if (toUndelete.Count() > 0)
                {
                    projectService.Undelete(toUndelete.FirstOrDefault());
                }
                if (toUncompleted.Count() > 0)
                {
                    projectService.Uncomplete(toUncompleted.FirstOrDefault());

                }
            }

            return new DtResponse { };
        }
    }
}
