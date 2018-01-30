
using System;
using System.Web.Mvc;
using WendtEquipmentTracking.App.Models;
using WendtEquipmentTracking.BusinessLogic;
using WendtEquipmentTracking.BusinessLogic.Api;
using WendtEquipmentTracking.BusinessLogic.BO;

namespace WendtEquipmentTracking.App.Controllers
{
    public class HardwareCommercialCodeController : BaseController
    {
        private IHardwareCommercialCodeService hardwareCommercialCodeService;
        private IUserService userService;


        public HardwareCommercialCodeController()
        {
            hardwareCommercialCodeService = new HardwareCommercialCodeService();
            userService = new UserService();
        }

        //
        // GET: /HardwareCommercialCode/

        public ActionResult Index()
        {
            var user = userService.GetCurrentUser();

            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }


        //
        // GET: /HardwareCommercialCode/Create

        public ActionResult Create()
        {
            var user = userService.GetCurrentUser();

            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        //
        // POST: /HardwareCommercialCode/Create

        [HttpPost]
        public ActionResult Create(HardwareCommercialCodeModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var hardwareCommercialCodeBO = new HardwareCommercialCodeBO
                    {
                        CommodityCode = model.CommodityCode.ToUpperInvariant(),
                        Description = model.Description.ToUpperInvariant(),
                        HardwareCommercialCodeId = model.HardwareCommercialCodeId,
                        PartNumber = model.PartNumber.ToUpperInvariant()
                    };

                    hardwareCommercialCodeService.Save(hardwareCommercialCodeBO);

                    return RedirectToAction("Index");
                }

                return View(model);
            }
            catch (Exception e)
            {

                HandleError("There was an error while trying to create this hardware/commerical code", e);

                return View(model);
            }
        }

        //
        // GET: /HardwareCommercialCode/Edit/5

        public ActionResult Edit(int id)
        {
            var user = userService.GetCurrentUser();

            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var hardwareCommercialCode = hardwareCommercialCodeService.GetById(id);
            if (hardwareCommercialCode == null)
            {
                return HttpNotFound();
            }

            var model = new HardwareCommercialCodeModel
            {
                CommodityCode = hardwareCommercialCode.CommodityCode,
                Description = hardwareCommercialCode.Description,
                HardwareCommercialCodeId = hardwareCommercialCode.HardwareCommercialCodeId,
                PartNumber = hardwareCommercialCode.PartNumber
            };

            return View(model);
        }

        //
        // POST: /HardwareCommercialCode/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, HardwareCommercialCodeModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var hardwareCommercialCodeBO = new HardwareCommercialCodeBO
                    {
                        CommodityCode = model.CommodityCode.ToUpperInvariant(),
                        Description = model.Description.ToUpperInvariant(),
                        HardwareCommercialCodeId = model.HardwareCommercialCodeId,
                        PartNumber = model.PartNumber.ToUpperInvariant()
                    };

                    hardwareCommercialCodeService.Update(hardwareCommercialCodeBO);

                    return RedirectToAction("Index");
                }

                return View(model);
            }
            catch (Exception e)
            {
                HandleError("There was an error attempting to save this hardware/commercial code", e);
                return View(model);
            }
        }
    }
}
