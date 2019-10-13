using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using WendtEquipmentTracking.BusinessLogic;
using WendtEquipmentTracking.BusinessLogic.Api;

namespace WendtEquipmentTracking.App.Controllers
{
    public class AutoCompleteApiController : ApiController
    {
        private IVendorService vendorService;
        private IBrokerService brokerService;
        private IWorkOrderPriceService workOrderPriceService;
        private IUserService userService;
        private IProjectService projectService;

        public AutoCompleteApiController()
        {
            vendorService = new VendorService();
            workOrderPriceService = new WorkOrderPriceService();
            brokerService = new BrokerService();
            userService = new UserService();
            projectService = new ProjectService();
        }

        //
        // GET: api/AutocompleteApi/Vendor
        [HttpGet]
        public IEnumerable<string> Vendor()
        {
            var vendors = new List<string>();

            var user = userService.GetCurrentUser();

            if (user == null)
            {
                return vendors;
            }

            //Get Data
            var vendorBOs = vendorService.GetAll();

            vendors = vendorBOs.Select(x => x.Name)
                                .OrderBy(e => e)
                                .ToList();

            return vendors;
        }

        //
        // GET: api/AutocompleteApi/Broker
        [HttpGet]
        public IEnumerable<string> Broker()
        {
            var brokers = new List<string>();

            var user = userService.GetCurrentUser();

            if (user == null)
            {
                return brokers;
            }

            //Get Data
            var brokerBOs = brokerService.GetAll();

            brokers = brokerBOs.Select(x => x.Name)
                                .OrderBy(e => e)
                                .ToList();

            return brokers;
        }

        //
        // GET: api/AutocompleteApi/Vendor
        [HttpGet]
        public IEnumerable<double> Project()
        {
            var projects = new List<double>();

            var user = userService.GetCurrentUser();

            if (user == null)
            {
                return projects;
            }

            //Get Data
            var projectBOs = projectService.GetAll();

            projects = projectBOs.Select(x => x.ProjectNumber)
                                .OrderBy(e => e)
                                .ToList();

            return projects;
        }

        //
        // GET: api/AutocompleteApi/WorkOrderPrice
        [HttpGet]
        public IEnumerable<string> WorkOrderPrice(string term)
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

    }
}
