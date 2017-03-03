using AutoMapper;
using System.Collections.Generic;
using System.Linq;
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
            var hardwareCommercialCodeBOs = hardwareCommercialCodeService.GetAll().ToList();

            var hardwareCommercialCodeModels = Mapper.Map<IEnumerable<HardwareCommercialCodeModel>>(hardwareCommercialCodeBOs);

            //Filter and sort data

            hardwareCommercialCodeModels = hardwareCommercialCodeModels.OrderBy(r => r.PartNumber);

            return View(hardwareCommercialCodeModels);
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

            var model = Mapper.Map<HardwareCommercialCodeModel>(hardwareCommercialCode);

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

                    var hardwareCommercialCodeBO = Mapper.Map<HardwareCommercialCodeBO>(model);

                    hardwareCommercialCodeService.Save(hardwareCommercialCodeBO);

                    return RedirectToAction("Index");
                }

                return View(model);
            }
            catch
            {
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

            var hardwareCommercialCodeModel = Mapper.Map<HardwareCommercialCodeModel>(hardwareCommercialCode);

            return View(hardwareCommercialCodeModel);
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
                    var hardwareCommercialCode = hardwareCommercialCodeService.GetById(id);

                    Mapper.Map<HardwareCommercialCodeModel, HardwareCommercialCodeBO>(model, hardwareCommercialCode);

                    hardwareCommercialCodeService.Update(hardwareCommercialCode);

                    return RedirectToAction("Index");
                }

                return View(model);
            }
            catch
            {
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

            var model = Mapper.Map<HardwareCommercialCodeModel>(hardwareCommercialCode);

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
            catch
            {
                var hardwareCommercialCode = hardwareCommercialCodeService.GetById(id);

                if (hardwareCommercialCode == null)
                {
                    return HttpNotFound();
                }

                model = Mapper.Map<HardwareCommercialCodeModel>(hardwareCommercialCode);

                return View(model);
            }
        }

    }
}
