using System.Linq;
using System.Web.Mvc;
using WendtEquipmentTracking.BusinessLogic;
using WendtEquipmentTracking.BusinessLogic.Api;

namespace WendtEquipmentTracking.App.Controllers
{
    public class TruckingScheduleController : BaseController
    {
        private ITruckingScheduleService truckingScheduleService;
        private IProjectService projectService;
        private IUserService userService;
        private IVendorService vendorService;

        public TruckingScheduleController()
        {
            truckingScheduleService = new TruckingScheduleService();
            projectService = new ProjectService();
            userService = new UserService();
            vendorService = new VendorService();
        }

        //
        // GET: /TruckingSchedule/

        public ActionResult Index(bool? ajaxSuccess)
        {
            if (ajaxSuccess.HasValue && ajaxSuccess.Value)
            {
                SuccessMessage("Trucking Schedules were successfully imported.");
            }

            var user = userService.GetCurrentUser();

            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }


            var vendorBOs = vendorService.GetAll();
            var vendors = vendorBOs.Select(x => x.Name).OrderBy(p => p).ToList();

            ViewBag.Vendors = vendors;

            return View();
        }

        //
        // GET: /TruckingSchedule/Create
        [ChildActionOnly]
        public ActionResult Create()
        {
            return PartialView();
        }
    }
}
