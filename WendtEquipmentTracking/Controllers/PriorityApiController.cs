
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
    public class PriorityApiController : ApiController
    {
        private IPriorityService priorityService;
        private IUserService userService;
        private IProjectService projectService;

        public PriorityApiController()
        {
            priorityService = new PriorityService();
            userService = new UserService();
            projectService = new ProjectService();
        }

        //
        // GET: api/PriorityApi/Table
        [HttpGet]
        public IEnumerable<PriorityModel> Table()
        {
            IEnumerable<PriorityModel> priorityModels = new List<PriorityModel>();

            var user = userService.GetCurrentUser();

            if (user == null)
            {
                return priorityModels;
            }


            //Get Data
            var priorityBOs = priorityService.GetAll(user.ProjectId);

            priorityModels = priorityBOs.Select(x => new PriorityModel
            {
                PriorityNumber = x.PriorityNumber,
                PriorityId = x.PriorityId,
                DueDate = x.DueDate,
                EquipmentName = x.EquipmentName
            });

            return priorityModels;
        }

        [HttpGet]
        [HttpPost]
        public DtResponse Delete()
        {
            var user = userService.GetCurrentUser();


            if (user != null)
            {
                var httpData = DatatableHelpers.HttpData();
                Dictionary<string, object> data = httpData["data"] as Dictionary<string, object>;

                var prioritys = new List<int>();
                foreach (var priorityIdString in data.Keys)
                {

                    var priorityId = !string.IsNullOrEmpty(priorityIdString) ? Convert.ToInt32(priorityIdString) : 0;
                    if (priorityId != 0)
                    {
                        prioritys.Add(priorityId);
                    }
                }

                priorityService.Delete(prioritys.FirstOrDefault());


            }

            return new DtResponse { };
        }
    }
}