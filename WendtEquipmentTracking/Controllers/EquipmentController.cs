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
    public class EquipmentController : BaseController
    {
        private IEquipmentService equipmentService;
        private IProjectService projectService;

        public EquipmentController()
        {
            equipmentService = new EquipmentService();
            projectService = new ProjectService();
        }

        //
        // GET: /Equipment/
        public ActionResult Index()
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

            var equipmentModels = Mapper.Map<List<EquipmentModel>>(projectBO.Equipments);
            equipmentModels.ToList().ForEach(e =>
            {
                e.ProjectNumber = projectBO.ProjectNumber;
                e.SetIndicators();
                e.BillOfLadingEquipments.ToList().ForEach(b => b.BillOfLading.SetBillOfLadingIndicators());
            });

            var hardwareKitModels = Mapper.Map<IEnumerable<EquipmentModel>>(projectBO.HardwareKits);

            equipmentModels.AddRange(hardwareKitModels);

            

            equipmentModels = equipmentModels.OrderBy(r => r.EquipmentName).ToList();

            return View(equipmentModels);
        }

        //
        // GET: /Equipment/ReadyToShip
        public ActionResult ReadyToShip()
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

            var equipmentBOs = projectBO.Equipments.Where(e => e.ReleaseDate != null && e.LeftToShip > 0 && !e.IsHardware);

            var equipmentModels = Mapper.Map<IEnumerable<EquipmentModel>>(equipmentBOs);
            equipmentModels.ToList().ForEach(e =>
            {
                e.ProjectNumber = projectBO.ProjectNumber;
                e.SetIndicators();
                e.BillOfLadingEquipments.ToList().ForEach(b => b.BillOfLading.SetBillOfLadingIndicators());
            });

            //Filter and sort data

            equipmentModels = equipmentModels.OrderBy(r => r.EquipmentId);

            return View(equipmentModels.ToList());
        }

        // POST: Equipment/ReadyToShip
        [HttpPost]
        public ActionResult ReadyToShip(IEnumerable<EquipmentModel> model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var projectIdCookie = CookieHelper.Get("ProjectId");

                    if (!string.IsNullOrEmpty(projectIdCookie))
                    {
                        var projectId = Convert.ToInt32(projectIdCookie);
                        model.ToList().ForEach(c => c.ProjectId = projectId);

                        var equipmentBOs = Mapper.Map<IEnumerable<EquipmentBO>>(model);
                        equipmentService.UpdateReadyToShip(equipmentBOs);

                        return RedirectToAction("Index", "Home");
                    }
                }

                return View(model);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);

                return View(model);
            }
        }

        //
        // GET: /Equipment/Create
        [ChildActionOnly]
        public ActionResult Create()
        {
            return PartialView(new EquipmentModel());
        }

        //
        // POST: /Equipment/Create

        [HttpPost]
        public ActionResult Create(EquipmentModel model)
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

                        var equipmentBO = Mapper.Map<EquipmentBO>(model);

                        var id = equipmentService.Save(equipmentBO);


                        var newEquipmentBO = equipmentService.GetById(id);
                        var newEquipmentModel = Mapper.Map<EquipmentModel>(newEquipmentBO);

                        //Get Data
                        var projectBO = projectService.GetById(projectId);


                        newEquipmentModel.ProjectNumber = projectBO.ProjectNumber;
                        newEquipmentModel.SetIndicators();
                        return PartialView("Edit", newEquipmentModel);
                    }
                }
                return PartialView(model);
            }
            catch
            {
                return PartialView(model);
            }
        }


        //
        // POST: /Equipment/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, EquipmentModel model)
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
                        var equipment = equipmentService.GetById(id);

                        Mapper.Map<EquipmentModel, EquipmentBO>(model, equipment);

                        equipmentService.Update(equipment);

                        var updatedEquipmentBO = equipmentService.GetById(id);
                        var updatedEquipmentModel = Mapper.Map<EquipmentModel>(updatedEquipmentBO);

                        //Get Data
                        var projectBO = projectService.GetById(projectId);

                        updatedEquipmentModel.ProjectNumber = projectBO.ProjectNumber;
                        updatedEquipmentModel.SetIndicators();
                        return PartialView(updatedEquipmentModel);
                    }
                }

                return PartialView(model);
            }
            catch (Exception e)
            {
                return PartialView(model);
            }
        }

        // POST: Equipment/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, EquipmentModel model)
        {
            try
            {
                var equipment = equipmentService.GetById(id);

                if (equipment == null)
                {
                    return HttpNotFound();
                }

                equipmentService.Delete(id);

                return RedirectToAction("Index");
            }
            catch
            {
                var equipment = equipmentService.GetById(id);

                if (equipment == null)
                {
                    return HttpNotFound();
                }

                model = Mapper.Map<EquipmentModel>(equipment);

                return View(model);
            }
        }
    }
}
