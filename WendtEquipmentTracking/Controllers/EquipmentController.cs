using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WendtEquipmentTracking.App.Models;
using WendtEquipmentTracking.BusinessLogic;
using WendtEquipmentTracking.BusinessLogic.Api;

namespace WendtEquipmentTracking.App.Controllers
{
    public class EquipmentController : BaseController
    {
        private IEquipmentService equipmentService;
        private IProjectService projectService;
        private IUserService userService;
        private IPriorityService priorityService;

        public EquipmentController()
        {
            equipmentService = new EquipmentService();
            projectService = new ProjectService();
            userService = new UserService();
            priorityService = new PriorityService();
        }

        //
        // GET: /Equipment/
        public ActionResult Index()
        {
            var user = userService.GetCurrentUser();

            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }


            //Get Data
            var equipmentBOs = equipmentService.GetAll(user.ProjectId);
            var equipmentModels = Mapper.Map<List<EquipmentModel>>(equipmentBOs);

            ViewBag.ProjectNumber = projectService.GetById(user.ProjectId).ProjectNumber;

            return View(equipmentModels);
        }

        //
        // GET: /Equipment/Handsontable
        public ActionResult Handsontable()
        {
            return View();
        }

        //
        // GET: /Equipment/Create
        [ChildActionOnly]
        public ActionResult Create()
        {
            var user = userService.GetCurrentUser();

            IEnumerable<int> priorities = new List<int>();
            if (user != null)
            {
                var prioritiesBOs = priorityService.GetAll(user.ProjectId);

                priorities = prioritiesBOs.Select(p => p.PriorityNumber).OrderBy(p => p).ToList();
            }


            return PartialView(new EquipmentModel
            {
                Priorities = priorities
            });
        }

        //
        // GET: /Equipment/Template
        [ChildActionOnly]
        public ActionResult Template()
        {
            var user = userService.GetCurrentUser();

            IEnumerable<int> priorities = new List<int>();
            if (user != null)
            {
                var prioritiesBOs = priorityService.GetAll(user.ProjectId);

                priorities = prioritiesBOs.Select(p => p.PriorityNumber).OrderBy(p => p).ToList();
            }


            return PartialView(new EquipmentModel
            {
                Priorities = priorities
            });
        }



        // POST: Equipment/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, EquipmentModel model)
        {
            try
            {
                var equipment = equipmentService.GetById(id);

                if (equipment == null)
                {
                    return HttpNotFound();
                }

                equipmentService.Delete(id);

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                var equipment = equipmentService.GetById(id);

                if (equipment == null)
                {
                    return HttpNotFound();
                }

                model = Mapper.Map<EquipmentModel>(equipment);


                LogError(e);

                return View(model);
            }
        }
    }
}
