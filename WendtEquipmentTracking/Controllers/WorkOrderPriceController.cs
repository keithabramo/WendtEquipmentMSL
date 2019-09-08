using System;
using System.Web.Mvc;
using WendtEquipmentTracking.App.Models;
using WendtEquipmentTracking.BusinessLogic;
using WendtEquipmentTracking.BusinessLogic.Api;
using WendtEquipmentTracking.BusinessLogic.BO;

namespace WendtEquipmentTracking.App.Controllers
{
    public class WorkOrderPriceController : BaseController
    {
        private IWorkOrderPriceService workOrderPriceService;
        private IProjectService projectService;
        private IUserService userService;

        public WorkOrderPriceController()
        {
            workOrderPriceService = new WorkOrderPriceService();
            projectService = new ProjectService();
            userService = new UserService();
        }

        //
        // GET: /WorkOrderPrice/

        public ActionResult Index(bool? ajaxSuccess)
        {
            if (ajaxSuccess.HasValue && ajaxSuccess.Value)
            {
                SuccessMessage("Work order prices were successfully imported.");
            }

            var user = userService.GetCurrentUser();

            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        //
        // GET: /WorkOrderPrice/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(WorkOrderPriceModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = userService.GetCurrentUser();

                    if (user != null)
                    {
                        model.ProjectId = user.ProjectId;

                        var workOrderPriceBO = new WorkOrderPriceBO
                        {
                            CostPrice = model.CostPrice,
                            ReleasedPercent = model.ReleasedPercent,
                            SalePrice = model.SalePrice,
                            ShippedPercent = model.ShippedPercent,
                            WorkOrderNumber = model.WorkOrderNumber,
                            ProjectId = model.ProjectId
                        };

                        workOrderPriceService.Save(workOrderPriceBO);

                        return RedirectToAction("Index");
                    }
                }
                HandleError("There was an issue while creating this work order price", ModelState);

                return View(model);
            }
            catch (Exception e)
            {
                HandleError("There was an error while creating this work order price", e);

                return View(model);
            }
        }

    }
}
