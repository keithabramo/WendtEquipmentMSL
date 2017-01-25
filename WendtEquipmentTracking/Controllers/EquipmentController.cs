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
            var watch = System.Diagnostics.Stopwatch.StartNew();

            var projectIdCookie = CookieHelper.Get("ProjectId");

            if (string.IsNullOrEmpty(projectIdCookie))
            {
                return RedirectToAction("Index", "Home");
            }

            var projectId = Convert.ToInt32(projectIdCookie);
            var projectBO = projectService.GetById(projectId);

            //Get Data
            var equipmentBOs = equipmentService.GetSome(projectId, 0, 50).ToList();

            // the code that you want to measure comes here
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            var equipmentModels = Mapper.Map<List<EquipmentModel>>(equipmentBOs);
            equipmentModels.ToList().ForEach(e =>
            {
                e.SetIndicators(projectBO.ProjectNumber, projectBO.IsCustomsProject);
            });

            return View(equipmentModels);
        }

        public ActionResult Chunk(int skip, int take)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            var projectIdCookie = CookieHelper.Get("ProjectId");

            if (string.IsNullOrEmpty(projectIdCookie))
            {
                return RedirectToAction("Index", "Home");
            }

            var projectId = Convert.ToInt32(projectIdCookie);
            var projectBO = projectService.GetById(projectId);

            //Get Data
            var equipmentBOs = equipmentService.GetSome(projectId, skip, take).ToList();



            var equipmentModels = Mapper.Map<List<EquipmentModel>>(equipmentBOs);
            equipmentModels.ToList().ForEach(e =>
            {
                e.SetIndicators(projectBO.ProjectNumber, projectBO.IsCustomsProject);
            });

            // the code that you want to measure comes here
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            return PartialView(equipmentModels);
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

                        newEquipmentModel.Status = SuccessStatus.Success;
                        return PartialView("Chunk", new List<EquipmentModel> { newEquipmentModel });
                    }
                }
                model.Status = SuccessStatus.Error;
                return PartialView(model);
            }
            catch
            {
                model.Status = SuccessStatus.Error;
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
                        var projectBO = projectService.GetById(projectId);

                        model.ProjectId = projectId;
                        var equipment = equipmentService.GetById(id);

                        Mapper.Map<EquipmentModel, EquipmentBO>(model, equipment);

                        equipmentService.Update(equipment);

                        var updatedEquipmentBO = equipmentService.GetById(id);
                        var updatedEquipmentModel = Mapper.Map<EquipmentModel>(updatedEquipmentBO);

                        updatedEquipmentModel.SetIndicators(projectBO.ProjectNumber, projectBO.IsCustomsProject);
                        updatedEquipmentModel.Status = SuccessStatus.Success;
                        return PartialView("Chunk", new List<EquipmentModel> { updatedEquipmentModel });
                    }
                }

                model.Status = SuccessStatus.Error;
                return PartialView("Chunk", new List<EquipmentModel> { model });
            }
            catch (Exception e)
            {
                model.Status = SuccessStatus.Error;
                return PartialView("Chunk", new List<EquipmentModel> { model });
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
