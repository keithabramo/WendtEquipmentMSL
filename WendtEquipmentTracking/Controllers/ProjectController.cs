using AutoMapper;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using WendtEquipmentTracking.App.Common;
using WendtEquipmentTracking.App.Models;
using WendtEquipmentTracking.BusinessLogic;
using WendtEquipmentTracking.BusinessLogic.Api;
using WendtEquipmentTracking.BusinessLogic.BO;

namespace WendtEquipmentTracking.App.Controllers
{
    public class ProjectController : BaseController
    {
        private const int PAGE_SIZE = 30;
        private IProjectService projectService;

        public ProjectController()
        {
            projectService = new ProjectService();
        }

        //
        // GET: /Project/

        public ViewResult Index(int? page)
        {
            //Get Data
            var projectBOs = projectService.GetAll().ToList();

            var projectModels = Mapper.Map<IEnumerable<ProjectModel>>(projectBOs);

            //Filter and sort data


            projectModels = projectModels.OrderBy(r => r.ProjectNumber);

            int pageNumber = (page ?? 1);

            return View(projectModels.ToPagedList(pageNumber, PAGE_SIZE));
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
                return View();
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


        // GET: Project/Delete/5
        public ActionResult Delete(int id)
        {
            var project = projectService.GetById(id);

            if (project == null)
            {
                return HttpNotFound();
            }

            var model = Mapper.Map<ProjectModel>(project);

            return View(model);
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

        [ChildActionOnly]
        [OutputCache(Duration = 3600)]
        public ActionResult ProjectNavPartial()
        {
            var currentProjectId = CookieHelper.Get("ProjectId");
            if (!string.IsNullOrEmpty(currentProjectId))
            {
                var currentProjectBO = projectService.GetById(Convert.ToInt32(currentProjectId));
                var projectBOs = projectService.GetAll().ToList();

                var model = new ProjectNavModel
                {
                    CurrentProject = Mapper.Map<ProjectModel>(currentProjectBO),
                    Projects = Mapper.Map<IEnumerable<ProjectModel>>(projectBOs)
                };

                return PartialView(model);
            }

            return PartialView();
        }

        [HttpPost]
        public ActionResult ChangeProject(int ProjectId)
        {
            CookieHelper.Set("ProjectId", ProjectId.ToString());

            return RedirectToAction("Index", "Equipment");
        }

        private void clearProjectNavCache()
        {
            var urlToRemove = Url.Action("ProjectNavPartial");
            HttpResponse.RemoveOutputCacheItem(urlToRemove);
        }
    }
}
