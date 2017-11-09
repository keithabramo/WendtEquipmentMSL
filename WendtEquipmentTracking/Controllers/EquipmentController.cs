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
            var priorities = prioritiesBOs.Select(p => p.PriorityNumber).OrderBy(p => p).ToList();

            ViewBag.ProjectNumber = projectService.GetById(user.ProjectId).ProjectNumber;
            ViewBag.Priorities = priorities;

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
                Priorities = priorities
            });
        }


        //GET Equipment/BOLsAssociatedToEquipment/5
        [HttpGet]
        public ActionResult BOLsAssociatedToEquipment(int id)
        {
            var equipment = equipmentService.GetById(id);

            var model = equipment.BillOfLadingEquipments.Select(x => new BillOfLadingEquipmentModel
            {
                BillOfLadingEquipmentId = x.BillOfLadingEquipmentId,
                BillOfLadingId = x.BillOfLadingId,
                EquipmentId = x.EquipmentId,
                Quantity = x.Quantity,
                ShippedFrom = x.ShippedFrom,
                BillOfLading = new BillOfLadingModel
                {
                    BillOfLadingId = x.BillOfLading.BillOfLadingId,
                    BillOfLadingNumber = x.BillOfLading.BillOfLadingNumber,
                    Carrier = x.BillOfLading.Carrier,
                    DateShipped = x.BillOfLading.DateShipped,
                    FreightTerms = x.BillOfLading.FreightTerms,
                    IsCurrentRevision = x.BillOfLading.IsCurrentRevision,
                    ProjectId = x.BillOfLading.ProjectId,
                    Revision = x.BillOfLading.Revision,
                    ToStorage = x.BillOfLading.ToStorage,
                    TrailerNumber = x.BillOfLading.TrailerNumber
                },
            });

            return PartialView(model);
        }
    }
}
