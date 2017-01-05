using AutoMapper;
using PagedList;
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
        private const int PAGE_SIZE = 30;
        private IEquipmentService equipmentService;
        private IProjectService projectService;

        public EquipmentController()
        {
            equipmentService = new EquipmentService();
            projectService = new ProjectService();
        }

        //
        // GET: /Equipment/

        public ActionResult Index(int? page)
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

            var equipmentModels = Mapper.Map<IEnumerable<EquipmentModel>>(projectBO.Equipments);

            //Filter and sort data

            equipmentModels = equipmentModels.OrderBy(r => r.EquipmentId);

            int pageNumber = (page ?? 1);

            return View(equipmentModels.ToPagedList(pageNumber, PAGE_SIZE));
        }

        //
        // GET: /Equipment/ReadyToShip
        [ChildActionOnly]
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

            var equipmentBOs = projectBO.Equipments.Where(e => e.ReadyToShip != null && e.ReadyToShip > 0);

            var equipmentModels = Mapper.Map<IEnumerable<EquipmentModel>>(equipmentBOs);

            //Filter and sort data

            equipmentModels = equipmentModels.OrderBy(r => r.EquipmentId);

            var billOfLadingEquipments = equipmentModels.Select(e => new BillOfLadingEquipmentModel
            {
                Equipment = e
            }).ToList();

            var model = new BillOfLadingModel
            {
                BillOfLadingEquipments = billOfLadingEquipments
            };

            return PartialView(model);
        }

        //
        // GET: /Equipment/Details/5

        //public ActionResult Details(int id)
        //{
        //    var equipment = equipmentService.GetById(id);

        //    if (equipment == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    var model = Mapper.Map<EquipmentModel>(equipment);

        //    return View(model);
        //}

        //
        // GET: /Equipment/Create
        [ChildActionOnly]
        public ActionResult Create()
        {
            return PartialView();
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

                        equipmentService.Save(equipmentBO);

                        return RedirectToAction("Index");
                    }
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        //
        // GET: /Equipment/Edit/5

        public ActionResult Edit(int id)
        {
            var equipment = equipmentService.GetById(id);
            if (equipment == null)
            {
                return HttpNotFound();
            }

            var equipmentModel = Mapper.Map<EquipmentModel>(equipment);

            return View(equipmentModel);
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
                    var equipment = equipmentService.GetById(id);

                    Mapper.Map<EquipmentModel, EquipmentBO>(model, equipment);

                    equipmentService.Update(equipment);

                    return RedirectToAction("Index");
                }

                return View(model);
            }
            catch
            {
                return View(model);
            }
        }


        // GET: Equipment/Delete/5
        public ActionResult Delete(int id)
        {
            var equipment = equipmentService.GetById(id);

            if (equipment == null)
            {
                return HttpNotFound();
            }

            var model = Mapper.Map<EquipmentModel>(equipment);

            return View(model);
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
