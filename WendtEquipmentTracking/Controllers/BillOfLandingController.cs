using AutoMapper;
using System.Collections.Generic;
using System.Web.Mvc;
using WendtEquipmentTracking.App.Models;
using WendtEquipmentTracking.BusinessLogic;
using WendtEquipmentTracking.BusinessLogic.Api;
using WendtEquipmentTracking.BusinessLogic.BO;

namespace WendtEquipmentTracking.App.Controllers
{
    public class BillOfLandingController : Controller
    {
        private IBillOfLandingService billOfLandingService;
        private IEquipmentService equipmentService;

        public BillOfLandingController()
        {
            billOfLandingService = new BillOfLandingService();
            equipmentService = new EquipmentService();
        }

        //
        // GET: /BillOfLanding/

        public ActionResult Index(int equipmentId)
        {
            //Get Data
            var equipmentBO = equipmentService.GetById(equipmentId);

            var equipmentModel = Mapper.Map<EquipmentModel>(equipmentBO);


            return PartialView(equipmentModel);
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

            return PartialView(model);
        }

        //
        // GET: /BillOfLanding/Create

        public ActionResult Create(int equipmentId)
        {
            return PartialView(new BillOfLandingModel { EquipmentId = equipmentId });
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

                    return RedirectToAction("Index", new { EquipmentId = model.EquipmentId });
                }

                return PartialView(model);
            }
            catch
            {
                return PartialView(model);
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

            return PartialView(billOfLandingModel);
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

                    return RedirectToAction("Index", new { EquipmentId = model.EquipmentId });
                }

                return PartialView(model);
            }
            catch
            {
                return PartialView(model);
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

            return PartialView(model);
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

                return RedirectToAction("Index", new { EquipmentId = model.EquipmentId });
            }
            catch
            {
                var billOfLanding = billOfLandingService.GetById(id);

                if (billOfLanding == null)
                {
                    return HttpNotFound();
                }

                model = Mapper.Map<BillOfLandingModel>(billOfLanding);

                return PartialView(model);
            }
        }

    }
}
