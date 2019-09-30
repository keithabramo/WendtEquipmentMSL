using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WendtEquipmentTracking.App.Models;
using WendtEquipmentTracking.BusinessLogic;
using WendtEquipmentTracking.BusinessLogic.Api;
using WendtEquipmentTracking.Common;

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
            var vendors = vendorBOs.Select(x => new Select2Model
            {
                value = x.Name + " " + x.Address,
                label = x.Name + " " + x.Address
            }).OrderBy(p => p.label).ToList();
            ViewBag.Vendors = vendors;

            var projectBOs = projectService.GetAll();

            var projects = projectBOs.Select(x => new Select2Model {
                value = x.ProjectNumber.ToString(),
                label = x.ProjectNumber.ToString(),
            }).OrderBy(p => p.label).ToList();

            ViewBag.Projects = projects;

            var statuses = new List<string> {
                TruckingScheduleStatuses.RFP.ToString(),
                TruckingScheduleStatuses.Planned.ToString(),
                TruckingScheduleStatuses.Confirmed.ToString(),
                TruckingScheduleStatuses.Closed.ToString()
            };

            ViewBag.Statuses = statuses;

            return View();
        }

        //
        // GET: /TruckingSchedule/Create
        [ChildActionOnly]
        public ActionResult Create()
        {
            var statuses = new List<string> {
                TruckingScheduleStatuses.RFP.ToString(),
                TruckingScheduleStatuses.Planned.ToString(),
                TruckingScheduleStatuses.Confirmed.ToString(),
                TruckingScheduleStatuses.Closed.ToString()
            };

            var model = new TruckingScheduleModel
            {
                Statuses = statuses
            };

            return PartialView(model);
        }
    }
}
