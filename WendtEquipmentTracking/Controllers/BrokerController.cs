using System.Web.Mvc;
using WendtEquipmentTracking.BusinessLogic;
using WendtEquipmentTracking.BusinessLogic.Api;

namespace WendtEquipmentTracking.App.Controllers
{
    public class BrokerController : BaseController
    {
        private IBrokerService brokerService;
        private IProjectService projectService;
        private IUserService userService;

        public BrokerController()
        {
            brokerService = new BrokerService();
            projectService = new ProjectService();
            userService = new UserService();
        }

        //
        // GET: /Broker/

        public ActionResult Index(bool? ajaxSuccess)
        {
            if (ajaxSuccess.HasValue && ajaxSuccess.Value)
            {
                SuccessMessage("Brokers were successfully imported.");
            }

            var user = userService.GetCurrentUser();

            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        //
        // GET: /Broker/Create
        [ChildActionOnly]
        public ActionResult Create()
        {
            return PartialView();
        }
    }
}
