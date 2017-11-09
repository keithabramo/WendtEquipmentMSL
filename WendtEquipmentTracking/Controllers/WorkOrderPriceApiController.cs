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
    public class WorkOrderPriceApiController : ApiController
    {
        private IWorkOrderPriceService workOrderPriceService;
        private IUserService userService;
        private IProjectService projectService;

        public WorkOrderPriceApiController()
        {
            workOrderPriceService = new WorkOrderPriceService();
            userService = new UserService();
            projectService = new ProjectService();
        }

        //
        // GET: api/WorkOrderPrice/Search
        [HttpGet]
        public IEnumerable<string> Search(string term)
        {
            var workOrderPrices = new List<string>();

            var user = userService.GetCurrentUser();

            if (user == null)
            {
                return workOrderPrices;
            }

            //Get Data
            var workOrderPriceBOs = workOrderPriceService.GetAll(user.ProjectId);

            workOrderPrices = workOrderPriceBOs
                                .Where(wop => wop.WorkOrderNumber.Contains(term))
                                .Select(wop => wop.WorkOrderNumber)
                                .OrderBy(e => e)
                                .ToList();

            return workOrderPrices;
        }

        //
        // GET: api/WorkOrderPrice/Table
        [HttpGet]
        public IEnumerable<WorkOrderPriceModel> Table()
        {
            var user = userService.GetCurrentUser();

            var workOrderBOs = workOrderPriceService.GetAll(user.ProjectId);

            var workOrderPriceModels = workOrderBOs.Select(x => new WorkOrderPriceModel
            {
                CostPrice = x.CostPrice,
                ProjectId = x.ProjectId,
                ReleasedPercent = x.ReleasedPercent,
                SalePrice = x.SalePrice,
                ShippedPercent = x.ShippedPercent,
                WorkOrderNumber = x.WorkOrderNumber,
                WorkOrderPriceId = x.WorkOrderPriceId,
            }).ToList();

            workOrderPriceModels
                .GroupBy(x => x.WorkOrderNumber != null ? x.WorkOrderNumber.ToUpperInvariant() : string.Empty)
                .Where(g => g.Count() > 1)
                .SelectMany(y => y)
                .ToList().ForEach(e => e.IsDuplicate = true);

            workOrderPriceModels = workOrderPriceModels.OrderBy(r => r.WorkOrderNumber).ToList();

            return workOrderPriceModels;
        }

        //
        // GET: api/WorkOrderPrice/Editor
        [HttpGet]
        [HttpPost]
        public DtResponse Editor()
        {
            var user = userService.GetCurrentUser();
            var workOrderPriceModels = new List<WorkOrderPriceModel>();

            if (user != null)
            {
                var project = projectService.GetById(user.ProjectId);

                var httpData = DatatableHelpers.HttpData();
                Dictionary<string, object> data = httpData["data"] as Dictionary<string, object>;
                var action = httpData["action"];


                var workOrderPrices = new List<WorkOrderPriceBO>();
                foreach (string workOrderPriceId in data.Keys)
                {
                    var row = data[workOrderPriceId];
                    var workOrderPriceProperties = row as Dictionary<string, object>;

                    WorkOrderPriceBO workOrderPrice = new WorkOrderPriceBO();

                    workOrderPrice.WorkOrderPriceId = !string.IsNullOrWhiteSpace(workOrderPriceProperties["WorkOrderPriceId"].ToString()) ? Convert.ToInt32(workOrderPriceProperties["WorkOrderPriceId"]) : 0;
                    workOrderPrice.CostPrice = !string.IsNullOrWhiteSpace(workOrderPriceProperties["CostPrice"].ToString()) ? Convert.ToDouble(workOrderPriceProperties["CostPrice"]) : 0;
                    workOrderPrice.ProjectId = user.ProjectId;
                    workOrderPrice.ReleasedPercent = !string.IsNullOrWhiteSpace(workOrderPriceProperties["ReleasedPercent"].ToString()) ? Convert.ToDouble(workOrderPriceProperties["ReleasedPercent"]) : 0;
                    workOrderPrice.SalePrice = !string.IsNullOrWhiteSpace(workOrderPriceProperties["SalePrice"].ToString()) ? Convert.ToDouble(workOrderPriceProperties["SalePrice"]) : 0;
                    workOrderPrice.ShippedPercent = !string.IsNullOrWhiteSpace(workOrderPriceProperties["ShippedPercent"].ToString()) ? Convert.ToDouble(workOrderPriceProperties["ShippedPercent"]) : 0;
                    workOrderPrice.WorkOrderNumber = workOrderPriceProperties["WorkOrderNumber"].ToString();


                    workOrderPrices.Add(workOrderPrice);
                }


                if (action.Equals(EditorActions.edit.ToString()))
                {
                    workOrderPriceService.UpdateAll(workOrderPrices);
                }
                else if (action.Equals(EditorActions.create.ToString()))
                {
                    workOrderPriceService.SaveAll(workOrderPrices);
                }
                else if (action.Equals(EditorActions.remove.ToString()))
                {
                    workOrderPriceService.Delete(workOrderPrices.FirstOrDefault().WorkOrderPriceId);
                }

                var allWorkOrderPrices = workOrderPriceService.GetAll(user.ProjectId);
                workOrderPriceModels = workOrderPrices.Select(x => new WorkOrderPriceModel
                {
                    CostPrice = x.CostPrice,
                    ProjectId = x.ProjectId,
                    ReleasedPercent = x.ReleasedPercent,
                    SalePrice = x.SalePrice,
                    ShippedPercent = x.ShippedPercent,
                    WorkOrderNumber = x.WorkOrderNumber,
                    WorkOrderPriceId = x.WorkOrderPriceId,
                    IsDuplicate = allWorkOrderPrices.Any(w =>
                       w.WorkOrderPriceId != x.WorkOrderPriceId &&
                       (w.WorkOrderNumber ?? string.Empty).Equals((x.WorkOrderNumber ?? string.Empty), StringComparison.InvariantCultureIgnoreCase))
                }).ToList();
            }

            return new DtResponse { data = workOrderPriceModels };
        }



    }
}
