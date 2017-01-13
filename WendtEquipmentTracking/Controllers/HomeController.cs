using AutoMapper;
using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using WendtEquipmentTracking.App.Common;
using WendtEquipmentTracking.App.Models;
using WendtEquipmentTracking.BusinessLogic;
using WendtEquipmentTracking.BusinessLogic.Api;

namespace WendtEquipmentTracking.App.Controllers
{
    public class HomeController : Controller
    {
        private IProjectService projectService;

        public HomeController()
        {
            projectService = new ProjectService();
        }

        public ActionResult Index()
        {
            if (CookieHelper.Exists("ProjectId"))
            {
                return RedirectToAction("Index", "Equipment");
            }
            else
            {
                var projectBOs = projectService.GetAll();

                var model = Mapper.Map<IEnumerable<ProjectModel>>(projectBOs);

                return View(model);
            }
        }

        public ActionResult ClearCache()
        {
            foreach (DictionaryEntry entry in System.Web.HttpContext.Current.Cache)
            {
                System.Web.HttpContext.Current.Cache.Remove((string)entry.Key);
            }

            return RedirectToAction("Index");
        }
    }
}