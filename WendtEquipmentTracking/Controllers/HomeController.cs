
using DevTrends.MvcDonutCaching;
using System.Collections;
using System.Linq;
using System.Web.Mvc;
using WendtEquipmentTracking.App.Common;
using WendtEquipmentTracking.BusinessLogic;
using WendtEquipmentTracking.BusinessLogic.Api;
using WendtEquipmentTracking.Common;

namespace WendtEquipmentTracking.App.Controllers
{
    public class HomeController : Controller
    {
        private IProjectService projectService;
        private IUserService userService;

        public HomeController()
        {
            projectService = new ProjectService();
            userService = new UserService();
        }

        [WendtAuthorize]
        public ActionResult Index()
        {
            var user = userService.GetCurrentUser();


            if (user != null)
            {
                return RedirectToAction("Index", "Equipment");
            }
            else
            {
                clearProjectNavCache();

                var projectBOs = projectService.GetAllForNavigation().OrderByDescending(p => p.ProjectNumber);

                var projects = projectBOs.Select(x => new SelectListItem
                {
                    Value = x.ProjectId.ToString(),
                    Text = x.ProjectNumber + (!string.IsNullOrWhiteSpace(x.ShipToCompany) ? ": " + x.ShipToCompany : "")
                }).ToList();


                return View(projects);
            }
        }

        [WendtAuthorize]
        public ActionResult ClearCache()
        {
            foreach (DictionaryEntry entry in System.Web.HttpContext.Current.Cache)
            {
                System.Web.HttpContext.Current.Cache.Remove((string)entry.Key);
            }

            return RedirectToAction("Index");
        }

        [WendtAuthorize]
        public ActionResult ADDetails()
        {
            var user = ActiveDirectoryHelper.CurrentUserUsername();
            var info = ActiveDirectoryHelper.GetUserInfo(user);

            return Content(info);
        }

        [WendtAuthorize]
        public ActionResult ClearProjectNavCache()
        {
            clearProjectNavCache();

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Error()
        {
            return View();
        }

        public ActionResult NotFound()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Unauthorized()
        {
            return View();
        }

        private void clearProjectNavCache()
        {
            var cacheManager = new OutputCacheManager();

            //remove a single cached action output (Index action)
            cacheManager.RemoveItem("Project", "ProjectNavPartial");
        }

    }
}