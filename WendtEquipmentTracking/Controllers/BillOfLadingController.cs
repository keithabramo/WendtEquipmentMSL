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
    public class BillOfLadingController : BaseController
    {
        private IBillOfLadingService billOfLadingService;
        private IProjectService projectService;
        private IEquipmentService equipmentService;

        public BillOfLadingController()
        {
            billOfLadingService = new BillOfLadingService();
            projectService = new ProjectService();
            equipmentService = new EquipmentService();
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

            var billOfLadingBOs = billOfLadingService.GetAll().Where(b => b.ProjectId == projectId && b.IsCurrentRevision).ToList();

            var billOfLadingModels = Mapper.Map<IEnumerable<BillOfLadingModel>>(billOfLadingBOs);

            return View(billOfLadingModels);
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

            model = model.OrderByDescending(b => b.Revision).ToList();

            return View(model);
        }

        //
        // GET: /BillOfLading/Create

        public ActionResult Create()
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

            var model = new BillOfLadingModel
            {
                FreightTerms = projectBO.FreightTerms
            };

            return View(model);
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
                    var projectIdCookie = CookieHelper.Get("ProjectId");

                    if (!string.IsNullOrEmpty(projectIdCookie))
                    {
                        var projectId = Convert.ToInt32(projectIdCookie);
                        model.ProjectId = projectId;
                        model.BillOfLadingEquipments = model.BillOfLadingEquipments.Where(be => be.Quantity > 0 && be.Checked).ToList();

                        var billOfLadingBO = Mapper.Map<BillOfLadingBO>(model);

                        billOfLadingService.Save(billOfLadingBO);

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
                    var projectIdCookie = CookieHelper.Get("ProjectId");

                    if (!string.IsNullOrEmpty(projectIdCookie))
                    {
                        var projectId = Convert.ToInt32(projectIdCookie);
                        model.ProjectId = projectId;
                        model.BillOfLadingEquipments = model.BillOfLadingEquipments.Where(be => be.Quantity > 0 && be.Checked).ToList();

                        var billOfLading = billOfLadingService.GetById(id);

                        Mapper.Map<BillOfLadingModel, BillOfLadingBO>(model, billOfLading);

                        billOfLadingService.Update(billOfLading);

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


        //
        // GET: /BillOfLading/EquipmentToAddToBOL
        [ChildActionOnly]
        public ActionResult EquipmentToAddToBOL()
        {
            var projectIdCookie = CookieHelper.Get("ProjectId");

            if (string.IsNullOrEmpty(projectIdCookie))
            {
                return RedirectToAction("Index", "Home");
            }

            var projectId = Convert.ToInt32(projectIdCookie);

            var projectBO = projectService.GetById(projectId);

            //Get Data
            var equipmentBOs = equipmentService.GetAll().Where(e => e.ProjectId == projectId
                                                                    && e.ReadyToShip != null
                                                                    && e.ReadyToShip > 0
                                                                    && !e.IsHardware).ToList();

            var equipmentModels = Mapper.Map<IEnumerable<EquipmentModel>>(equipmentBOs);

            equipmentModels.ToList().ForEach(e =>
            {
                e.SetIndicators(projectBO.ProjectNumber, projectBO.IsCustomsProject);
            });

            //Filter and sort data

            equipmentModels = equipmentModels.OrderBy(r => r.EquipmentName);

            var billOfLadingEquipments = equipmentModels.Select(e => new BillOfLadingEquipmentModel
            {
                Equipment = e,
                EquipmentId = e.EquipmentId
            }).ToList();

            var model = new BillOfLadingModel
            {
                BillOfLadingEquipments = billOfLadingEquipments
            };

            return PartialView(model);
        }

        //
        // GET: /BillOfLading/EquipmentToEditForBOL
        [ChildActionOnly]
        public ActionResult EquipmentToEditForBOL(BillOfLadingModel model)
        {
            var projectIdCookie = CookieHelper.Get("ProjectId");

            if (string.IsNullOrEmpty(projectIdCookie))
            {
                return RedirectToAction("Index", "Home");
            }

            var projectId = Convert.ToInt32(projectIdCookie);
            var projectBO = projectService.GetById(projectId);

            //Get Data
            var equipmentBOs = equipmentService.GetAll().Where(e => e.ProjectId == projectId
                                                                    && e.ReadyToShip != null
                                                                    && e.ReadyToShip > 0
                                                                    && !e.IsHardware).ToList();

            var equipmentModels = Mapper.Map<IEnumerable<EquipmentModel>>(equipmentBOs);

            var billOfLadingEquipments = equipmentModels.Select(e => new BillOfLadingEquipmentModel
            {
                Equipment = e,
                EquipmentId = e.EquipmentId,
                Quantity = model.BillOfLadingEquipments.Any(be => be.EquipmentId == e.EquipmentId) ? model.BillOfLadingEquipments.FirstOrDefault(be => be.EquipmentId == e.EquipmentId).Quantity : 0,
                Checked = model.BillOfLadingEquipments.Any(be => be.EquipmentId == e.EquipmentId)
            }).ToList();

            var modelBOLEquipments = model.BillOfLadingEquipments.Where(be => equipmentModels == null || !equipmentModels.Any(fullbe => fullbe.EquipmentId == be.EquipmentId));

            billOfLadingEquipments.AddRange(modelBOLEquipments);
            billOfLadingEquipments.ToList().ForEach(e =>
            {
                e.Equipment.SetIndicators(projectBO.ProjectNumber, projectBO.IsCustomsProject);
            });

            model.BillOfLadingEquipments = billOfLadingEquipments;

            return PartialView("EquipmentToAddToBOL", model);
        }
    }
}
