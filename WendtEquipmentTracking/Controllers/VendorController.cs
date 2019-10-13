using System.Web.Mvc;
using WendtEquipmentTracking.BusinessLogic;
using WendtEquipmentTracking.BusinessLogic.Api;

namespace WendtEquipmentTracking.App.Controllers
{
    public class VendorController : BaseController
    {
        private IVendorService vendorService;
        private IProjectService projectService;
        private IUserService userService;

        public VendorController()
        {
            vendorService = new VendorService();
            projectService = new ProjectService();
            userService = new UserService();
        }

        //
        // GET: /Vendor/

        public ActionResult Index(bool? ajaxSuccess)
        {
            if (ajaxSuccess.HasValue && ajaxSuccess.Value)
            {
                SuccessMessage("Vendors were successfully imported.");
            }

            var user = userService.GetCurrentUser();

            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        //
        // GET: /Vendor/Create
        [ChildActionOnly]
        public ActionResult Create()
        {
            return PartialView();
        }
    }
}
