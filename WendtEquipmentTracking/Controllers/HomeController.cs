using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
            } else
            {
                var projectBOs = projectService.GetAll();

                var model = Mapper.Map<IEnumerable<ProjectModel>>(projectBOs);

                return View(model);
            }
        }
    }
}