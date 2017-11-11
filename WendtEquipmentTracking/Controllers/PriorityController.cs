
using System;
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


            return View();
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




    }
}
