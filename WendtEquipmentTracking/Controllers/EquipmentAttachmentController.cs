
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
    public class EquipmentAttachmentController : BaseController
    {
        private IEquipmentAttachmentService equipmentAttachmentService;

        public EquipmentAttachmentController()
        {
            equipmentAttachmentService = new EquipmentAttachmentService();
        }

        // GET: Equipment
        [ChildActionOnly]
        public ActionResult Index()
        {
            return PartialView();
        }

        // POST: SelectEquipmentAttachmentFile
        [HttpPost]
        public JsonResult SelectEquipmentAttachmentFile(EquipmentAttachmentFileModel model)
        {
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

                    var equipmentAttachmentBO = new EquipmentAttachmentBO
                    {
                        EquipmentId = model.EquipmentId,
                        FileTitle = model.File.FileName,
                        FileName = model.File.FileName + "_" + Guid.NewGuid().ToString()
                    };

                    var filePath = Path.Combine(Server.MapPath("/Attachments"), equipmentAttachmentBO.FileName);

                    equipmentAttachmentService.Save(equipmentAttachmentBO, filePath, file);

                    return Json(true);
                }
                else
                {
                    return Json(new { Error = "You must specify a file." });
                }
            }
            catch (Exception e)
            {
                HandleError("There was an error", e);
                return Json(new { Error = "There was an error while trying to save this attachment." });
            }
        }

        public ActionResult ViewAll(int equipmentId)
        {
            var model = equipmentAttachmentService.GetByEquipmentId(equipmentId).Select(x => x.FileName);

            return View(model);
        }
    }
}
