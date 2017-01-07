using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WendtEquipmentTracking.App.Common;
using WendtEquipmentTracking.App.Models;
using WendtEquipmentTracking.BusinessLogic;
using WendtEquipmentTracking.BusinessLogic.Api;
using WendtEquipmentTracking.BusinessLogic.BO;

namespace WendtEquipmentTracking.App.Controllers
{
    public class HardwareKitController : Controller
    {
        private IHardwareKitService hardwareKitService;
        private IProjectService projectService;

        public HardwareKitController()
        {
            hardwareKitService = new HardwareKitService();
            projectService = new ProjectService();
        }

        //
        // GET: /HardwareKit/

        public ActionResult Index()
        {
            var projectIdCookie = CookieHelper.Get("ProjectId");

            if (string.IsNullOrEmpty(projectIdCookie))
            {
                return RedirectToAction("Index", "Home");
            }

            var projectId = Convert.ToInt32(projectIdCookie);

            var projectBO = projectService.GetById(projectId);

            if (projectBO == null)
            {
                CookieHelper.Delete("ProjectId");
                return RedirectToAction("Index", "Home");
            }

            var hardwareKitModels = Mapper.Map<IEnumerable<HardwareKitModel>>(projectBO.HardwareKits);

            //Filter and sort data

            hardwareKitModels = hardwareKitModels.OrderBy(r => r.HardwareKitNumber);

            return View(hardwareKitModels);
        }

        //
        // GET: /HardwareKit/Details/5

        public ActionResult Details(string hardwareKitNumber)
        {
            var hardwareKits = hardwareKitService.GetByHardwareKitNumber(hardwareKitNumber);

            if (hardwareKits == null)
            {
                return HttpNotFound();
            }

            var model = Mapper.Map<IEnumerable<HardwareKitModel>>(hardwareKits);

            return View(model);
        }

        //
        // GET: /HardwareKit/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /HardwareKit/Create

        [HttpPost]
        public ActionResult Create(HardwareKitModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var projectIdCookie = CookieHelper.Get("ProjectId");

                    if (!string.IsNullOrEmpty(projectIdCookie))
                    {
                        var projectId = Convert.ToInt32(projectIdCookie);
                        model.ProjectId = projectId;

                        var hardwareKitBO = Mapper.Map<HardwareKitBO>(model);

                        hardwareKitService.Save(hardwareKitBO);

                        return RedirectToAction("Index");
                    }
                }

                return View(model);
            }
            catch
            {
                return View(model);
            }
        }

        //
        // GET: /HardwareKit/Edit/5

        public ActionResult Edit(int id)
        {
            var hardwareKit = hardwareKitService.GetById(id);
            if (hardwareKit == null)
            {
                return HttpNotFound();
            }

            var hardwareKitModel = Mapper.Map<HardwareKitModel>(hardwareKit);

            return View(hardwareKitModel);
        }

        //
        // POST: /HardwareKit/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, HardwareKitModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var hardwareKit = hardwareKitService.GetById(id);

                    Mapper.Map<HardwareKitModel, HardwareKitBO>(model, hardwareKit);

                    hardwareKitService.Update(hardwareKit);

                    return RedirectToAction("Index");
                }

                return View(model);
            }
            catch
            {
                return View(model);
            }
        }


        // GET: HardwareKit/Delete/5
        public ActionResult Delete(int id)
        {
            var hardwareKit = hardwareKitService.GetById(id);

            if (hardwareKit == null)
            {
                return HttpNotFound();
            }

            var model = Mapper.Map<HardwareKitModel>(hardwareKit);

            return View(model);
        }

        // POST: HardwareKit/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, HardwareKitModel model)
        {
            try
            {
                var hardwareKit = hardwareKitService.GetById(id);

                if (hardwareKit == null)
                {
                    return HttpNotFound();
                }

                hardwareKitService.Delete(id);

                return RedirectToAction("Index");
            }
            catch
            {
                var hardwareKit = hardwareKitService.GetById(id);

                if (hardwareKit == null)
                {
                    return HttpNotFound();
                }

                model = Mapper.Map<HardwareKitModel>(hardwareKit);

                return View(model);
            }
        }

        //
        // GET: /Equipment/Hardware
        [ChildActionOnly]
        public ActionResult HardwareToAddToHardwareKit()
        {
            var projectIdCookie = CookieHelper.Get("ProjectId");

            if (string.IsNullOrEmpty(projectIdCookie))
            {
                return RedirectToAction("Index", "Home");
            }

            var projectId = Convert.ToInt32(projectIdCookie);

            //Get Data
            var projectBO = projectService.GetById(projectId);

            if (projectBO == null)
            {
                CookieHelper.Delete("ProjectId");
                return RedirectToAction("Index", "Home");
            }

            var equipmentBOs = projectBO.Equipments.Where(e => e.IsHardware && (e.HardwareKitEquipments == null || e.HardwareKitEquipments.Count() == 0));

            var equipmentModels = Mapper.Map<IEnumerable<EquipmentModel>>(equipmentBOs);
            equipmentModels.ToList().ForEach(e =>
            {
                e.ProjectNumber = projectBO.ProjectNumber;
                e.SetIndicators();
            });

            //Filter and sort data

            equipmentModels = equipmentModels.OrderBy(r => r.EquipmentId);

            var hardwareKitEquipments = equipmentModels.Select(e => new HardwareKitEquipmentModel
            {
                Equipment = e,
                EquipmentId = e.EquipmentId
            }).ToList();

            var model = new HardwareKitModel
            {
                HardwareKitEquipments = hardwareKitEquipments
            };

            return PartialView(model);
        }

    }
}
