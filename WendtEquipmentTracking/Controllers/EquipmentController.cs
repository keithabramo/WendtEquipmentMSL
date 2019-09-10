using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WendtEquipmentTracking.App.Models;
using WendtEquipmentTracking.BusinessLogic;
using WendtEquipmentTracking.BusinessLogic.Api;

namespace WendtEquipmentTracking.App.Controllers
{
    public class EquipmentController : BaseController
    {
        private IEquipmentService equipmentService;
        private IProjectService projectService;
        private IUserService userService;
        private IPriorityService priorityService;

        public EquipmentController()
        {
            equipmentService = new EquipmentService();
            projectService = new ProjectService();
            userService = new UserService();
            priorityService = new PriorityService();
        }

        //
        // GET: /Equipment/
        public ActionResult Index(bool? ajaxSuccess)
        {
            if (ajaxSuccess.HasValue && ajaxSuccess.Value)
            {
                SuccessMessage("Equipment records were successfully imported.");
            }

            var user = userService.GetCurrentUser();

            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var prioritiesBOs = priorityService.GetAll(user.ProjectId);
            var priorities = prioritiesBOs.Select(x => x.PriorityNumber).OrderBy(p => p).ToList();
            var project = projectService.GetById(user.ProjectId);

            ViewBag.ProjectNumber = project.ProjectNumber + (!string.IsNullOrWhiteSpace(project.ShipToCompany) ? ": " + project.ShipToCompany : "");
            ViewBag.Priorities = priorities;
            ViewBag.ProjectId = project.ProjectId;

            return View();
        }


        //
        // GET: /Equipment/Create
        [ChildActionOnly]
        public ActionResult Create()
        {
            var user = userService.GetCurrentUser();

            IEnumerable<int> priorities = new List<int>();
            if (user != null)
            {
                var prioritiesBOs = priorityService.GetAll(user.ProjectId);

                priorities = prioritiesBOs.Select(p => p.PriorityNumber).OrderBy(p => p).ToList();
            }


            return PartialView(new EquipmentModel
            {
                Priorities = priorities,
                ShippedFrom = "WENDT",
                ReadyToShip = 0
            });
        }
    }
}
