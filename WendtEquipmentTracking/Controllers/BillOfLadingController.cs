using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WendtEquipmentTracking.App.Common;
using WendtEquipmentTracking.App.Models;
using WendtEquipmentTracking.BusinessLogic;
using WendtEquipmentTracking.BusinessLogic.Api;
using WendtEquipmentTracking.BusinessLogic.BO;

namespace WendtEquipmentTracking.App.Controllers
{
    public class BillOfLadingController : BaseController
    {
        private IBillOfLadingService billOfLadingService;
        private IProjectService projectService;
        private IEquipmentService equipmentService;
        private IUserService userService;

        public BillOfLadingController()
        {
            billOfLadingService = new BillOfLadingService();
            projectService = new ProjectService();
            equipmentService = new EquipmentService();
            userService = new UserService();
        }

        //
        // GET: /BillOfLading/

        public ActionResult Index()
        {
            var user = userService.GetCurrentUser();

            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }


            var billOfLadingBOs = billOfLadingService.GetAll().Where(b => b.ProjectId == user.ProjectId && b.IsCurrentRevision).ToList();

            var billOfLadingModels = Mapper.Map<IEnumerable<BillOfLadingModel>>(billOfLadingBOs);

            return View(billOfLadingModels);
        }

        //
        // GET: /BillOfLading/Details/5

        public ActionResult Details(string billOfLadingNumber)
        {
            var user = userService.GetCurrentUser();

            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var billOfLadings = billOfLadingService.GetByBillOfLadingNumber(user.ProjectId, billOfLadingNumber);

            if (billOfLadings == null)
            {
                return HttpNotFound();
            }

            var model = Mapper.Map<IEnumerable<BillOfLadingModel>>(billOfLadings);

            model = model.OrderByDescending(b => b.Revision).ToList();

            return View(model);
        }

        //
        // GET: /BillOfLading/Create

        public ActionResult Create()
        {
            var user = userService.GetCurrentUser();

            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }


            //Get Data
            var projectBO = projectService.GetById(user.ProjectId);

            if (projectBO == null)
            {
                userService.Delete();
                return RedirectToAction("Index", "Home");
            }

            var model = new BillOfLadingModel
            {
                FreightTerms = projectBO.FreightTerms
            };

            return View(model);
        }

        //
        // POST: /BillOfLading/Create

        [HttpPost]
        public ActionResult Create(BillOfLadingModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = userService.GetCurrentUser();

                    if (user != null)
                    {

                        model.ProjectId = user.ProjectId;
                        model.BillOfLadingEquipments = model.BillOfLadingEquipments.Where(be => be.Quantity > 0 && be.Checked).ToList();

                        if (model.BillOfLadingEquipments.Count() != 0)
                        {

                            var billOfLadingBO = Mapper.Map<BillOfLadingBO>(model);

                            billOfLadingService.Save(billOfLadingBO);

                            return RedirectToAction("Index");
                        }
                        ViewData.SetStatusMessage("You must select at least one equipment item to associate with this BOL", StatusCodes.Error);


                    }


                }

                HandleError("There was an error saving your Bill Of Lading", ModelState);

                return View(model);
            }
            catch (Exception e)
            {
                HandleError("There was an error saving your Bill Of Lading", e);

                return View(model);
            }
        }

        //
        // GET: /BillOfLading/Edit/5

        public ActionResult Edit(int id)
        {
            var billOfLading = billOfLadingService.GetById(id);
            if (billOfLading == null)
            {
                return HttpNotFound();
            }

            var billOfLadingModel = Mapper.Map<BillOfLadingModel>(billOfLading);
            billOfLadingModel.BillOfLadingEquipments.ToList().ForEach(e => e.Checked = true);
            return View(billOfLadingModel);
        }

        //
        // POST: /BillOfLading/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, BillOfLadingModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = userService.GetCurrentUser();

                    if (user != null)
                    {
                        model.ProjectId = user.ProjectId;
                        model.BillOfLadingEquipments = model.BillOfLadingEquipments.Where(be => be.Quantity > 0 && be.Checked).ToList();

                        if (model.BillOfLadingEquipments.Count() != 0)
                        {
                            var billOfLading = billOfLadingService.GetById(id);

                            Mapper.Map<BillOfLadingModel, BillOfLadingBO>(model, billOfLading);

                            billOfLadingService.Update(billOfLading);

                            return RedirectToAction("Index");
                        }

                        ViewData.SetStatusMessage("You must select at least one equipment item to associate with this BOL", StatusCodes.Error);
                    }
                }

                HandleError("There was an error while saving this BOL", ModelState);

                return View(model);
            }
            catch (Exception e)
            {
                HandleError("An error occured while saving this Bill Of Lading", e);

                return View(model);
            }
        }

        // POST: BillOfLading/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, BillOfLadingModel model)
        {
            try
            {
                var billOfLading = billOfLadingService.GetById(id);

                if (billOfLading == null)
                {
                    return HttpNotFound();
                }

                billOfLadingService.Delete(id);

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                HandleError("There was an error while trying to delete this Bill of Lading", e);
                return View(model);
            }
        }


        //
        // GET: /BillOfLading/EquipmentToAddToBOL
        [ChildActionOnly]
        public ActionResult EquipmentToAddToBOL()
        {
            var user = userService.GetCurrentUser();

            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var projectBO = projectService.GetById(user.ProjectId);

            //Get Data
            var equipmentBOs = equipmentService.GetAll(user.ProjectId).Where(e => e.ReadyToShip != null
                                                                    && e.ReadyToShip > 0
                                                                    && !(e.FullyShipped ?? false)
                                                                    && !e.IsHardware).ToList();

            var equipmentModels = Mapper.Map<IEnumerable<EquipmentModel>>(equipmentBOs);

            equipmentModels.ToList().ForEach(e =>
            {
                e.SetIndicators(projectBO.ProjectNumber, projectBO.IsCustomsProject);
            });

            //Filter and sort data

            equipmentModels = equipmentModels.OrderBy(r => r.EquipmentName);

            var billOfLadingEquipments = equipmentModels.Select(e => new BillOfLadingEquipmentModel
            {
                Equipment = e,
                EquipmentId = e.EquipmentId,
                HTSCode = e.HTSCode,
                CountryOfOrigin = e.CountryOfOrigin,
                ShippedFrom = e.ShippedFrom
            }).ToList();

            var model = new BillOfLadingModel
            {
                BillOfLadingEquipments = billOfLadingEquipments
            };

            return PartialView(model);
        }

        //
        // GET: /BillOfLading/EquipmentToEditForBOL
        [ChildActionOnly]
        public ActionResult EquipmentToEditForBOL(BillOfLadingModel model)
        {
            var user = userService.GetCurrentUser();

            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var projectBO = projectService.GetById(user.ProjectId);

            //Get Data
            var equipmentBOs = equipmentService.GetAll(user.ProjectId).Where(e => e.ReadyToShip != null
                                                                    && e.ReadyToShip > 0
                                                                    && !(e.FullyShipped ?? false)
                                                                    && !e.IsHardware).ToList();

            var equipmentModels = Mapper.Map<IEnumerable<EquipmentModel>>(equipmentBOs);

            //from database
            var billOfLadingEquipments = equipmentModels.Select(e => new BillOfLadingEquipmentModel
            {
                Equipment = e,
                EquipmentId = e.EquipmentId,
                Quantity = model.BillOfLadingEquipments.Any(be => be.EquipmentId == e.EquipmentId) ? model.BillOfLadingEquipments.FirstOrDefault(be => be.EquipmentId == e.EquipmentId).Quantity : 0,
                ShippedFrom = model.BillOfLadingEquipments.Any(be => be.EquipmentId == e.EquipmentId) ? model.BillOfLadingEquipments.FirstOrDefault(be => be.EquipmentId == e.EquipmentId).ShippedFrom : e.ShippedFrom,

                Checked = model.BillOfLadingEquipments.Any(be => be.EquipmentId == e.EquipmentId)
            });

            //get any added to model but not found in query because ready to ship is now 0
            var modelBOLEquipments = model.BillOfLadingEquipments.Where(be => equipmentModels == null || !equipmentModels.Any(fullbe => fullbe.EquipmentId == be.EquipmentId)).ToList();

            modelBOLEquipments.AddRange(billOfLadingEquipments);
            modelBOLEquipments.OrderByDescending(e => e.Checked ? 1 : 0).ThenBy(e => e.Equipment.EquipmentName).ToList().ForEach(e =>
            {
                e.Equipment.SetIndicators(projectBO.ProjectNumber, projectBO.IsCustomsProject);
                e.HTSCode = e.Equipment.HTSCode;
                e.CountryOfOrigin = e.Equipment.CountryOfOrigin;
            });

            model.BillOfLadingEquipments = modelBOLEquipments;

            return PartialView("EquipmentToAddToBOL", model);
        }
    }
}
