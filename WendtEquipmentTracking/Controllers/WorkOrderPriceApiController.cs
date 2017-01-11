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
        private IProjectService projectService;

        public WorkOrderPriceApiController()
        {
            workOrderPriceService = new WorkOrderPriceService();
            projectService = new ProjectService();
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
            var projectBO = projectService.GetById(projectId);

            if (projectBO == null)
            {
                CookieHelper.Delete("ProjectId");
                return workOrderPrices;
            }

            var workOrderPriceBOs = projectBO.WorkOrderPrices;

            workOrderPrices = workOrderPriceBOs
                                .Where(wop => wop.WorkOrderNumber.Contains(term))
                                .Select(wop => wop.WorkOrderNumber)
                                .OrderBy(e => e)
                                .ToList();

            return workOrderPrices;
        }
    }
}
