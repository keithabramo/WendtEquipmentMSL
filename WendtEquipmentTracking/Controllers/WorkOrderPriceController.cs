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
    public class WorkOrderPriceController : Controller
    {
        private IWorkOrderPriceService workOrderPriceService;
        private IProjectService projectService;

        public WorkOrderPriceController()
        {
            workOrderPriceService = new WorkOrderPriceService();
            projectService = new ProjectService();
        }

        //
        // GET: /WorkOrderPrice/

        public ActionResult Index()
        {
            var projectIdCookie = CookieHelper.Get("ProjectId");

            if (string.IsNullOrEmpty(projectIdCookie))
            {
                return RedirectToAction("Index", "Home");
            }

            var projectId = Convert.ToInt32(projectIdCookie);

            var workOrderBOs = workOrderPriceService.GetAll().Where(w => w.ProjectId == projectId).ToList();

            var workOrderPriceModels = Mapper.Map<IEnumerable<WorkOrderPriceModel>>(workOrderBOs);

            //Filter and sort data

            workOrderPriceModels = workOrderPriceModels.OrderBy(r => r.WorkOrderNumber);

            return View(workOrderPriceModels);
        }

        //
        // GET: /WorkOrderPrice/Details/5

        public ActionResult Details(int id)
        {
            var workOrderPrice = workOrderPriceService.GetById(id);

            if (workOrderPrice == null)
            {
                return HttpNotFound();
            }

            var model = Mapper.Map<WorkOrderPriceModel>(workOrderPrice);

            return View(model);
        }

        //
        // GET: /WorkOrderPrice/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /WorkOrderPrice/Create

        [HttpPost]
        public ActionResult Create(WorkOrderPriceModel model)
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

                        var workOrderPriceBO = Mapper.Map<WorkOrderPriceBO>(model);

                        workOrderPriceService.Save(workOrderPriceBO);

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
        // GET: /WorkOrderPrice/Edit/5

        public ActionResult Edit(int id)
        {
            var workOrderPrice = workOrderPriceService.GetById(id);
            if (workOrderPrice == null)
            {
                return HttpNotFound();
            }

            var workOrderPriceModel = Mapper.Map<WorkOrderPriceModel>(workOrderPrice);

            return View(workOrderPriceModel);
        }

        //
        // POST: /WorkOrderPrice/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, WorkOrderPriceModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var workOrderPrice = workOrderPriceService.GetById(id);

                    Mapper.Map<WorkOrderPriceModel, WorkOrderPriceBO>(model, workOrderPrice);

                    workOrderPriceService.Update(workOrderPrice);

                    return RedirectToAction("Index");
                }

                return View(model);
            }
            catch
            {
                return View(model);
            }
        }


        // GET: WorkOrderPrice/Delete/5
        public ActionResult Delete(int id)
        {
            var workOrderPrice = workOrderPriceService.GetById(id);

            if (workOrderPrice == null)
            {
                return HttpNotFound();
            }

            var model = Mapper.Map<WorkOrderPriceModel>(workOrderPrice);

            return View(model);
        }

        // POST: WorkOrderPrice/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, WorkOrderPriceModel model)
        {
            try
            {
                var workOrderPrice = workOrderPriceService.GetById(id);

                if (workOrderPrice == null)
                {
                    return HttpNotFound();
                }

                workOrderPriceService.Delete(id);

                return RedirectToAction("Index");
            }
            catch
            {
                var workOrderPrice = workOrderPriceService.GetById(id);

                if (workOrderPrice == null)
                {
                    return HttpNotFound();
                }

                model = Mapper.Map<WorkOrderPriceModel>(workOrderPrice);

                return View(model);
            }
        }

    }
}
