using System;
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

        public ImportController()
        {
            importService = new ImportService();
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
                        var importBO = new ImportBO();

                        using (var memoryStream = new MemoryStream())
                        {
                            model.File.InputStream.CopyTo(memoryStream);
                            importBO.File = memoryStream.ToArray();
                        }

                        var sheets = importService.GetSheets(importBO);

                        model.Sheets = sheets.Select(s => new ImportSheetModel
                        {
                            Checked = true,
                            Name = s
                        }).ToList();

                        return View("ImportSelection", model);
                    }
                    else
                    {
                        ModelState.AddModelError("File", "You must specify a file.");
                    }
                }

                model.Status = ImportModel.ImportStatus.Error;
                return View(model);
            }
            catch (Exception e)
            {
                model.Status = ImportModel.ImportStatus.Error;
                ModelState.AddModelError("File", e.Message);
                return View(model);
            }
        }
    }
}
