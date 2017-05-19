using AutoMapper;
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

            var priorityModels = Mapper.Map<IEnumerable<PriorityModel>>(priorityBOs);

            //Filter and sort data

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

            var model = Mapper.Map<PriorityModel>(priority);

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

                        var priorityBO = Mapper.Map<PriorityBO>(model);

                        priorityService.Save(priorityBO);

                        return RedirectToAction("Index");
                    }
                }

                return View(model);
            }
            catch (Exception e)
            {
                LogError(e);
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

            var priorityModel = Mapper.Map<PriorityModel>(priority);

            return View(priorityModel);
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

                        var priority = priorityService.GetById(id);

                        Mapper.Map<PriorityModel, PriorityBO>(model, priority);


                        priorityService.Update(priority);

                        return RedirectToAction("Index");
                    }
                }

                return View(model);
            }
            catch (Exception e)
            {
                LogError(e);
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

            var model = Mapper.Map<PriorityModel>(priority);

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
                LogError(e);
                var priority = priorityService.GetById(id);

                if (priority == null)
                {
                    return HttpNotFound();
                }

                model = Mapper.Map<PriorityModel>(priority);

                return View(model);
            }
        }

    }
}
