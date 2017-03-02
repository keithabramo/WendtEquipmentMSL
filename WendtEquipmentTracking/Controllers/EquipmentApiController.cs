using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using WendtEquipmentTracking.App.Common;
using WendtEquipmentTracking.App.Models;
using WendtEquipmentTracking.BusinessLogic;
using WendtEquipmentTracking.BusinessLogic.Api;
using WendtEquipmentTracking.BusinessLogic.BO;

namespace WendtEquipmentTracking.App.Controllers
{
    public class EquipmentApiController : ApiController
    {
        private IEquipmentService equipmentService;
        private IProjectService projectService;
        private IUserService userService;

        public EquipmentApiController()
        {
            equipmentService = new EquipmentService();
            projectService = new ProjectService();
            userService = new UserService();
        }

        //
        // GET: api/EquipmentApi/Table
        [HttpGet]
        public IEnumerable<EquipmentModel> Table()
        {
            var equipmentModels = new List<EquipmentModel>();

            var user = userService.GetCurrentUser();

            if (user == null)
            {
                return equipmentModels;
            }

            var projectBO = projectService.GetById(user.ProjectId);

            //Get Data
            var equipmentBOs = equipmentService.GetAll(user.ProjectId);

            equipmentModels = Mapper.Map<List<EquipmentModel>>(equipmentBOs);
            equipmentModels.ToList().ForEach(e =>
            {
                e.SetIndicators(projectBO.ProjectNumber, projectBO.IsCustomsProject);
                e.HasBillOfLading = e.BillOfLadingEquipments.Count() > 0;
                e.IsHardwareKit = e.HardwareKit != null;
                e.IsAssociatedToHardwareKit = e.HardwareKitEquipments.Where(h => h.HardwareKit.IsCurrentRevision).FirstOrDefault() != null;
                e.AssociatedHardwareKitNumber = e.IsAssociatedToHardwareKit ? e.HardwareKitEquipments.Where(h => h.HardwareKit.IsCurrentRevision).FirstOrDefault().HardwareKit.HardwareKitNumber : string.Empty;
                e.BillOfLadingEquipments = null;
                e.HardwareKitEquipments = null;
            });

            equipmentModels.GroupBy(x => new { x.DrawingNumber, x.WorkOrderNumber, x.Quantity, x.ShippingTagNumber, x.Description })
              .Where(g => g.Count() > 1)
              .SelectMany(y => y)
              .ToList().ForEach(e => e.IsDuplicate = true);


            return equipmentModels;
        }

        //
        // POST: api/EquipmentApi/Create
        [HttpPost]
        public EquipmentModel Create(EquipmentModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = userService.GetCurrentUser();

                    if (user != null)
                    {
                        var projectBO = projectService.GetById(user.ProjectId);

                        model.ProjectId = user.ProjectId;

                        var equipmentBO = Mapper.Map<EquipmentBO>(model);
                        equipmentBO.IsHardware = model.EquipmentName.Equals("hardware", StringComparison.InvariantCultureIgnoreCase);

                        var id = equipmentService.Save(equipmentBO);


                        var newEquipmentBO = equipmentService.GetById(id);

                        var newEquipmentModel = Mapper.Map<EquipmentModel>(newEquipmentBO);
                        newEquipmentModel.SetIndicators(projectBO.ProjectNumber, projectBO.IsCustomsProject);
                        newEquipmentModel.HasBillOfLading = newEquipmentModel.BillOfLadingEquipments.Count() > 0;
                        newEquipmentModel.IsHardwareKit = newEquipmentModel.HardwareKit != null;
                        newEquipmentModel.IsAssociatedToHardwareKit = newEquipmentModel.HardwareKitEquipments.Where(h => h.HardwareKit.IsCurrentRevision).FirstOrDefault() != null;
                        newEquipmentModel.AssociatedHardwareKitNumber = newEquipmentModel.IsAssociatedToHardwareKit ? newEquipmentModel.HardwareKitEquipments.Where(h => h.HardwareKit.IsCurrentRevision).FirstOrDefault().HardwareKit.HardwareKitNumber : string.Empty;
                        newEquipmentModel.BillOfLadingEquipments = null;
                        newEquipmentModel.HardwareKitEquipments = null;

                        newEquipmentModel.Status = SuccessStatus.Success;
                        return newEquipmentModel;
                    }
                }

                model.Status = SuccessStatus.Error;
                model.Errors = ModelState.Where(v => v.Value.Errors.Count() > 0).ToList().Select(v => new BaseModelError { Name = v.Key, Message = v.Value.Errors.First().ErrorMessage });
                return model;
            }
            catch (Exception e)
            {
                model.Status = SuccessStatus.Error;
                model.Errors = new List<BaseModelError> { new BaseModelError { Name = "Generic", Message = "There was an issue saving thie record" } };
                return model;
            }
        }

        //
        // POST: api/EquipmentApi/Edit
        [HttpPost]
        public EquipmentModel Edit(int id, EquipmentModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = userService.GetCurrentUser();

                    if (user != null)
                    {
                        var projectBO = projectService.GetById(user.ProjectId);

                        model.ProjectId = user.ProjectId;
                        var equipment = equipmentService.GetById(id);

                        Mapper.Map<EquipmentModel, EquipmentBO>(model, equipment);

                        equipment.IsHardware = model.EquipmentName.Equals("hardware", StringComparison.InvariantCultureIgnoreCase);


                        equipmentService.Update(equipment);

                        var updatedEquipmentBO = equipmentService.GetById(id);
                        var updatedEquipmentModel = Mapper.Map<EquipmentModel>(updatedEquipmentBO);

                        updatedEquipmentModel.SetIndicators(projectBO.ProjectNumber, projectBO.IsCustomsProject);
                        updatedEquipmentModel.HasBillOfLading = updatedEquipmentModel.BillOfLadingEquipments.Count() > 0;
                        updatedEquipmentModel.IsHardwareKit = updatedEquipmentModel.HardwareKit != null;
                        updatedEquipmentModel.IsAssociatedToHardwareKit = updatedEquipmentModel.HardwareKitEquipments.Where(h => h.HardwareKit.IsCurrentRevision).FirstOrDefault() != null;
                        updatedEquipmentModel.AssociatedHardwareKitNumber = updatedEquipmentModel.IsAssociatedToHardwareKit ? updatedEquipmentModel.HardwareKitEquipments.Where(h => h.HardwareKit.IsCurrentRevision).FirstOrDefault().HardwareKit.HardwareKitNumber : string.Empty;
                        updatedEquipmentModel.BillOfLadingEquipments = null;
                        updatedEquipmentModel.HardwareKitEquipments = null;

                        updatedEquipmentModel.Status = SuccessStatus.Success;
                        return updatedEquipmentModel;
                    }
                }

                model.Status = SuccessStatus.Error;
                model.Errors = ModelState.Where(v => v.Value.Errors.Count() > 0).ToList().Select(v => new BaseModelError { Name = v.Key, Message = v.Value.Errors.First().ErrorMessage });
                return model;
            }
            catch (Exception e)
            {
                model.Status = SuccessStatus.Error;
                model.Errors = new List<BaseModelError> { new BaseModelError { Name = "Generic", Message = "There was an issue saving thie record" } };
                return model;
            }
        }
    }
}
