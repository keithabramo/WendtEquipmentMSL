
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using WendtEquipmentTracking.App.Models;
using WendtEquipmentTracking.BusinessLogic;
using WendtEquipmentTracking.BusinessLogic.Api;

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
            var priorities = prioritiesBOs.Select(p => p.PriorityNumber).OrderBy(p => p).ToList();
            var project = projectService.GetById(user.ProjectId);

            ViewBag.ProjectNumber = project.ProjectNumber + (!string.IsNullOrWhiteSpace(project.ShipToCompany) ? ": " + project.ShipToCompany : "");
            ViewBag.Priorities = priorities;

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

                        double projectNumber = 0;
                        IEnumerable<int> priorities = new List<int>();
                        if (user != null)
                        {
                            var projectBO = projectService.GetById(user.ProjectId);
                            var priorityBOs = priorityService.GetAll(user.ProjectId);

                            projectNumber = projectBO.ProjectNumber;
                            priorities = priorityBOs.Select(p => p.PriorityNumber).OrderBy(p => p).ToList();
                        }

                        equipmentImportModel.Priorities = priorities;
                        equipmentImportModel.QuantityMultiplier = 1;
                        equipmentImportModel.WorkOrderNumber = projectNumber == 0 ? string.Empty : projectNumber.ToString();
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
