using AutoMapper;
using DevTrends.MvcDonutCaching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WendtEquipmentTracking.App.Models;
using WendtEquipmentTracking.BusinessLogic;
using WendtEquipmentTracking.BusinessLogic.Api;
using WendtEquipmentTracking.BusinessLogic.BO;

namespace WendtEquipmentTracking.App.Controllers
{
    public class ProjectController : BaseController
    {
        private IProjectService projectService;
        private IUserService userService;

        public ProjectController()
        {
            projectService = new ProjectService();
            userService = new UserService();
        }

        //
        // GET: /Project/

        public ViewResult Index()
        {
            //Get Data
            var projectBOs = projectService.GetAll().ToList();

            var projectModels = Mapper.Map<IEnumerable<ProjectModel>>(projectBOs);

            //Filter and sort data
            projectModels = projectModels.OrderBy(r => r.ProjectNumber);

            return View(projectModels.ToList());
        }

        //
        // GET: /Project/Details/5

        public ActionResult Details(int id)
        {
            var project = projectService.GetById(id);

            if (project == null)
            {
                return HttpNotFound();
            }

            var model = Mapper.Map<ProjectModel>(project);

            return View(model);
        }

        //
        // GET: /Project/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Project/Create

        [HttpPost]
        public ActionResult Create(ProjectModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var projectBO = Mapper.Map<ProjectBO>(model);

                    projectService.Save(projectBO);

                    //clear the cache for the project list at the top right of the page
                    clearProjectNavCache();

                    return RedirectToAction("Index");
                }

                return View(model);
            }
            catch
            {
                return View(model);
            }
        }

        //
        // GET: /Project/Edit/5

        public ActionResult Edit(int id)
        {
            var project = projectService.GetById(id);
            if (project == null)
            {
                return HttpNotFound();
            }

            var projectModel = Mapper.Map<ProjectModel>(project);

            return View(projectModel);
        }

        //
        // POST: /Project/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, ProjectModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var project = projectService.GetById(id);

                    Mapper.Map<ProjectModel, ProjectBO>(model, project);

                    projectService.Update(project);

                    //clear the cache for the project list at the top right of the page
                    clearProjectNavCache();

                    return RedirectToAction("Index");
                }

                return View(model);
            }
            catch
            {
                return View(model);
            }
        }

        // POST: Project/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, ProjectModel model)
        {
            try
            {
                var project = projectService.GetById(id);

                if (project == null)
                {
                    return HttpNotFound();
                }

                projectService.Delete(id);

                var user = userService.GetCurrentUser();

                if (user != null)
                {

                    if (id == user.ProjectId)
                    {
                        userService.Delete();
                    }
                }

                //clear the cache for the project list at the top right of the page
                clearProjectNavCache();

                return RedirectToAction("Index");
            }
            catch
            {
                var project = projectService.GetById(id);

                if (project == null)
                {
                    return HttpNotFound();
                }

                model = Mapper.Map<ProjectModel>(project);

                return View(model);
            }
        }

        //[DonutOutputCache(Duration = 3600)]
        public ActionResult ProjectNavPartial()
        {
            var user = userService.GetCurrentUser();

            if (user != null)
            {

                var projectBOs = projectService.GetAllForNavigation().ToList();
                var projectModels = Mapper.Map<IEnumerable<ProjectModel>>(projectBOs);
                var currentProjectModel = projectModels.SingleOrDefault(p => p.ProjectId == user.ProjectId);

                var model = new ProjectNavModel();

                try
                {
                    model = new ProjectNavModel
                    {
                        CurrentProject = currentProjectModel,
                        Projects = projectModels.OrderBy(p => Convert.ToInt32(p.ProjectNumber))
                    };
                } catch(Exception e)
                {
                    model = new ProjectNavModel
                    {
                        CurrentProject = currentProjectModel,
                        Projects = projectModels.OrderBy(p => p.ProjectNumber)
                    };
                }

                return PartialView(model);
            }

            return PartialView();
        }

        [HttpPost]
        public ActionResult ChangeProject(int ProjectId)
        {
            var user = userService.GetCurrentUser();

            if (user != null)
            {
                userService.Update(ProjectId);
            }
            else
            {
                userService.Save(ProjectId);
            }

            var projectBO = projectService.GetAllForNavigation().SingleOrDefault(p => p.ProjectId == ProjectId);

            //clear the cache for the project list at the top right of the page
            clearProjectNavCache();

            return RedirectToAction("Index", "Home");
        }

        private void clearProjectNavCache()
        {
            //var cacheManager = new OutputCacheManager();

            ////remove a single cached action output (Index action)
            //cacheManager.RemoveItem("Project", "ProjectNavPartial");
        }
    }
}
