using AutoMapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using WendtEquipmentTracking.App.Common;
using WendtEquipmentTracking.App.Models;
using WendtEquipmentTracking.BusinessLogic;
using WendtEquipmentTracking.BusinessLogic.Api;
using WendtEquipmentTracking.BusinessLogic.BO;

namespace WendtEquipmentTracking.App.Controllers
{
    public class ImportController : BaseController
    {
        private IImportService importService;
        private IEquipmentService equipmentService;
        private IWorkOrderPriceService workOrderPriceService;
        private IUserService userService;
        private IPriorityService priorityService;
        private IProjectService projectService;


        public ImportController()
        {
            importService = new ImportService();
            equipmentService = new EquipmentService();
            workOrderPriceService = new WorkOrderPriceService();
            userService = new UserService();
            priorityService = new PriorityService();
            projectService = new ProjectService();
        }

        // GET: Equipment
        public ActionResult Equipment()
        {
            return View();
        }

        // POST: Equipment
        [HttpPost]
        public ActionResult SelectEquipmentFile(EquipmentImportModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    if (model.File != null)
                    {
                        byte[] file = null;
                        using (var memoryStream = new MemoryStream())
                        {
                            model.File.InputStream.CopyTo(memoryStream);
                            file = memoryStream.ToArray();
                        }

                        var filePath = importService.SaveEquipmentFile(file);

                        var user = userService.GetCurrentUser();

                        string projectNumber = string.Empty;
                        IEnumerable<int> priorities = new List<int>();
                        if (user != null)
                        {
                            var projectBO = projectService.GetById(user.ProjectId);
                            var priorityBOs = priorityService.GetAll(user.ProjectId);

                            projectNumber = projectBO.ProjectNumber;
                            priorities = priorityBOs.Select(p => p.PriorityNumber);
                        }

                        model.Priorities = priorities;
                        model.QuantityMultiplier = 1;
                        model.WorkOrderNumber = projectNumber;
                        model.DrawingNumber = Path.GetFileNameWithoutExtension(model.File.FileName);
                        model.FilePath = filePath;

                        return PartialView("EquipmentConfigurationPartial", model);
                    }
                    else
                    {
                        ModelState.AddModelError("File", "You must specify a file.");
                    }
                }

                model.Status = SuccessStatus.Error;
                return PartialView("EquipmentConfigurationPartial", model);
            }
            catch (Exception e)
            {
                model.Status = SuccessStatus.Error;
                ModelState.AddModelError("File", e.Message);
                return PartialView("EquipmentConfigurationPartial", model);
            }
        }

        // POST: Equipment
        [HttpPost]
        public ActionResult ConfigureEquipment(EquipmentImportModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var importBO = Mapper.Map<EquipmentImportBO>(model);
                    var equipmentBOs = importService.GetEquipmentImport(importBO);
                    var equipmentModels = Mapper.Map<IList<EquipmentSelectionModel>>(equipmentBOs);


                    var user = userService.GetCurrentUser();

                    IEnumerable<int> priorities = new List<int>();
                    if (user != null)
                    {
                        var priorityBOs = priorityService.GetAll(user.ProjectId);
                        priorities = priorityBOs.Select(p => p.PriorityNumber);
                    }


                    equipmentModels.ToList().ForEach(w =>
                    {
                        w.Checked = true;
                        w.Priorities = priorities;
                    });

                    return PartialView("ImportEquipmentPartial", equipmentModels);

                }

                model.Status = SuccessStatus.Error;
                return PartialView("EquipmentConfigurationPartial", model);
            }
            catch (Exception e)
            {
                model.Status = SuccessStatus.Error;
                ModelState.AddModelError("File", e.Message);
                return PartialView("EquipmentConfigurationPartial", model);
            }
        }

        // POST: SaveEquipment
        [HttpPost]
        public ActionResult SaveEquipment(IEnumerable<EquipmentSelectionModel> model)
        {

            var resultModel = new ImportModel();
            try
            {
                if (ModelState.IsValid)
                {
                    var user = userService.GetCurrentUser();

                    if (user != null)
                    {
                        model.ToList().ForEach(c => c.ProjectId = user.ProjectId);

                        var equipmentBOs = Mapper.Map<IEnumerable<EquipmentBO>>(model.Where(m => m.Checked).ToList());
                        equipmentBOs.ToList().ForEach(e =>
                        {
                            e.IsHardware = e.EquipmentName.Equals("hardware", StringComparison.InvariantCultureIgnoreCase);
                            e.UnitWeight = e.IsHardware ? .01 : e.UnitWeight;
                        });

                        equipmentService.SaveAll(equipmentBOs);

                        resultModel.Status = SuccessStatus.Success;
                        return RedirectToAction("Index", "Home");
                    }
                }

                IEnumerable<string> allErrors = ModelState.Values.SelectMany(v => v.Errors.Select(b => b.ErrorMessage));
                ModelState.AddModelError("", "There were some validation errors when trying to import equipment: \n" + String.Join("<br/>", allErrors));

                resultModel.Status = SuccessStatus.Error;

                return View("Equipment");
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);

                resultModel.Status = SuccessStatus.Error;

                return View("Equipment");
            }
        }



        // GET: WorkOrderPrice
        public ActionResult WorkOrderPrice()
        {
            var model = new ImportModel();

            return View(model);
        }

        // POST: WorkOrderPrice
        [HttpPost]
        public ActionResult WorkOrderPrice(ImportModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    if (model.File != null)
                    {
                        byte[] file = null;
                        using (var memoryStream = new MemoryStream())
                        {
                            model.File.InputStream.CopyTo(memoryStream);
                            file = memoryStream.ToArray();
                        }

                        var workOrderPriceBOs = importService.GetWorkOrderPricesImport(file);

                        var workOrderPriceModels = Mapper.Map<IList<WorkOrderPriceSelectionModel>>(workOrderPriceBOs);
                        workOrderPriceModels.ToList().ForEach(w => w.Checked = true);

                        return View("ImportWorkOrderPrice", workOrderPriceModels);
                    }
                    else
                    {
                        ModelState.AddModelError("File", "You must specify a file.");
                    }
                }

                model.Status = SuccessStatus.Error;
                return View(model);
            }
            catch (Exception e)
            {
                model.Status = SuccessStatus.Error;
                ModelState.AddModelError("File", e.Message);
                return View(model);
            }
        }

        // POST: SaveWorkOrderPrices
        [HttpPost]
        public ActionResult SaveWorkOrderPrice(IEnumerable<WorkOrderPriceSelectionModel> model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = userService.GetCurrentUser();

                    if (user != null)
                    {
                        model.ToList().ForEach(c => c.ProjectId = user.ProjectId);

                        var workOrderPriceBOs = Mapper.Map<IEnumerable<WorkOrderPriceBO>>(model.Where(m => m.Checked).ToList());
                        workOrderPriceService.SaveAll(workOrderPriceBOs);

                        return RedirectToAction("Index", "WorkOrderPrice");
                    }
                }

                return View("ImportWorkOrderPrice", model);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);

                return View("ImportWorkOrderPrice", model);
            }
        }

    }
}
