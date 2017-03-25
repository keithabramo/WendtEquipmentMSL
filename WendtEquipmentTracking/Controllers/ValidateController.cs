using System.IO;
using System.Linq;
using System.Web.Mvc;
using WendtEquipmentTracking.BusinessLogic;
using WendtEquipmentTracking.BusinessLogic.Api;

namespace WendtEquipmentTracking.App.Controllers
{
    public class ValidateController : BaseController
    {
        private IProjectService projectService;
        private IBillOfLadingService billOfLadingService;
        private IHardwareKitService hardwareKitService;
        private IWorkOrderPriceService workOrderPriceService;
        private IUserService userService;
        private IPriorityService priorityService;


        public ValidateController()
        {
            projectService = new ProjectService();
            billOfLadingService = new BillOfLadingService();
            hardwareKitService = new HardwareKitService();
            workOrderPriceService = new WorkOrderPriceService();
            userService = new UserService();
            priorityService = new PriorityService();
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
            var user = userService.GetCurrentUser();

            if (user == null)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }


            var exists = billOfLadingService.GetAll().Any(b => b.ProjectId == user.ProjectId
                                                                          && b.IsCurrentRevision
                                                                          && b.BillOfLadingId != billOfLadingId
                                                                          && b.BillOfLadingNumber == billOfLadingNumber);

            return Json(!exists, JsonRequestBehavior.AllowGet);
        }

        // GET: HTSCodeRequired
        public ActionResult HTSCodeRequired(string htsCode)
        {
            var valid = true;
            if (string.IsNullOrEmpty(htsCode))
            {
                var user = userService.GetCurrentUser();

                if (user == null)
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }

                var projectBO = projectService.GetById(user.ProjectId);

                valid = !projectBO.IsCustomsProject;
            }

            return Json(valid, JsonRequestBehavior.AllowGet);
        }

        // GET: CountryOfOriginRequired
        public ActionResult CountryOfOriginRequired(string countryOfOrigin)
        {
            var valid = true;
            if (string.IsNullOrEmpty(countryOfOrigin))
            {
                var user = userService.GetCurrentUser();

                if (user == null)
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }


                var projectBO = projectService.GetById(user.ProjectId);

                valid = !projectBO.IsCustomsProject;
            }

            return Json(valid, JsonRequestBehavior.AllowGet);
        }

        // GET: ValidWorkOrderNumber
        public ActionResult ValidWorkOrderNumber(string workOrderNumber, int? workOrderPriceId)
        {

            var user = userService.GetCurrentUser();

            if (user == null)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            var exists = workOrderPriceService.GetAll().Any(b => b.ProjectId == user.ProjectId
                                                                 && b.WorkOrderPriceId != workOrderPriceId
                                                                 && b.WorkOrderNumber == workOrderNumber);

            return Json(!exists, JsonRequestBehavior.AllowGet);
        }

        // GET: ValidWorkOrderNumber
        public ActionResult ValidWorkOrderNumberImport()
        {

            if (Request.QueryString.AllKeys.FirstOrDefault(p => p.ToLower().Contains("workordernumber")) != null)
            {
                var isChecked = false;
                if (Request.QueryString.AllKeys.FirstOrDefault(p => p.ToLower().Contains("checked")) != null)
                {
                    isChecked = Request.QueryString[Request.QueryString.AllKeys.First(p => p.ToLower().Contains("checked"))] == "true";
                }

                if (isChecked)
                {

                    string workOrderNumber = Request.QueryString[Request.QueryString.AllKeys.First(p => p.ToLower().Contains("workordernumber"))];

                    var user = userService.GetCurrentUser();

                    if (user == null)
                    {
                        return Json(true, JsonRequestBehavior.AllowGet);
                    }

                    var exists = workOrderPriceService.GetAll().Any(b => b.ProjectId == user.ProjectId
                                                                         && b.WorkOrderNumber == workOrderNumber);

                    return Json(!exists, JsonRequestBehavior.AllowGet);
                }
            }


            return Json(true, JsonRequestBehavior.AllowGet);

        }

        // GET: ValidHardwareKitNumber
        public ActionResult ValidHardwareKitNumber(string hardwareKitNumber, int? hardwareKitId)
        {
            var user = userService.GetCurrentUser();

            if (user == null)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }



            var exists = hardwareKitService.GetAll().Any(b => b.ProjectId == user.ProjectId
                                                              && b.IsCurrentRevision
                                                              && b.HardwareKitId != hardwareKitId
                                                              && b.HardwareKitNumber == hardwareKitNumber);

            return Json(!exists, JsonRequestBehavior.AllowGet);
        }

        // GET: ValidProjectNumber
        public ActionResult ValidProjectNumber(string projectNumber, int? projectId)
        {
            var projectBOs = projectService.GetAll();

            var exists = projectBOs.Any(b => b.ProjectId != projectId && b.ProjectNumber == projectNumber);

            return Json(!exists, JsonRequestBehavior.AllowGet);
        }

        // GET: ValidPriorityNumber
        public ActionResult ValidPriorityNumber(int priorityNumber, int? priorityId)
        {
            var user = userService.GetCurrentUser();

            if (user == null)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            var exists = priorityService.GetAll(user.ProjectId).Any(b => b.PriorityId != priorityId
                                                                 && b.PriorityNumber == priorityNumber);

            return Json(!exists, JsonRequestBehavior.AllowGet);
        }

    }
}
