using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using WendtEquipmentTracking.App.Common;
using WendtEquipmentTracking.BusinessLogic;
using WendtEquipmentTracking.BusinessLogic.Api;

namespace WendtEquipmentTracking.App.Controllers
{
    public class ValidateController : BaseController
    {
        private IProjectService projectService;


        public ValidateController()
        {
            projectService = new ProjectService();
        }

        // GET: ValidImportFile
        public ActionResult ValidImportFile(string file)
        {
            var extension = Path.GetExtension(file);

            //if (!extension.Equals(".xlsx"))
            //{
            //    return Json("Only xlsx files are allowed.", JsonRequestBehavior.AllowGet);
            //}

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        // GET: ValidBOLNumber
        public ActionResult ValidBOLNumber(string billOfLadingNumber, int? billOfLadingId)
        {
            var projectIdCookie = CookieHelper.Get("ProjectId");

            if (string.IsNullOrEmpty(projectIdCookie))
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            var projectId = Convert.ToInt32(projectIdCookie);

            var projectBO = projectService.GetById(projectId);

            if (projectBO == null)
            {
                CookieHelper.Delete("ProjectId");
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            var exists = projectBO.BillOfLadings.Any(b => b.IsCurrentRevision && b.BillOfLadingId != billOfLadingId && b.BillOfLadingNumber == billOfLadingNumber);

            return Json(!exists, JsonRequestBehavior.AllowGet);
        }

        // GET: ValidWorkOrderNumber
        public ActionResult ValidWorkOrderNumber(string workOrderNumber, int? workOrderPriceId)
        {
            var projectIdCookie = CookieHelper.Get("ProjectId");

            if (string.IsNullOrEmpty(projectIdCookie))
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            var projectId = Convert.ToInt32(projectIdCookie);

            var projectBO = projectService.GetById(projectId);

            if (projectBO == null)
            {
                CookieHelper.Delete("ProjectId");
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            var exists = projectBO.WorkOrderPrices.Any(b => b.WorkOrderPriceId != workOrderPriceId && b.WorkOrderNumber == workOrderNumber);

            return Json(!exists, JsonRequestBehavior.AllowGet);
        }

        // GET: ValidWorkOrderNumber
        public ActionResult ValidHardwareKitNumber(string hardwareKitNumber, int? hardwareKitId)
        {
            var projectIdCookie = CookieHelper.Get("ProjectId");

            if (string.IsNullOrEmpty(projectIdCookie))
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            var projectId = Convert.ToInt32(projectIdCookie);

            var projectBO = projectService.GetById(projectId);

            if (projectBO == null)
            {
                CookieHelper.Delete("ProjectId");
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            var exists = projectBO.HardwareKits.Any(b => b.IsCurrentRevision && b.HardwareKitId != hardwareKitId && b.HardwareKitNumber == hardwareKitNumber);

            return Json(!exists, JsonRequestBehavior.AllowGet);
        }

        // GET: ValidWorkOrderNumber
        public ActionResult ValidProjectNumber(string projectNumber, int? projectId)
        {
            var projectBOs = projectService.GetAll();

            var exists = projectBOs.Any(b => b.ProjectId != projectId && b.ProjectNumber == projectNumber);

            return Json(!exists, JsonRequestBehavior.AllowGet);
        }

    }
}
