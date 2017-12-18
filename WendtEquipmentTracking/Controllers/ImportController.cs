﻿
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
            var user = userService.GetCurrentUser();

            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var prioritiesBOs = priorityService.GetAll(user.ProjectId);
            var priorities = prioritiesBOs.Select(x => x.PriorityNumber).OrderBy(p => p).ToList();
            var project = projectService.GetById(user.ProjectId);

            ViewBag.ProjectNumber = project.ProjectNumber + (!string.IsNullOrWhiteSpace(project.ShipToCompany) ? ": " + project.ShipToCompany : "");
            ViewBag.Priorities = priorities;

            return View();
        }

        // POST: Equipment
        [HttpPost]
        public JsonResult SelectEquipmentFile(ImportModel model)
        {
            var equipmentImportModel = new EquipmentImportModel();

            try
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
                    var priority = priorityService.GetAll(user.ProjectId).FirstOrDefault();

                    //check to see if the file is in correct format
                    try
                    {
                        var importBO = new EquipmentImportBO
                        {
                            //DrawingNumber = string.Empty,
                            Equipment = string.Empty,
                            FilePaths = new Dictionary<string, string>() { { "", filePath } },
                            PriorityId = priority != null ? (int?)priority.PriorityId : null,
                            QuantityMultiplier = 1,
                            WorkOrderNumber = string.Empty
                        };

                        importService.GetEquipmentImport(importBO);
                    }
                    catch (Exception e)
                    {
                        return Json(new { Error = "The file does not conform to the expected format. Please make sure all column headers are spelled correctly and in the first row of the spreadsheet. Details: " + e.Message });
                    }

                    equipmentImportModel.DrawingNumber = Path.GetFileNameWithoutExtension(model.File.FileName);
                    equipmentImportModel.FilePath = filePath;

                    return Json(equipmentImportModel);
                }
                else
                {
                    return Json(new { Error = "You must specify a file." });
                }
            }
            catch (Exception e)
            {
                HandleError("There was an error", e);
                return Json(new { Error = "There was an error while trying to load this drawing file." });
            }
        }

        public ActionResult EquipmentConfigurationPartial()
        {
            var user = userService.GetCurrentUser();

            double projectNumber = 0;
            IEnumerable<PriorityModel> priorities = new List<PriorityModel>();
            if (user != null)
            {
                var projectBO = projectService.GetById(user.ProjectId);
                var priorityBOs = priorityService.GetAll(user.ProjectId);

                projectNumber = projectBO.ProjectNumber;
                priorities = priorityBOs.Select(x => new PriorityModel
                {
                    PriorityId = x.PriorityId,
                    PriorityNumber = x.PriorityNumber
                }).OrderBy(p => p.PriorityNumber).ToList();
            }

            var equipmentImportModel = new EquipmentImportModel();
            equipmentImportModel.Priorities = priorities;
            equipmentImportModel.QuantityMultiplier = 1;
            equipmentImportModel.WorkOrderNumber = projectNumber == 0 ? string.Empty : projectNumber.ToString();

            return PartialView(equipmentImportModel);
        }




        public ActionResult WorkOrderPrice()
        {
            var user = userService.GetCurrentUser();

            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        // POST: Equipment
        [HttpPost]
        public JsonResult SelectWorkOrderPriceFile(ImportModel model)
        {
            var workOrderPriceImportModel = new WorkOrderPriceImportModel();

            try
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


                    //check to see if the file is in correct format
                    try
                    {
                        var importBOs = importService.GetWorkOrderPricesImport(filePath);
                    }
                    catch (Exception e)
                    {
                        return Json(new { Error = "The file does not conform to the expected format. Please make sure all column headers are spelled correctly and in the first row of the spreadsheet. Details: " + e.Message });
                    }

                    workOrderPriceImportModel.FilePath = filePath;

                    return Json(workOrderPriceImportModel);
                }
                else
                {
                    return Json(new { Error = "You must specify a file." });
                }
            }
            catch (Exception e)
            {
                HandleError("There was an error", e);
                return Json(new { Error = "There was an error while trying to load this file." });
            }
        }



        public ActionResult RawEquipment()
        {
            var user = userService.GetCurrentUser();

            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var prioritiesBOs = priorityService.GetAll(user.ProjectId);
            var priorities = prioritiesBOs.Select(x => x.PriorityNumber).OrderBy(p => p).ToList();
            var project = projectService.GetById(user.ProjectId);

            ViewBag.ProjectNumber = project.ProjectNumber + (!string.IsNullOrWhiteSpace(project.ShipToCompany) ? ": " + project.ShipToCompany : "");
            ViewBag.Priorities = priorities;

            return View();
        }

        // POST: Equipment
        [HttpPost]
        public JsonResult SelectRawEquipmentFile(ImportModel model)
        {
            var rawEquipmentImportModel = new RawEquipmentImportModel();

            try
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


                    //check to see if the file is in correct format
                    try
                    {
                        var importBOs = importService.GetRawEquipmentImport(filePath);
                    }
                    catch (Exception e)
                    {
                        return Json(new { Error = "The file does not conform to the expected format. Please make sure all column headers are spelled correctly and in the first row of the spreadsheet. Details: " + e.Message });
                    }

                    rawEquipmentImportModel.FilePath = filePath;

                    return Json(rawEquipmentImportModel);
                }
                else
                {
                    return Json(new { Error = "You must specify a file." });
                }
            }
            catch (Exception e)
            {
                HandleError("There was an error", e);
                return Json(new { Error = "There was an error while trying to load this file." });
            }
        }
    }
}
