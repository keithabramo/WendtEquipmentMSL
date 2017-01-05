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
    public class BillOfLadingController : Controller
    {
        private IBillOfLadingService billOfLadingService;
        private IProjectService projectService;

        public BillOfLadingController()
        {
            billOfLadingService = new BillOfLadingService();
            projectService = new ProjectService();
        }

        //
        // GET: /BillOfLading/

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

            var billOfLAndingModels = Mapper.Map<IEnumerable<BillOfLadingModel>>(projectBO.BillOfLadings);

            //Filter and sort data

            billOfLAndingModels = billOfLAndingModels.OrderBy(r => r.DateShipped);

            return View(billOfLAndingModels);
        }

        //
        // GET: /BillOfLading/Details/5

        public ActionResult Details(string billOfLadingNumber)
        {
            var billOfLadings = billOfLadingService.GetByBillOfLadingNumber(billOfLadingNumber);

            if (billOfLadings == null)
            {
                return HttpNotFound();
            }

            var model = Mapper.Map<IEnumerable<BillOfLadingModel>>(billOfLadings);

            return View(model);
        }

        //
        // GET: /BillOfLading/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /BillOfLading/Create

        [HttpPost]
        public ActionResult Create(BillOfLadingModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var billOfLadingBO = Mapper.Map<BillOfLadingBO>(model);

                    billOfLadingService.Save(billOfLadingBO);

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
        // GET: /BillOfLading/Edit/5

        public ActionResult Edit(int id)
        {
            var billOfLading = billOfLadingService.GetById(id);
            if (billOfLading == null)
            {
                return HttpNotFound();
            }

            var billOfLadingModel = Mapper.Map<BillOfLadingModel>(billOfLading);

            return View(billOfLadingModel);
        }

        //
        // POST: /BillOfLading/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, BillOfLadingModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var billOfLading = billOfLadingService.GetById(id);

                    Mapper.Map<BillOfLadingModel, BillOfLadingBO>(model, billOfLading);

                    billOfLadingService.Update(billOfLading);

                    return RedirectToAction("Index");
                }

                return View(model);
            }
            catch
            {
                return View(model);
            }
        }


        // GET: BillOfLading/Delete/5
        public ActionResult Delete(int id)
        {
            var billOfLading = billOfLadingService.GetById(id);

            if (billOfLading == null)
            {
                return HttpNotFound();
            }

            var model = Mapper.Map<BillOfLadingModel>(billOfLading);

            return View(model);
        }

        // POST: BillOfLading/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, BillOfLadingModel model)
        {
            try
            {
                var billOfLading = billOfLadingService.GetById(id);

                if (billOfLading == null)
                {
                    return HttpNotFound();
                }

                billOfLadingService.Delete(id);

                return RedirectToAction("Index");
            }
            catch
            {
                var billOfLading = billOfLadingService.GetById(id);

                if (billOfLading == null)
                {
                    return HttpNotFound();
                }

                model = Mapper.Map<BillOfLadingModel>(billOfLading);

                return View(model);
            }
        }

    }
}
