
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

        public ActionResult Index()
        {
            var user = userService.GetCurrentUser();

            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        //
        // GET: /Project/Details/5

        public ActionResult Details(int id)
        {
            var user = userService.GetCurrentUser();

            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var project = projectService.GetById(id);

            if (project == null)
            {
                return HttpNotFound();
            }

            var model = new ProjectModel
            {
                FreightTerms = project.FreightTerms,
                IncludeSoftCosts = project.IncludeSoftCosts,
                IsCustomsProject = project.IsCustomsProject,
                IsCompleted = project.IsCompleted,
                ProjectId = project.ProjectId,
                ProjectNumber = project.ProjectNumber,
                ShipToAddress = project.ShipToAddress,
                ShipToBroker = project.ShipToBroker,
                ShipToBrokerEmail = project.ShipToBrokerEmail,
                ShipToBrokerPhoneFax = project.ShipToBrokerPhoneFax,
                ShipToCompany = project.ShipToCompany,
                ShipToContact1 = project.ShipToContact1,
                ShipToContact1Email = project.ShipToContact1Email,
                ShipToContact1PhoneFax = project.ShipToContact1PhoneFax,
                ShipToContact2 = project.ShipToContact2,
                ShipToContact2Email = project.ShipToContact2Email,
                ShipToContact2PhoneFax = project.ShipToContact2PhoneFax,
                ShipToCSZ = project.ShipToCSZ
            };


            return View(model);
        }

        //
        // GET: /Project/Create

        public ActionResult Create()
        {
            var user = userService.GetCurrentUser();

            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

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
                    var projectBO = new ProjectBO
                    {
                        FreightTerms = model.FreightTerms,
                        IncludeSoftCosts = model.IncludeSoftCosts,
                        IsCustomsProject = model.IsCustomsProject,
                        IsCompleted = model.IsCompleted,
                        ProjectId = model.ProjectId,
                        ProjectNumber = model.ProjectNumber,
                        ShipToAddress = model.ShipToAddress,
                        ShipToBroker = model.ShipToBroker,
                        ShipToBrokerEmail = model.ShipToBrokerEmail,
                        ShipToBrokerPhoneFax = model.ShipToBrokerPhoneFax,
                        ShipToCompany = model.ShipToCompany,
                        ShipToContact1 = model.ShipToContact1,
                        ShipToContact1Email = model.ShipToContact1Email,
                        ShipToContact1PhoneFax = model.ShipToContact1PhoneFax,
                        ShipToContact2 = model.ShipToContact2,
                        ShipToContact2Email = model.ShipToContact2Email,
                        ShipToContact2PhoneFax = model.ShipToContact2PhoneFax,
                        ShipToCSZ = model.ShipToCSZ
                    };



                    projectService.Save(projectBO);

                    return RedirectToAction("Index");
                }

                HandleError("There was an issue attempting to create this project", ModelState);

                return View(model);
            }
            catch (Exception e)
            {
                HandleError("There was an error attempting to create this project", e);
                return View(model);
            }
        }

        //
        // GET: /Project/Edit/5

        public ActionResult Edit(int id)
        {
            var user = userService.GetCurrentUser();

            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var project = projectService.GetById(id);
            if (project == null)
            {
                return HttpNotFound();
            }

            var model = new ProjectModel
            {
                FreightTerms = project.FreightTerms,
                IncludeSoftCosts = project.IncludeSoftCosts,
                IsCustomsProject = project.IsCustomsProject,
                IsCompleted = project.IsCompleted,
                ProjectId = project.ProjectId,
                ProjectNumber = project.ProjectNumber,
                ShipToAddress = project.ShipToAddress,
                ShipToBroker = project.ShipToBroker,
                ShipToBrokerEmail = project.ShipToBrokerEmail,
                ShipToBrokerPhoneFax = project.ShipToBrokerPhoneFax,
                ShipToCompany = project.ShipToCompany,
                ShipToContact1 = project.ShipToContact1,
                ShipToContact1Email = project.ShipToContact1Email,
                ShipToContact1PhoneFax = project.ShipToContact1PhoneFax,
                ShipToContact2 = project.ShipToContact2,
                ShipToContact2Email = project.ShipToContact2Email,
                ShipToContact2PhoneFax = project.ShipToContact2PhoneFax,
                ShipToCSZ = project.ShipToCSZ
            };

            return View(model);
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
                    var projectBO = new ProjectBO
                    {
                        FreightTerms = model.FreightTerms,
                        IncludeSoftCosts = model.IncludeSoftCosts,
                        IsCustomsProject = model.IsCustomsProject,
                        IsCompleted = model.IsCompleted,
                        ProjectId = model.ProjectId,
                        ProjectNumber = model.ProjectNumber,
                        ShipToAddress = model.ShipToAddress,
                        ShipToBroker = model.ShipToBroker,
                        ShipToBrokerEmail = model.ShipToBrokerEmail,
                        ShipToBrokerPhoneFax = model.ShipToBrokerPhoneFax,
                        ShipToCompany = model.ShipToCompany,
                        ShipToContact1 = model.ShipToContact1,
                        ShipToContact1Email = model.ShipToContact1Email,
                        ShipToContact1PhoneFax = model.ShipToContact1PhoneFax,
                        ShipToContact2 = model.ShipToContact2,
                        ShipToContact2Email = model.ShipToContact2Email,
                        ShipToContact2PhoneFax = model.ShipToContact2PhoneFax,
                        ShipToCSZ = model.ShipToCSZ
                    };

                    projectService.Update(projectBO);

                    return RedirectToAction("Index");
                }

                HandleError("There was an issue attempting to save this project", ModelState);
                return View(model);
            }
            catch (Exception e)
            {
                HandleError("There was an error attempting to save this project", e);
                return View(model);
            }
        }

        public ActionResult ProjectNavPartial()
        {
            try
            {
                var projects = new List<SelectListItem>();
                ProjectModel currentProject = null;
                var user = userService.GetCurrentUser();

                if (user != null)
                {
                    try
                    {
                        var projectBOs = projectService.GetAllForNavigation().OrderByDescending(p => p.ProjectNumber);
                        projects = projectBOs.Select(x => new SelectListItem
                        {
                            Value = x.ProjectId.ToString(),
                            Text = x.ProjectNumber + (!string.IsNullOrWhiteSpace(x.ShipToCompany) ? ": " + x.ShipToCompany : ""),
                            Selected = user.ProjectId == x.ProjectId
                        }).ToList();

                        var currentProjectBO = projectBOs.FirstOrDefault(x => x.ProjectId == user.ProjectId);
                        if (currentProjectBO != null)
                        {
                            currentProject = new ProjectModel
                            {
                                ProjectId = currentProjectBO.ProjectId,
                                ProjectNumber = currentProjectBO.ProjectNumber
                            };
                        }

                    }
                    catch (Exception e)
                    {
                        HandleError("There was an error attempting to load this list of projects due to projects numbers containing text", e);
                        var projectBOs = projectService.GetAllForNavigation().OrderByDescending(p => p.ProjectNumber);
                        projects = projectBOs.Select(x => new SelectListItem
                        {
                            Value = x.ProjectId.ToString(),
                            Text = x.ProjectNumber + (!string.IsNullOrWhiteSpace(x.ShipToCompany) ? ": " + x.ShipToCompany : ""),
                            Selected = user.ProjectId == x.ProjectId
                        }).ToList();

                    }

                    var model = new ProjectNavModel
                    {
                        Projects = projects,
                        CurrentProject = currentProject
                    };

                    return PartialView(model);
                }

            }
            catch (Exception e)
            {
                HandleError("There was an error attempting to load this list of projects", e);
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

            return RedirectToAction("Index", "Home");
        }

    }
}
