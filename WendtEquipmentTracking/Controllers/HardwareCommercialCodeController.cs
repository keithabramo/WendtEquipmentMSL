
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
        private IProjectService projectService;

        public HardwareCommercialCodeController()
        {
            hardwareCommercialCodeService = new HardwareCommercialCodeService();
            projectService = new ProjectService();
        }

        //
        // GET: /HardwareCommercialCode/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /HardwareCommercialCode/Details/5

        public ActionResult Details(int id)
        {
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
        // GET: /HardwareCommercialCode/Create

        public ActionResult Create()
        {
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
                        CommodityCode = model.CommodityCode,
                        Description = model.Description,
                        HardwareCommercialCodeId = model.HardwareCommercialCodeId,
                        PartNumber = model.PartNumber
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
                        CommodityCode = model.CommodityCode,
                        Description = model.Description,
                        HardwareCommercialCodeId = model.HardwareCommercialCodeId,
                        PartNumber = model.PartNumber
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


        // GET: HardwareCommercialCode/Delete/5
        public ActionResult Delete(int id)
        {
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

        // POST: HardwareCommercialCode/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, HardwareCommercialCodeModel model)
        {
            try
            {
                var hardwareCommercialCode = hardwareCommercialCodeService.GetById(id);

                if (hardwareCommercialCode == null)
                {
                    return HttpNotFound();
                }

                hardwareCommercialCodeService.Delete(id);

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                HandleError("There was an issue attempting to delete this Hardware/Commercial code", e);

                return View(model);
            }
        }

    }
}
