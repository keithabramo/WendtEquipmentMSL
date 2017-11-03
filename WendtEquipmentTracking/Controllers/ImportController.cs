using AutoMapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
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
        public ActionResult SelectEquipmentFile(ImportModel model)
        {
            var equipmentImportModel = new EquipmentImportModel();

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

                        equipmentImportModel.Priorities = priorities;
                        equipmentImportModel.QuantityMultiplier = 1;
                        equipmentImportModel.WorkOrderNumber = projectNumber;
                        equipmentImportModel.DrawingNumber = Path.GetFileNameWithoutExtension(model.File.FileName);
                        equipmentImportModel.FilePath = filePath;

                        return PartialView("EquipmentConfigurationPartial", equipmentImportModel);
                    }
                    else
                    {
                        ModelState.AddModelError("File", "You must specify a file.");
                    }
                }

                HandleError("There was an issue while trying to load this equipment file", ModelState);

                return PartialView("EquipmentConfigurationPartial", equipmentImportModel);
            }
            catch (Exception e)
            {
                HandleError("There was an error while trying to load this equipment file", e);
                return PartialView("EquipmentConfigurationPartial", equipmentImportModel);
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

                HandleError("There was an issue while trying to setup this equipment file", ModelState);

                return PartialView("EquipmentConfigurationPartial", model);
            }
            catch (Exception e)
            {
                HandleError("There was an error while trying to setup this equipment file", e);

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

                    SuccessMessage("Equipment Import was successful");
                    return RedirectToAction("Index", "Home");
                }

                HandleError("There was an issue while Importing this equipment", ModelState);

                return View("Equipment");
            }
            catch (Exception e)
            {
                HandleError("There was an issue while Importing this equipment", e);

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

                HandleError("There was an issue while loading this file", ModelState);

                return View(model);
            }
            catch (Exception e)
            {
                HandleError("There was an error while loading this file", e);
                return View(model);
            }
        }

        // POST: SaveWorkOrderPrices
        [HttpPost]
        public ActionResult SaveWorkOrderPrice(IEnumerable<WorkOrderPriceSelectionModel> model)
        {
            try
            {
                var user = userService.GetCurrentUser();

                if (user != null)
                {
                    model.ToList().ForEach(c => c.ProjectId = user.ProjectId);

                    var workOrderPriceBOs = Mapper.Map<IEnumerable<WorkOrderPriceBO>>(model.Where(m => m.Checked).ToList());
                    workOrderPriceService.SaveAll(workOrderPriceBOs);

                    SuccessMessage("Import successful!");
                    return RedirectToAction("Index", "WorkOrderPrice");
                }

                HandleError("There was an issue while importing these work order prices", ModelState);

                return View("ImportWorkOrderPrice", model);
            }
            catch (Exception e)
            {
                HandleError("There was an error while importing these work order prices", e);

                return View("ImportWorkOrderPrice", model);
            }
        }

    }
}
