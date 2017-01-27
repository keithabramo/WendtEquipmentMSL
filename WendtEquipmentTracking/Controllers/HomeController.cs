using AutoMapper;
using DevTrends.MvcDonutCaching;
using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using WendtEquipmentTracking.App.Common;
using WendtEquipmentTracking.App.Models;
using WendtEquipmentTracking.BusinessLogic;
using WendtEquipmentTracking.BusinessLogic.Api;
using WendtEquipmentTracking.Common;

namespace WendtEquipmentTracking.App.Controllers
{
    public class HomeController : Controller
    {
        private IProjectService projectService;

        public HomeController()
        {
            projectService = new ProjectService();
        }

        [WendtAuthorize]
        public ActionResult Index()
        {
            if (CookieHelper.Exists("ProjectId"))
            {
                return RedirectToAction("Index", "Equipment");
            }
            else
            {
                clearProjectNavCache();

                var projectBOs = projectService.GetAllForNavigation();

                var model = Mapper.Map<IEnumerable<ProjectModel>>(projectBOs);

                return View(model);
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