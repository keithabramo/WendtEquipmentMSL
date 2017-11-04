
using System;
using System.Linq;
using System.Web.Mvc;
using WendtEquipmentTracking.App.Models;
using WendtEquipmentTracking.BusinessLogic;
using WendtEquipmentTracking.BusinessLogic.Api;
using WendtEquipmentTracking.BusinessLogic.BO;

namespace WendtEquipmentTracking.App.Controllers
{
    public class PriorityController : BaseController
    {
        private IPriorityService priorityService;
        private IProjectService projectService;
        private IUserService userService;

        public PriorityController()
        {
            priorityService = new PriorityService();
            projectService = new ProjectService();
            userService = new UserService();
        }

        //
        // GET: /Priority/

        public ActionResult Index()
        {
            var user = userService.GetCurrentUser();

            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }


            var priorityBOs = priorityService.GetAll(user.ProjectId).ToList();

            var priorityModels = priorityBOs.Select(x => new PriorityModel
            {
                DueDate = x.DueDate,
                EquipmentName = x.EquipmentName,
                PriorityId = x.PriorityId,
                PriorityNumber = x.PriorityNumber,
                ProjectId = x.ProjectId
            });

            priorityModels = priorityModels.OrderBy(r => r.PriorityNumber);

            return View(priorityModels);
        }

        //
        // GET: /Priority/Details/5

        public ActionResult Details(int id)
        {
            var priority = priorityService.GetById(id);

            if (priority == null)
            {
                return HttpNotFound();
            }

            var model = new PriorityModel
            {
                DueDate = priority.DueDate,
                EquipmentName = priority.EquipmentName,
                PriorityId = priority.PriorityId,
                PriorityNumber = priority.PriorityNumber,
                ProjectId = priority.ProjectId
            };

            return View(model);
        }

        //
        // GET: /Priority/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Priority/Create

        [HttpPost]
        public ActionResult Create(PriorityModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = userService.GetCurrentUser();

                    if (user != null)
                    {
                        model.ProjectId = user.ProjectId;

                        var priorityBO = new PriorityBO
                        {
                            DueDate = model.DueDate,
                            EquipmentName = model.EquipmentName,
                            PriorityId = model.PriorityId.HasValue ? model.PriorityId.Value : 0,
                            PriorityNumber = model.PriorityNumber,
                            ProjectId = model.ProjectId
                        };

                        priorityService.Save(priorityBO);

                        return RedirectToAction("Index");
                    }
                }
                HandleError("There was an issue while creating this priority", ModelState);

                return View(model);
            }
            catch (Exception e)
            {
                HandleError("There was an error while creating this priority", e);

                return View(model);
            }
        }

        //
        // GET: /Priority/Edit/5

        public ActionResult Edit(int id)
        {
            var priority = priorityService.GetById(id);
            if (priority == null)
            {
                return HttpNotFound();
            }

            var model = new PriorityModel
            {
                DueDate = priority.DueDate,
                EquipmentName = priority.EquipmentName,
                PriorityId = priority.PriorityId,
                PriorityNumber = priority.PriorityNumber,
                ProjectId = priority.ProjectId
            };

            return View(model);
        }

        //
        // POST: /Priority/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, PriorityModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var user = userService.GetCurrentUser();

                    if (user != null)
                    {
                        model.ProjectId = user.ProjectId;

                        var priorityBO = new PriorityBO
                        {
                            DueDate = model.DueDate,
                            EquipmentName = model.EquipmentName,
                            PriorityId = model.PriorityId.HasValue ? model.PriorityId.Value : 0,
                            PriorityNumber = model.PriorityNumber,
                            ProjectId = model.ProjectId
                        };

                        priorityService.Update(priorityBO);

                        return RedirectToAction("Index");
                    }
                }

                HandleError("There was an issue while saving this priority", ModelState);

                return View(model);
            }
            catch (Exception e)
            {
                HandleError("There was an error while saving this priority", e);
                return View(model);
            }
        }


        // GET: Priority/Delete/5
        public ActionResult Delete(int id)
        {
            var priority = priorityService.GetById(id);

            if (priority == null)
            {
                return HttpNotFound();
            }

            var model = new PriorityModel
            {
                DueDate = priority.DueDate,
                EquipmentName = priority.EquipmentName,
                PriorityId = priority.PriorityId,
                PriorityNumber = priority.PriorityNumber,
                ProjectId = priority.ProjectId
            };

            return View(model);
        }

        // POST: Priority/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, PriorityModel model)
        {
            try
            {
                var priority = priorityService.GetById(id);

                if (priority == null)
                {
                    return HttpNotFound();
                }

                priorityService.Delete(id);

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                HandleError("There was an issue attempting to delete this priority", e);

                return View(model);
            }
        }

    }
}
