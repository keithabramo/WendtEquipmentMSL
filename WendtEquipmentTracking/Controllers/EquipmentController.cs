using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WendtEquipmentTracking.App.Common;
using WendtEquipmentTracking.App.Models;
using WendtEquipmentTracking.BusinessLogic;
using WendtEquipmentTracking.BusinessLogic.Api;

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

            //Get Data
            var equipmentBOs = equipmentService.GetAll(projectId);

            // the code that you want to measure comes here
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            var equipmentModels = Mapper.Map<List<EquipmentModel>>(equipmentBOs);
            
            return View(equipmentModels);
        }

        //
        // GET: /Equipment/Handsontable
        public ActionResult Handsontable()
        {
            return View();
        }

        //
        // GET: /Equipment/Create
        [ChildActionOnly]
        public ActionResult Create()
        {
            return PartialView(new EquipmentModel());
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
