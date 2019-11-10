
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
    public class BillOfLadingAttachmentController : BaseController
    {
        private IBillOfLadingAttachmentService billOfLadingAttachmentService;

        public BillOfLadingAttachmentController()
        {
            billOfLadingAttachmentService = new BillOfLadingAttachmentService();
        }

        // GET: Equipment
        [ChildActionOnly]
        public ActionResult Index()
        {
            return PartialView();
        }

        // POST: SelectBillOfLadingAttachmentFile
        [HttpPost]
        public JsonResult SelectBillOfLadingAttachmentFile(BillOfLadingAttachmentFileModel model)
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

                    var billOfLadingAttachmentBO = new BillOfLadingAttachmentBO
                    {
                        BillOfLadingId = model.BillOfLadingId,
                        FileTitle = model.File.FileName,
                        FileName = Guid.NewGuid().ToString() + model.File.FileName
                    };

                    var filePath = Path.Combine(Server.MapPath("/Attachments"), billOfLadingAttachmentBO.FileName);

                    billOfLadingAttachmentService.Save(billOfLadingAttachmentBO, filePath, file);

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

        public ActionResult ViewAll(int billOfLadingId)
        {
            var model = billOfLadingAttachmentService.GetByBillOfLadingId(billOfLadingId).Select(x => x.FileName);

            return View(model);
        }
    }
}
