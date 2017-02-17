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

        public ImportController()
        {
            importService = new ImportService();
            equipmentService = new EquipmentService();
            workOrderPriceService = new WorkOrderPriceService();
            userService = new UserService();
        }

        // GET: Equipment
        public ActionResult Equipment()
        {
            var model = new ImportModel();

            return View(model);
        }

        // POST: Equipment
        [HttpPost]
        public ActionResult Equipment(ImportModel model)
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

                        var importBO = importService.GetSheets(file);

                        model.Sheets = importBO.Sheets.Select(s => new ImportSheetModel
                        {
                            Checked = true,
                            Name = s
                        }).ToList();

                        model.FileName = importBO.FileName;

                        return View("SelectEquipmentSheets", model);
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

        // POST: SelectEquipmentSheets
        [HttpPost]
        public ActionResult SelectEquipmentSheets(ImportModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var importBO = new ImportBO
                    {
                        Sheets = model.Sheets.Where(s => s.Checked).Select(s => s.Name),
                        FileName = model.FileName
                    };

                    var equipmentBOs = importService.GetEquipmentImport(importBO).ToList();

                    var equipmentModels = Mapper.Map<List<EquipmentSelectionModel>>(equipmentBOs);
                    equipmentModels.ForEach(e =>
                    {
                        e.Checked = true;
                        e.IsHardware = e.EquipmentName.Equals("HARDWARE", StringComparison.InvariantCultureIgnoreCase);
                    });

                    return PartialView("ImportEquipmentPartial", equipmentModels);
                }

                return PartialView("ImportEquipmentPartial");
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return PartialView("ImportEquipmentPartial");
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
