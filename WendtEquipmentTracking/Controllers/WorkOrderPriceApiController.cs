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

        public WorkOrderPriceApiController()
        {
            workOrderPriceService = new WorkOrderPriceService();
            userService = new UserService();
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
                WorkOrderPriceId = x.WorkOrderPriceId
            });

            workOrderPriceModels = workOrderPriceModels.OrderBy(r => r.WorkOrderNumber);

            return workOrderPriceModels;
        }

        //
        // GET: api/WorkOrderPrice/Editor
        [HttpGet]
        [HttpPost]
        public DtResponse Editor()
        {
            var httpData = DatatableHelpers.HttpData();
            Dictionary<string, object> data = httpData["data"] as Dictionary<string, object>;

            var workOrderPrices = new List<WorkOrderPriceBO>();
            foreach (string workOrderPriceId in data.Keys)
            {
                var row = data[workOrderPriceId];
                var workOrderPriceProperties = row as Dictionary<string, object>;

                var workOrderPrice = workOrderPriceProperties.ToObject<WorkOrderPriceBO>();

                workOrderPrices.Add(workOrderPrice);
            }

            workOrderPriceService.UpdateAll(workOrderPrices);

            var workOrderPriceModels = workOrderPrices.Select(x => new WorkOrderPriceModel
            {
                CostPrice = x.CostPrice,
                ProjectId = x.ProjectId,
                ReleasedPercent = x.ReleasedPercent,
                SalePrice = x.SalePrice,
                ShippedPercent = x.ShippedPercent,
                WorkOrderNumber = x.WorkOrderNumber,
                WorkOrderPriceId = x.WorkOrderPriceId
            });

            return new DtResponse { data = workOrderPriceModels };
        }



    }
}
