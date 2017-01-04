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
    public class BillOfLandingController : Controller
    {
        private IBillOfLandingService billOfLandingService;
        private IProjectService projectService;

        public BillOfLandingController()
        {
            billOfLandingService = new BillOfLandingService();
            projectService = new ProjectService();
        }

        //
        // GET: /BillOfLanding/

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

            var billOfLAndingModels = Mapper.Map<IEnumerable<BillOfLandingModel>>(projectBO.BillOfLandings);

            //Filter and sort data

            billOfLAndingModels = billOfLAndingModels.OrderBy(r => r.DateShipped);

            return View(billOfLAndingModels);
        }

        //
        // GET: /BillOfLanding/Details/5

        public ActionResult Details(string billOfLandingNumber)
        {
            var billOfLandings = billOfLandingService.GetByBillOfLandingNumber(billOfLandingNumber);

            if (billOfLandings == null)
            {
                return HttpNotFound();
            }

            var model = Mapper.Map<IEnumerable<BillOfLandingModel>>(billOfLandings);

            return View(model);
        }

        //
        // GET: /BillOfLanding/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /BillOfLanding/Create

        [HttpPost]
        public ActionResult Create(BillOfLandingModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var billOfLandingBO = Mapper.Map<BillOfLandingBO>(model);

                    billOfLandingService.Save(billOfLandingBO);

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
        // GET: /BillOfLanding/Edit/5

        public ActionResult Edit(int id)
        {
            var billOfLanding = billOfLandingService.GetById(id);
            if (billOfLanding == null)
            {
                return HttpNotFound();
            }

            var billOfLandingModel = Mapper.Map<BillOfLandingModel>(billOfLanding);

            return View(billOfLandingModel);
        }

        //
        // POST: /BillOfLanding/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, BillOfLandingModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var billOfLanding = billOfLandingService.GetById(id);

                    Mapper.Map<BillOfLandingModel, BillOfLandingBO>(model, billOfLanding);

                    billOfLandingService.Update(billOfLanding);

                    return RedirectToAction("Index");
                }

                return View(model);
            }
            catch
            {
                return View(model);
            }
        }


        // GET: BillOfLanding/Delete/5
        public ActionResult Delete(int id)
        {
            var billOfLanding = billOfLandingService.GetById(id);

            if (billOfLanding == null)
            {
                return HttpNotFound();
            }

            var model = Mapper.Map<BillOfLandingModel>(billOfLanding);

            return View(model);
        }

        // POST: BillOfLanding/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, BillOfLandingModel model)
        {
            try
            {
                var billOfLanding = billOfLandingService.GetById(id);

                if (billOfLanding == null)
                {
                    return HttpNotFound();
                }

                billOfLandingService.Delete(id);

                return RedirectToAction("Index");
            }
            catch
            {
                var billOfLanding = billOfLandingService.GetById(id);

                if (billOfLanding == null)
                {
                    return HttpNotFound();
                }

                model = Mapper.Map<BillOfLandingModel>(billOfLanding);

                return View(model);
            }
        }

    }
}
