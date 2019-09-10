using System;
using System.Linq;
using System.Web.Mvc;
using WendtEquipmentTracking.App.Models;
using WendtEquipmentTracking.BusinessLogic;
using WendtEquipmentTracking.BusinessLogic.Api;

namespace WendtEquipmentTracking.App.Controllers
{
    public class HardwareKitController : BaseController
    {
        private IHardwareKitService hardwareKitService;
        private IProjectService projectService;
        private IEquipmentService equipmentService;
        private IUserService userService;

        private const float DEFAULT_PERCENT = 10;

        public HardwareKitController()
        {
            hardwareKitService = new HardwareKitService();
            projectService = new ProjectService();
            equipmentService = new EquipmentService();
            userService = new UserService();
        }

        //
        // GET: /HardwareKit/

        public ActionResult Index(bool? ajaxSuccess)
        {
            if (ajaxSuccess.HasValue && ajaxSuccess.Value)
            {
                SuccessMessage("Hardware Kit was successfully saved.");
            }

            var user = userService.GetCurrentUser();

            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        //
        // GET: /HardwareKit/Details/5

        public ActionResult Details(int id)
        {
            var user = userService.GetCurrentUser();

            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var hardwareKit = hardwareKitService.GetById(id);

            if (hardwareKit == null)
            {
                return HttpNotFound();
            }

            var model = new HardwareKitModel
            {
                HardwareKitId = hardwareKit.HardwareKitId,
                HardwareKitNumber = hardwareKit.HardwareKitNumber
            };




            return View(model);
        }

        //
        // GET: /HardwareKit/Create

        public ActionResult Create()
        {
            var user = userService.GetCurrentUser();

            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var percent = 10;

            return View(new HardwareKitModel { ExtraQuantityPercentage = percent });
        }

        //
        // GET: /HardwareKit/Edit/5

        public ActionResult Edit(int id)
        {
            var hardwareKitBO = hardwareKitService.GetById(id);
            if (hardwareKitBO == null)
            {
                return HttpNotFound();
            }



            var hardwareKitModel = new HardwareKitModel
            {
                ExtraQuantityPercentage = hardwareKitBO.ExtraQuantityPercentage,
                HardwareKitId = hardwareKitBO.HardwareKitId,
                HardwareKitNumber = hardwareKitBO.HardwareKitNumber
            };

            return View(hardwareKitModel);
        }

        //GET Equipment/HardwareAssociatedToHardwareKit/5
        [HttpGet]
        public ActionResult HardwareAssociatedToHardwareKit(int id)
        {
            var hardwareKit = hardwareKitService.GetById(id);

            var hardwareGroupModels = hardwareKit.HardwareKitEquipments
                .GroupBy(e => new { e.Equipment.ShippingTagNumber, e.Equipment.Description }, (key, g) => new HardwareKitGroupModel
                {
                    ShippingTagNumber = key.ShippingTagNumber,
                    Description = key.Description,
                    Quantity = g.Sum(e => e.Equipment.Quantity.HasValue ? e.Equipment.Quantity.Value : 0),
                    QuantityToShip = (int)Math.Ceiling(g.Sum(e => e.QuantityToShip))
                }).OrderBy(x => x.ShippingTagNumber).ToList();

            return PartialView(hardwareGroupModels);
        }


    }
}
