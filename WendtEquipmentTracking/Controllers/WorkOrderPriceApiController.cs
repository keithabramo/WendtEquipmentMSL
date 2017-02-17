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
        private IUserService userService;

        public WorkOrderPriceApiController()
        {
            workOrderPriceService = new WorkOrderPriceService();
            userService = new UserService();
        }

        //
        // GET: api/UserApi/Search
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
            var workOrderPriceBOs = workOrderPriceService.GetAll().Where(w => w.ProjectId == user.ProjectId);

            workOrderPrices = workOrderPriceBOs
                                .Where(wop => wop.WorkOrderNumber.Contains(term))
                                .Select(wop => wop.WorkOrderNumber)
                                .OrderBy(e => e)
                                .ToList();

            return workOrderPrices;
        }
    }
}
