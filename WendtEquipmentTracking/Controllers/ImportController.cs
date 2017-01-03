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

        public ImportController()
        {
            importService = new ImportService();
            equipmentService = new EquipmentService();
        }

        // GET: Index
        public ActionResult Index()
        {
            var model = new ImportModel();

            return View(model);
        }

        // POST: Import
        [HttpPost]
        public ActionResult Index(ImportModel model)
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

                        return View("SelectSheets", model);
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

        // POST: SelectSheets
        [HttpPost]
        public ActionResult SelectSheets(ImportModel model)
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

                    var equipmentModels = Mapper.Map<IList<EquipmentImportModel>>(equipmentBOs);


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

        // POST: ImportEquipment
        [HttpPost]
        public ActionResult ImportEquipment(IEnumerable<EquipmentImportModel> model)
        {

            var resultModel = new ImportModel();
            try
            {
                if (ModelState.IsValid)
                {
                    var projectIdCookie = CookieHelper.Get("ProjectId");

                    if (!string.IsNullOrEmpty(projectIdCookie))
                    {
                        var projectId = Convert.ToInt32(projectIdCookie);
                        model.ToList().ForEach(c => c.ProjectId = projectId);

                        var equipmentBOs = Mapper.Map<IEnumerable<EquipmentBO>>(model.Where(m => m.Checked).ToList());
                        equipmentService.SaveAll(equipmentBOs);

                        resultModel.Status = SuccessStatus.Success;
                        return View("SelectSheets", resultModel);
                    }
                }

                resultModel.Status = SuccessStatus.Error;

                return View("SelectSheets", resultModel);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);

                resultModel.Status = SuccessStatus.Error;

                return View("SelectSheets", resultModel);
            }
        }
    }
}
