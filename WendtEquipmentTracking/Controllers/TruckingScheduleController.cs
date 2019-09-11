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

        public TruckingScheduleController()
        {
            truckingScheduleService = new TruckingScheduleService();
            projectService = new ProjectService();
            userService = new UserService();
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
