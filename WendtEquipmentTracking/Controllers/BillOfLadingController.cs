using System.Linq;
using System.Web.Mvc;
using WendtEquipmentTracking.App.Models;
using WendtEquipmentTracking.BusinessLogic;
using WendtEquipmentTracking.BusinessLogic.Api;

namespace WendtEquipmentTracking.App.Controllers
{
    public class BillOfLadingController : BaseController
    {
        private IBillOfLadingService billOfLadingService;
        private IProjectService projectService;
        private IEquipmentService equipmentService;
        private IUserService userService;

        public BillOfLadingController()
        {
            billOfLadingService = new BillOfLadingService();
            projectService = new ProjectService();
            equipmentService = new EquipmentService();
            userService = new UserService();
        }

        //
        // GET: /BillOfLading/

        public ActionResult Index(bool? ajaxSuccess)
        {
            if (ajaxSuccess.HasValue && ajaxSuccess.Value)
            {
                SuccessMessage("Bill of Lading was successfully saved.");
            }

            var user = userService.GetCurrentUser();

            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }


            return View();
        }

        //
        // GET: /BillOfLading/Details/5

        public ActionResult Details(int id)
        {
            var billOfLading = billOfLadingService.GetById(id);

            if (billOfLading == null)
            {
                return HttpNotFound();
            }

            var model = new BillOfLadingModel
            {
                BillOfLadingId = billOfLading.BillOfLadingId,
                BillOfLadingNumber = billOfLading.BillOfLadingNumber
            };

            return View(model);
        }

        //
        // GET: /BillOfLading/Create

        public ActionResult Create()
        {
            var user = userService.GetCurrentUser();

            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }


            //Get Data
            var projectBO = projectService.GetById(user.ProjectId);

            if (projectBO == null)
            {
                userService.Delete();
                return RedirectToAction("Index", "Home");
            }

            var projectNumber = projectBO.ProjectNumber + (!string.IsNullOrWhiteSpace(projectBO.ShipToCompany) ? ": " + projectBO.ShipToCompany : "");

            ViewBag.ProjectNumber = projectNumber;

            var model = new BillOfLadingModel
            {
                FreightTerms = projectBO.FreightTerms,
                ShippedTo = projectNumber
            };

            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var billOfLading = billOfLadingService.GetById(id);
            if (billOfLading == null)
            {
                return HttpNotFound();
            }
            if (billOfLading.IsLocked)
            {
                InformationMessage("The Bill of Lading you are trying to edit is currently locked by " + billOfLading.LockedBy);
                return RedirectToAction("Index");
            }

            var user = userService.GetCurrentUser();

            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }


            //Get Data
            var projectBO = projectService.GetById(user.ProjectId);

            if (projectBO == null)
            {
                userService.Delete();
                return RedirectToAction("Index", "Home");
            }

            ViewBag.ProjectNumber = projectBO.ProjectNumber + (!string.IsNullOrWhiteSpace(projectBO.ShipToCompany) ? ": " + projectBO.ShipToCompany : "");



            var model = new BillOfLadingModel
            {
                BillOfLadingId = billOfLading.BillOfLadingId,
                BillOfLadingNumber = billOfLading.BillOfLadingNumber,
                Carrier = billOfLading.Carrier,
                DateShipped = billOfLading.DateShipped,
                FreightTerms = billOfLading.FreightTerms,
                IsCurrentRevision = billOfLading.IsCurrentRevision,
                ProjectId = billOfLading.ProjectId,
                Revision = billOfLading.Revision,
                ToStorage = billOfLading.ToStorage,
                TrailerNumber = billOfLading.TrailerNumber,
                ShippedFrom = billOfLading.ShippedFrom,
                ShippedTo = billOfLading.ShippedTo
            };

            return View(model);
        }

        //GET Equipment/BOLsAssociatedToEquipment/5
        [HttpGet]
        public ActionResult EquipmentAssociatedToBOL(int id)
        {
            var billOfLading = billOfLadingService.GetById(id);

            var model = billOfLading.BillOfLadingEquipments.Select(x => new BillOfLadingEquipmentModel
            {
                BillOfLadingEquipmentId = x.BillOfLadingEquipmentId,
                BillOfLadingId = x.BillOfLadingId,
                EquipmentId = x.EquipmentId,
                Quantity = x.Quantity,
                ShippedFrom = x.ShippedFrom,
                Equipment = new EquipmentModel
                {
                    EquipmentName = x.Equipment.EquipmentName,
                    Description = x.Equipment.Description,
                    ShippingTagNumber = x.Equipment.ShippingTagNumber,
                    WorkOrderNumber = x.Equipment.WorkOrderNumber,
                    ReadyToShip = x.Equipment.ReadyToShip,
                    LeftToShip = x.Equipment.LeftToShip.ToString(),
                    ShippedQuantity = x.Equipment.ShippedQuantity.ToString(),
                    Quantity = x.Equipment.Quantity.HasValue ? x.Equipment.Quantity.Value : 0,
                    ShippedFrom = x.Equipment.ShippedFrom,
                    HTSCode = x.Equipment.HTSCode,
                    CountryOfOrigin = x.Equipment.CountryOfOrigin
                },
            }).OrderBy(x => x.Equipment.ShippingTagNumber);

            return PartialView(model);
        }

    }
}
