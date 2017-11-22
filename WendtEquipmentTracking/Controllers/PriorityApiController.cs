
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using WendtEquipmentTracking.App.Common;
using WendtEquipmentTracking.App.Models;
using WendtEquipmentTracking.BusinessLogic;
using WendtEquipmentTracking.BusinessLogic.Api;
using WendtEquipmentTracking.BusinessLogic.BO;

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
                EndDate = x.EndDate,
                ContractualShipDate = x.ContractualShipDate,
                EquipmentName = x.EquipmentName
            });

            return priorityModels;
        }




        [HttpGet]
        [HttpPost]
        public DtResponse Editor()
        {
            var user = userService.GetCurrentUser();

            IEnumerable<PriorityModel> priorityModels = new List<PriorityModel>();

            if (user != null)
            {
                var httpData = DatatableHelpers.HttpData();
                Dictionary<string, object> data = httpData["data"] as Dictionary<string, object>;
                var action = httpData["action"];

                var prioritys = new List<PriorityBO>();
                foreach (var priorityId in data.Keys)
                {

                    var row = data[priorityId];
                    var priorityProperties = row as Dictionary<string, object>;

                    var priority = new PriorityBO();

                    priority.PriorityId = !string.IsNullOrWhiteSpace(priorityProperties["PriorityId"].ToString()) ? Convert.ToInt32(priorityProperties["PriorityId"]) : 0;
                    priority.ContractualShipDate = !string.IsNullOrWhiteSpace(priorityProperties["ContractualShipDate"].ToString()) ? (DateTime?)Convert.ToDateTime(priorityProperties["ContractualShipDate"]) : null;
                    priority.DueDate = !string.IsNullOrWhiteSpace(priorityProperties["DueDate"].ToString()) ? Convert.ToDateTime(priorityProperties["DueDate"]) : DateTime.Now;
                    priority.EndDate = !string.IsNullOrWhiteSpace(priorityProperties["EndDate"].ToString()) ? (DateTime?)Convert.ToDateTime(priorityProperties["EndDate"]) : null;
                    priority.EquipmentName = priorityProperties["EquipmentName"].ToString();
                    priority.PriorityNumber = Convert.ToInt32(priorityProperties["PriorityNumber"].ToString());
                    priority.ProjectId = user.ProjectId;

                    prioritys.Add(priority);
                }

                if (action.Equals(EditorActions.edit.ToString()))
                {
                    priorityService.UpdateAll(prioritys);
                }
                else if (action.Equals(EditorActions.create.ToString()))
                {
                    priorityService.SaveAll(prioritys);
                }
                else if (action.Equals(EditorActions.remove.ToString()))
                {
                    priorityService.Delete(prioritys.FirstOrDefault().PriorityId);
                }

                priorityModels = prioritys.Select(x => new PriorityModel
                {
                    ContractualShipDate = x.ContractualShipDate,
                    DueDate = x.DueDate,
                    EndDate = x.EndDate,
                    EquipmentName = x.EquipmentName,
                    PriorityId = x.PriorityId,
                    PriorityNumber = x.PriorityNumber
                }).ToList();
            }

            return new DtResponse { data = priorityModels };
        }
    }
}