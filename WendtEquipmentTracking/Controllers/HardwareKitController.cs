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
        private IEquipmentService equipmentService;

        private const float DEFAULT_PERCENT = 10;

        public HardwareKitController()
        {
            hardwareKitService = new HardwareKitService();
            projectService = new ProjectService();
            equipmentService = new EquipmentService();
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

            var hardwareKitBOs = hardwareKitService.GetAll().Where(hk => hk.ProjectId == projectId && hk.IsCurrentRevision).ToList();

            var hardwareKitModels = Mapper.Map<IEnumerable<HardwareKitModel>>(hardwareKitBOs);

            return View(hardwareKitModels);
        }

        //
        // GET: /HardwareKit/Details/5

        public ActionResult Details(string hardwareKitNumber)
        {
            var hardwareKitBOs = hardwareKitService.GetByHardwareKitNumber(hardwareKitNumber);

            if (hardwareKitBOs == null)
            {
                return HttpNotFound();
            }

            var model = new List<HardwareKitModel>();

            foreach (var hardwareKitBO in hardwareKitBOs)
            {
                var hardwareGroupModels = hardwareKitBO.HardwareKitEquipments
                .GroupBy(e => new { e.Equipment.ShippingTagNumber, e.Equipment.Description }, (key, g) => new HardwareKitGroupModel
                {
                    ShippingTagNumber = key.ShippingTagNumber,
                    Description = key.Description,
                    Quantity = g.Sum(e => e.Equipment.Quantity.HasValue ? e.Equipment.Quantity.Value : 0),
                    QuantityToShip = (int)Math.Ceiling(g.Sum(e => e.Quantity))
                }).ToList();

                var hardwareKitModel = Mapper.Map<HardwareKitModel>(hardwareKitBO);
                hardwareKitModel.HardwareGroups = hardwareGroupModels;

                model.Add(hardwareKitModel);
            }


            model = model.OrderByDescending(h => h.Revision).ToList();

            return View(model);
        }

        //
        // GET: /HardwareKit/Create

        public ActionResult Create()
        {
            var percent = 10;

            return View(new HardwareKitModel { ExtraQuantityPercentage = percent });
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

                        var hardwareKitEquipmentsBOs = new List<HardwareKitEquipmentBO>();
                        foreach (var hardwareGroup in model.HardwareGroups.Where(m => m.Checked).ToList())
                        {
                            var hardwareInWorkOrderBOs = equipmentService.GetHardwareByShippingTagNumber(projectId, hardwareGroup.ShippingTagNumber);
                            hardwareKitEquipmentsBOs.AddRange(hardwareInWorkOrderBOs.Select(h => new HardwareKitEquipmentBO
                            {
                                EquipmentId = h.EquipmentId,
                                HardwareKitId = model.HardwareKitId,
                                Quantity = h.Quantity.HasValue ? h.Quantity.Value + (h.Quantity.Value * (model.ExtraQuantityPercentage / 100)) : 0
                            }));
                        }

                        var hardwareKitBO = Mapper.Map<HardwareKitBO>(model);

                        hardwareKitBO.HardwareKitEquipments = hardwareKitEquipmentsBOs;

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
            var hardwareKitBO = hardwareKitService.GetById(id);
            if (hardwareKitBO == null)
            {
                return HttpNotFound();
            }

            var hardwareGroupModels = hardwareKitBO.HardwareKitEquipments
                .GroupBy(e => new { e.Equipment.ShippingTagNumber, e.Equipment.Description }, (key, g) => new HardwareKitGroupModel
                {
                    ShippingTagNumber = key.ShippingTagNumber,
                    Description = key.Description,
                    Quantity = g.Sum(e => e.Equipment.Quantity.HasValue ? e.Equipment.Quantity.Value : 0),
                    QuantityToShip = (int)Math.Ceiling(g.Sum(e => e.Quantity)),
                    Checked = true
                }).ToList();

            var hardwareKitModel = Mapper.Map<HardwareKitModel>(hardwareKitBO);
            hardwareKitModel.HardwareGroups = hardwareGroupModels;

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
                    var projectIdCookie = CookieHelper.Get("ProjectId");

                    if (!string.IsNullOrEmpty(projectIdCookie))
                    {
                        var projectId = Convert.ToInt32(projectIdCookie);
                        model.ProjectId = projectId;

                        var hardwareKitBO = hardwareKitService.GetById(id);

                        var hardwareKitEquipmentsBOs = new List<HardwareKitEquipmentBO>();
                        foreach (var hardwareGroup in model.HardwareGroups.Where(m => m.Checked).ToList())
                        {
                            var hardwareInWorkOrderBOs = equipmentService.GetHardwareByShippingTagNumber(projectId, hardwareGroup.ShippingTagNumber);
                            hardwareKitEquipmentsBOs.AddRange(hardwareInWorkOrderBOs.Select(h => new HardwareKitEquipmentBO
                            {
                                EquipmentId = h.EquipmentId,
                                HardwareKitId = model.HardwareKitId,
                                Quantity = h.Quantity.HasValue ? h.Quantity.Value + (h.Quantity.Value * (model.ExtraQuantityPercentage / 100)) : 0
                            }));
                        }


                        Mapper.Map<HardwareKitModel, HardwareKitBO>(model, hardwareKitBO);

                        hardwareKitBO.HardwareKitEquipments = hardwareKitEquipmentsBOs;

                        hardwareKitService.Update(hardwareKitBO);

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


            var equipmentBOs = equipmentService.GetAll().Where(e => e.ProjectId == projectId
                                                                    && e.IsHardware
                                                                    && (e.HardwareKitEquipments == null || e.HardwareKitEquipments.Count() == 0));

            var hardwareGroups = equipmentBOs
                .GroupBy(e => new { e.ShippingTagNumber, e.Description }, (key, g) => new HardwareKitGroupModel
                {
                    ShippingTagNumber = key.ShippingTagNumber,
                    Description = key.Description,
                    Quantity = g.Sum(e => e.Quantity.HasValue ? e.Quantity.Value : 0),
                    Checked = true
                }).ToList();

            hardwareGroups.ForEach(hg => hg.QuantityToShip = (int)Math.Ceiling(hg.Quantity + (hg.Quantity * (DEFAULT_PERCENT / 100))));

            var model = new HardwareKitModel
            {
                HardwareGroups = hardwareGroups
            };

            return PartialView(model);
        }

        //
        // GET: /HardwareKit/EquipmentToEditForHardwareKit
        [ChildActionOnly]
        public ActionResult HardwareToEditForHardwareKit(HardwareKitModel model)
        {
            var projectIdCookie = CookieHelper.Get("ProjectId");

            if (string.IsNullOrEmpty(projectIdCookie))
            {
                return RedirectToAction("Index", "Home");
            }

            var projectId = Convert.ToInt32(projectIdCookie);

            //Get Data
            var equipmentBOs = equipmentService.GetAll().Where(e => e.ProjectId == projectId
                                                                    && e.IsHardware
                                                                    && (e.HardwareKitEquipments == null || e.HardwareKitEquipments.Count() == 0));

            var hardwareGroups = equipmentBOs.Where(hg => !model.HardwareGroups.Any(mhg => mhg.ShippingTagNumber == hg.ShippingTagNumber))
                .GroupBy(e => new { e.ShippingTagNumber, e.Description }, (key, g) => new HardwareKitGroupModel
                {
                    ShippingTagNumber = key.ShippingTagNumber,
                    Description = key.Description,
                    Quantity = g.Sum(e => e.Quantity.HasValue ? e.Quantity.Value : 0),
                    Checked = true
                }).ToList();



            model.HardwareGroups.ToList().AddRange(hardwareGroups);

            return PartialView("HardwareToAddToHardwareKit", model);
        }

    }
}
