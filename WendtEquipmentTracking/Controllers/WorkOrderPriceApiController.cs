using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using WendtEquipmentTracking.App.Common;
using WendtEquipmentTracking.BusinessLogic;
using WendtEquipmentTracking.BusinessLogic.Api;

namespace WendtEquipmentTracking.App.Controllers
{
    public class WorkOrderPriceApiController : ApiController
    {
        private IWorkOrderPriceService workOrderPriceService;

        public WorkOrderPriceApiController()
        {
            workOrderPriceService = new WorkOrderPriceService();
        }

        //
        // GET: api/UserApi/Search
        [HttpGet]
        public IEnumerable<string> Search(string term)
        {
            var workOrderPrices = new List<string>();

            var projectIdCookie = CookieHelper.Get("ProjectId");

            if (string.IsNullOrEmpty(projectIdCookie))
            {
                return workOrderPrices;
            }

            var projectId = Convert.ToInt32(projectIdCookie);

            //Get Data

            var workOrderPriceBOs = workOrderPriceService.GetAll().Where(w => w.ProjectId == projectId);

            workOrderPrices = workOrderPriceBOs
                                .Where(wop => wop.WorkOrderNumber.Contains(term))
                                .Select(wop => wop.WorkOrderNumber)
                                .OrderBy(e => e)
                                .ToList();

            return workOrderPrices;
        }
    }
}
