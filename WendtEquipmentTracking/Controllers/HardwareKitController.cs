
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WendtEquipmentTracking.App.Models;
using WendtEquipmentTracking.BusinessLogic;
using WendtEquipmentTracking.BusinessLogic.Api;
using WendtEquipmentTracking.BusinessLogic.BO;

namespace WendtEquipmentTracking.App.Controllers
{
    public class HardwareKitController : BaseController
    {
        private IHardwareKitService hardwareKitService;
        private IProjectService projectService;
        private IEquipmentService equipmentService;
        private IUserService userService;

        private const float DEFAULT_PERCENT = 10;

        public HardwareKitController()
        {
            hardwareKitService = new HardwareKitService();
            projectService = new ProjectService();
            equipmentService = new EquipmentService();
            userService = new UserService();
        }

        //
        // GET: /HardwareKit/

        public ActionResult Index()
        {
            var user = userService.GetCurrentUser();

            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }


            var hardwareKitBOs = hardwareKitService.GetAll(user.ProjectId).Where(hk => hk.IsCurrentRevision).ToList();

            var hardwareKitModels = hardwareKitBOs.Select(x => new HardwareKitModel
            {
                ExtraQuantityPercentage = x.ExtraQuantityPercentage,
                HardwareKitId = x.HardwareKitId,
                HardwareKitNumber = x.HardwareKitNumber,
                IsCurrentRevision = x.IsCurrentRevision,
                ProjectId = x.ProjectId,
                Revision = x.Revision
            });

            return View(hardwareKitModels);
        }

        //
        // GET: /HardwareKit/Details/5

        public ActionResult Details(string hardwareKitNumber)
        {
            var user = userService.GetCurrentUser();

            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var hardwareKitBOs = hardwareKitService.GetByHardwareKitNumber(user.ProjectId, hardwareKitNumber);

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
                    QuantityToShip = (int)Math.Ceiling(g.Sum(e => e.QuantityToShip))
                }).ToList();

                var hardwareKitModel = new HardwareKitModel
                {
                    ExtraQuantityPercentage = hardwareKitBO.ExtraQuantityPercentage,
                    HardwareKitId = hardwareKitBO.HardwareKitId,
                    HardwareKitNumber = hardwareKitBO.HardwareKitNumber,
                    IsCurrentRevision = hardwareKitBO.IsCurrentRevision,
                    ProjectId = hardwareKitBO.ProjectId,
                    Revision = hardwareKitBO.Revision,
                    HardwareGroups = hardwareGroupModels
                };

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
                    var user = userService.GetCurrentUser();

                    if (user != null)
                    {
                        model.ProjectId = user.ProjectId;

                        var hardwareKitEquipmentsBOs = new List<HardwareKitEquipmentBO>();
                        foreach (var hardwareGroup in model.HardwareGroups.Where(m => m.Checked).ToList())
                        {
                            var hardwareInWorkOrderBOs = equipmentService.GetHardwareByShippingTagNumberAndDescription(user.ProjectId, hardwareGroup.ShippingTagNumber, hardwareGroup.Description);
                            hardwareKitEquipmentsBOs.AddRange(hardwareInWorkOrderBOs.Select(h => new HardwareKitEquipmentBO
                            {
                                EquipmentId = h.EquipmentId,
                                HardwareKitId = model.HardwareKitId,
                                QuantityToShip = h.Quantity.HasValue ? h.Quantity.Value + (h.Quantity.Value * (model.ExtraQuantityPercentage / 100)) : 0
                            }));
                        }


                        var hardwareKitBO = new HardwareKitBO
                        {
                            ExtraQuantityPercentage = model.ExtraQuantityPercentage,
                            HardwareKitId = model.HardwareKitId,
                            HardwareKitNumber = model.HardwareKitNumber,
                            IsCurrentRevision = model.IsCurrentRevision,
                            ProjectId = model.ProjectId,
                            Revision = model.Revision,
                            HardwareKitEquipments = hardwareKitEquipmentsBOs
                        };

                        hardwareKitService.Save(hardwareKitBO);

                        return RedirectToAction("Index");
                    }
                }

                return View(model);
            }
            catch (Exception e)
            {
                HandleError("There was an error attempting to create this hardware kit", e);
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
                    QuantityToShip = (int)Math.Ceiling(g.Sum(e => e.QuantityToShip)),
                    Checked = true
                }).ToList();

            var hardwareKitModel = new HardwareKitModel
            {
                ExtraQuantityPercentage = hardwareKitBO.ExtraQuantityPercentage,
                HardwareKitId = hardwareKitBO.HardwareKitId,
                HardwareKitNumber = hardwareKitBO.HardwareKitNumber,
                IsCurrentRevision = hardwareKitBO.IsCurrentRevision,
                ProjectId = hardwareKitBO.ProjectId,
                Revision = hardwareKitBO.Revision,
                HardwareGroups = hardwareGroupModels
            };

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
                    var user = userService.GetCurrentUser();

                    if (user != null)
                    {
                        model.ProjectId = user.ProjectId;

                        var hardwareKitEquipmentsBOs = new List<HardwareKitEquipmentBO>();
                        foreach (var hardwareGroup in model.HardwareGroups.Where(m => m.Checked).ToList())
                        {
                            var hardwareInWorkOrderBOs = equipmentService.GetHardwareByShippingTagNumberAndDescription(user.ProjectId, hardwareGroup.ShippingTagNumber, hardwareGroup.Description, model.HardwareKitId);
                            hardwareKitEquipmentsBOs.AddRange(hardwareInWorkOrderBOs.Select(h => new HardwareKitEquipmentBO
                            {
                                EquipmentId = h.EquipmentId,
                                HardwareKitId = model.HardwareKitId,
                                QuantityToShip = h.Quantity.HasValue ? h.Quantity.Value + (h.Quantity.Value * (model.ExtraQuantityPercentage / 100)) : 0
                            }));
                        }


                        var hardwareKitBO = new HardwareKitBO
                        {
                            ExtraQuantityPercentage = model.ExtraQuantityPercentage,
                            HardwareKitId = model.HardwareKitId,
                            HardwareKitNumber = model.HardwareKitNumber,
                            IsCurrentRevision = model.IsCurrentRevision,
                            ProjectId = model.ProjectId,
                            Revision = model.Revision,
                            HardwareKitEquipments = hardwareKitEquipmentsBOs
                        };

                        hardwareKitService.Update(hardwareKitBO);

                        return RedirectToAction("Index");
                    }
                }

                return View(model);
            }
            catch (Exception e)
            {
                HandleError("There was an error attempting to save this hardware kit", e);
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
            catch (Exception e)
            {
                HandleError("There was an error attempting to delete this hardware kit", e);

                return View(model);
            }
        }

        //
        // GET: /Equipment/Hardware
        [ChildActionOnly]
        public ActionResult HardwareToAddToHardwareKit()
        {
            var user = userService.GetCurrentUser();

            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }


            //Get Data
            var equipmentBOs = equipmentService
                .GetAll(user.ProjectId)
                .Where(e =>
                        e.IsHardware
                        && (e.HardwareKitEquipments == null
                            || e.HardwareKitEquipments.Count() == 0
                            || e.HardwareKitEquipments.Count(hke => hke.HardwareKit.IsCurrentRevision == true) == 0
                           )
                    );

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
            var user = userService.GetCurrentUser();

            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            //Get Data
            var equipmentBOs = equipmentService
                .GetAll(user.ProjectId)
                .Where(e =>
                        e.IsHardware
                        && (e.HardwareKitEquipments == null
                            || e.HardwareKitEquipments.Count() == 0
                            || e.HardwareKitEquipments.Count(hke => hke.HardwareKit.IsCurrentRevision == true) == 0
                           )
                    );

            var hardwareGroups = equipmentBOs.Where(hg => !model.HardwareGroups.Any(mhg => mhg.ShippingTagNumber == hg.ShippingTagNumber))
                .GroupBy(e => new { e.ShippingTagNumber, e.Description }, (key, g) => new HardwareKitGroupModel
                {
                    ShippingTagNumber = key.ShippingTagNumber,
                    Description = key.Description,
                    Quantity = g.Sum(e => e.Quantity.HasValue ? e.Quantity.Value : 0)
                }).ToList();

            var modelGroups = model.HardwareGroups.ToList();
            modelGroups.AddRange(hardwareGroups);

            model.HardwareGroups = modelGroups;

            return PartialView("HardwareToAddToHardwareKit", model);
        }

    }
}
