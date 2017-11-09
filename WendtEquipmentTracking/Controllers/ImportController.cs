
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

                        var filePath = importService.SaveFile(file);

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
                else
                {
                    HandleError("There was an issue while trying to load this equipment file", ModelState);
                }


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
                    var importBO = new EquipmentImportBO
                    {
                        DrawingNumber = model.DrawingNumber,
                        Equipment = model.Equipment,
                        FilePath = model.FilePath,
                        Priority = model.Priority,
                        QuantityMultiplier = model.QuantityMultiplier,
                        WorkOrderNumber = model.WorkOrderNumber
                    };

                    var equipmentBOs = importService.GetEquipmentImport(importBO);

                    var equipmentModels = equipmentBOs.Select(x => new EquipmentSelectionModel
                    {
                        CountryOfOrigin = x.CountryOfOrigin,
                        CustomsValue = x.CustomsValue,
                        Description = x.Description,
                        DrawingNumber = x.DrawingNumber,
                        EquipmentId = x.EquipmentId,
                        EquipmentName = x.EquipmentName,
                        FullyShipped = x.FullyShipped,
                        HTSCode = x.HTSCode,
                        LeftToShip = x.LeftToShip.ToString(),
                        Notes = x.Notes,
                        Priority = x.Priority,
                        ProjectId = x.ProjectId,
                        Quantity = x.Quantity.HasValue ? x.Quantity.Value : 0,
                        ReadyToShip = x.ReadyToShip,
                        ReleaseDate = x.ReleaseDate,
                        SalePrice = x.SalePrice,
                        ShippedFrom = x.ShippedFrom,
                        ShippedQuantity = x.ShippedQuantity.ToString(),
                        ShippingTagNumber = x.ShippingTagNumber,
                        TotalWeight = x.TotalWeight.ToString(),
                        TotalWeightShipped = x.TotalWeightShipped.ToString(),
                        UnitWeight = x.UnitWeight,
                        WorkOrderNumber = x.WorkOrderNumber
                    }).ToList();



                    var user = userService.GetCurrentUser();

                    IEnumerable<int> priorities = new List<int>();
                    if (user != null)
                    {
                        var priorityBOs = priorityService.GetAll(user.ProjectId);
                        priorities = priorityBOs.Select(p => p.PriorityNumber);
                    }


                    equipmentModels.ForEach(w =>
                    {
                        w.Checked = true;
                        w.Priorities = priorities;
                    });


                    return PartialView("ImportEquipmentPartial", equipmentModels);

                }

                HandleError("There was an issue while trying to setup this equipment file", ModelState);

                return PartialView("ImportEquipmentPartial", new List<EquipmentSelectionModel>());
            }
            catch (Exception e)
            {
                HandleError("There was an error while trying to setup this equipment file", e);

                return PartialView("ImportEquipmentPartial", new List<EquipmentSelectionModel>());
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
                    model = model.Where(m => m.Checked);
                    model.ToList().ForEach(c => c.ProjectId = user.ProjectId);



                    var equipmentBOs = model.Select(x => new EquipmentBO
                    {
                        CountryOfOrigin = x.CountryOfOrigin,
                        CustomsValue = x.CustomsValue,
                        Description = x.Description,
                        DrawingNumber = x.DrawingNumber,
                        EquipmentId = x.EquipmentId,
                        EquipmentName = x.EquipmentName,
                        FullyShipped = x.FullyShipped,
                        HTSCode = x.HTSCode,
                        Notes = x.Notes,
                        Priority = x.Priority,
                        ProjectId = x.ProjectId,
                        Quantity = x.Quantity,
                        ReadyToShip = x.ReadyToShip,
                        ReleaseDate = x.ReleaseDate,
                        SalePrice = x.SalePrice,
                        ShippedFrom = x.ShippedFrom,
                        ShippingTagNumber = x.ShippingTagNumber,
                        UnitWeight = x.UnitWeight,
                        WorkOrderNumber = x.WorkOrderNumber,
                        IsHardware = x.EquipmentName.Equals("hardware", StringComparison.InvariantCultureIgnoreCase)
                    });
                    equipmentBOs.ToList().ForEach(e =>
                    {
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

                        var filePath = importService.SaveFile(file);


                        return View("ImportWorkOrderPrice", new WorkOrderPriceImportModel { FilePath = filePath });
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
    }
}
