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
        private const int PAGE_SIZE = 30;
        private IEquipmentService equipmentService;
        private IProjectService projectService;
        private IWorkOrderPriceService workOrderPriceService;

        public EquipmentController()
        {
            equipmentService = new EquipmentService();
            projectService = new ProjectService();
            workOrderPriceService = new WorkOrderPriceService();
        }

        //
        // GET: /Equipment/
        public ActionResult Index()
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

            var workOrderPriceBOs = projectBO.WorkOrderPrices;

            var equipmentModels = Mapper.Map<IEnumerable<EquipmentModel>>(projectBO.Equipments);
            equipmentModels.ToList().ForEach(e =>
            {
                e.ProjectNumber = projectBO.ProjectNumber;
                e.SetIndicators();
                e.BillOfLadingEquipments.ToList().ForEach(b => b.BillOfLading.SetBillOfLadingIndicators());
                e.WorkOrders = workOrderPriceBOs.Select(w => w.WorkOrderNumber);
            });

            //Filter and sort data

            equipmentModels = equipmentModels.OrderBy(r => r.EquipmentId);

            return View(equipmentModels);
        }

        //
        // GET: /Equipment/ReadyToShip
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

            var equipmentBOs = projectBO.Equipments.Where(e => e.ReleaseDate != null && e.LeftToShip > 0 && !e.IsHardware);

            var equipmentModels = Mapper.Map<IEnumerable<EquipmentModel>>(equipmentBOs);
            equipmentModels.ToList().ForEach(e =>
            {
                e.ProjectNumber = projectBO.ProjectNumber;
                e.SetIndicators();
                e.BillOfLadingEquipments.ToList().ForEach(b => b.BillOfLading.SetBillOfLadingIndicators());
            });

            //Filter and sort data

            equipmentModels = equipmentModels.OrderBy(r => r.EquipmentId);

            return View(equipmentModels.ToList());
        }

        // POST: Equipment/ReadyToShip
        [HttpPost]
        public ActionResult ReadyToShip(IEnumerable<EquipmentModel> model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var projectIdCookie = CookieHelper.Get("ProjectId");

                    if (!string.IsNullOrEmpty(projectIdCookie))
                    {
                        var projectId = Convert.ToInt32(projectIdCookie);
                        model.ToList().ForEach(c => c.ProjectId = projectId);

                        var equipmentBOs = Mapper.Map<IEnumerable<EquipmentBO>>(model);
                        equipmentService.UpdateReadyToShip(equipmentBOs);

                        return RedirectToAction("Index", "Home");
                    }
                }

                return View(model);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);

                return View(model);
            }
        }

        //
        // GET: /Equipment/Create
        [ChildActionOnly]
        public ActionResult Create()
        {
            IEnumerable<string> workOrders = new List<string>();

            var projectIdCookie = CookieHelper.Get("ProjectId");

            if (!string.IsNullOrEmpty(projectIdCookie))
            {
                var projectId = Convert.ToInt32(projectIdCookie);

                //Get Data
                var projectBO = projectService.GetById(projectId);

                if (projectBO != null)
                {
                    var workOrderPriceBOs = projectBO.WorkOrderPrices;
                    workOrders = workOrderPriceBOs.Select(w => w.WorkOrderNumber);
                }
            }

            return PartialView(new EquipmentModel
            {
                WorkOrders = workOrders
            });
        }

        //
        // POST: /Equipment/Create

        [HttpPost]
        public ActionResult Create(EquipmentModel model)
        {
            IEnumerable<string> workOrders = new List<string>();
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

                        //Get Data
                        var projectBO = projectService.GetById(projectId);

                        if (projectBO != null)
                        {
                            var workOrderPriceBOs = projectBO.WorkOrderPrices;
                            workOrders = workOrderPriceBOs.Select(w => w.WorkOrderNumber);
                        }

                        newEquipmentModel.ProjectNumber = projectBO.ProjectNumber;
                        newEquipmentModel.SetIndicators();
                        newEquipmentModel.WorkOrders = workOrders;
                        return PartialView("Edit", newEquipmentModel);
                    }
                }

                model.WorkOrders = workOrders;
                return PartialView(model);
            }
            catch
            {
                model.WorkOrders = workOrders;
                return PartialView(model);
            }
        }


        //
        // POST: /Equipment/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, EquipmentModel model)
        {
            IEnumerable<string> workOrders = new List<string>();
            try
            {
                if (ModelState.IsValid)
                {
                    var projectIdCookie = CookieHelper.Get("ProjectId");

                    if (!string.IsNullOrEmpty(projectIdCookie))
                    {
                        var projectId = Convert.ToInt32(projectIdCookie);
                        model.ProjectId = projectId;
                        var equipment = equipmentService.GetById(id);

                        Mapper.Map<EquipmentModel, EquipmentBO>(model, equipment);

                        equipmentService.Update(equipment);

                        var updatedEquipmentBO = equipmentService.GetById(id);
                        var updatedEquipmentModel = Mapper.Map<EquipmentModel>(updatedEquipmentBO);

                        //Get Data
                        var projectBO = projectService.GetById(projectId);

                        if (projectBO != null)
                        {
                            var workOrderPriceBOs = projectBO.WorkOrderPrices;
                            workOrders = workOrderPriceBOs.Select(w => w.WorkOrderNumber);
                        }

                        updatedEquipmentModel.ProjectNumber = projectBO.ProjectNumber;
                        updatedEquipmentModel.SetIndicators();
                        updatedEquipmentModel.WorkOrders = workOrders;
                        return PartialView(updatedEquipmentModel);
                    }
                }

                model.WorkOrders = workOrders;
                return PartialView(model);
            }
            catch (Exception e)
            {
                model.WorkOrders = workOrders;
                return PartialView(model);
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
