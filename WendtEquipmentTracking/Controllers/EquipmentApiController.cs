using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using WendtEquipmentTracking.App.Models;
using WendtEquipmentTracking.BusinessLogic;
using WendtEquipmentTracking.BusinessLogic.Api;
using WendtEquipmentTracking.BusinessLogic.BO;

namespace WendtEquipmentTracking.App.Controllers
{
    public class EquipmentApiController : BaseApiController
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
            IEnumerable<EquipmentModel> equipmentModels = new List<EquipmentModel>();

            var user = userService.GetCurrentUser();

            if (user == null)
            {
                return equipmentModels;
            }

            var projectBO = projectService.GetById(user.ProjectId);

            //Get Data
            var equipmentBOs = equipmentService.GetAll(user.ProjectId);

            equipmentModels = equipmentBOs.Select(x => new EquipmentModel
            {
                EquipmentId = x.EquipmentId,
                CustomsValue = x.CustomsValue,
                FullyShipped = x.FullyShipped,
                LeftToShip = x.LeftToShip,
                Priority = x.Priority,
                ProjectId = x.ProjectId,
                Quantity = x.Quantity.HasValue ? x.Quantity.Value : 0,
                ReadyToShip = x.ReadyToShip,
                ReleaseDate = x.ReleaseDate,
                SalePrice = x.SalePrice,
                ShippedQuantity = x.ShippedQuantity,
                TotalWeight = x.TotalWeight,
                TotalWeightShipped = x.TotalWeightShipped,
                UnitWeight = x.UnitWeight,
                CountryOfOrigin = (x.CountryOfOrigin ?? string.Empty).ToUpperInvariant(),
                Description = (x.Description ?? string.Empty).ToUpperInvariant(),
                DrawingNumber = (x.DrawingNumber ?? string.Empty).ToUpperInvariant(),
                EquipmentName = (x.EquipmentName ?? string.Empty).ToUpperInvariant(),
                HTSCode = (x.HTSCode ?? string.Empty).ToUpperInvariant(),
                Notes = (x.Notes ?? string.Empty).ToUpperInvariant(),
                ShippedFrom = (x.ShippedFrom ?? string.Empty).ToUpperInvariant(),
                ShippingTagNumber = (x.ShippingTagNumber ?? string.Empty).ToUpperInvariant(),
                WorkOrderNumber = (x.WorkOrderNumber ?? string.Empty).ToUpperInvariant(),

                HasBillOfLading = x.HasBillOfLading,
                HasBillOfLadingInStorage = x.HasBillOfLadingInStorage,
                IsHardwareKit = x.IsHardwareKit,
                IsAssociatedToHardwareKit = x.IsAssociatedToHardwareKit,
                AssociatedHardwareKitNumber = x.AssociatedHardwareKitNumber
            });

            equipmentModels.ToList().ForEach(e =>
            {
                e.SetIndicators(projectBO.ProjectNumber, projectBO.IsCustomsProject);
            });

            equipmentModels
                .GroupBy(x => new
                {
                    DrawingNumber = x.DrawingNumber != null ? x.DrawingNumber.ToUpperInvariant() : string.Empty,
                    WorkOrderNumber = x.WorkOrderNumber != null ? x.WorkOrderNumber.ToUpperInvariant() : string.Empty,
                    Quantity = x.Quantity,
                    ShippingTagNumber = x.ShippingTagNumber != null ? x.ShippingTagNumber.ToUpperInvariant() : string.Empty,
                    Description = x.Description != null ? x.Description.ToUpperInvariant() : string.Empty
                })
                .Where(g => g.Count() > 1)
                .SelectMany(y => y)
                .ToList().ForEach(e => e.IsDuplicate = true);


            return equipmentModels;
        }

        //
        // GET: api/EquipmentApi/Table
        [HttpGet]
        public IEnumerable<EquipmentModel> TableBOL()
        {
            IEnumerable<EquipmentModel> equipmentModels = new List<EquipmentModel>();

            var user = userService.GetCurrentUser();

            if (user == null)
            {
                return equipmentModels;
            }

            var projectBO = projectService.GetById(user.ProjectId);

            //Get Data
            var equipmentBOs = equipmentService.GetAll(user.ProjectId);

            equipmentModels = equipmentBOs.Select(x => new EquipmentModel
            {
                EquipmentId = x.EquipmentId,
                CustomsValue = x.CustomsValue,
                FullyShipped = x.FullyShipped,
                LeftToShip = x.LeftToShip,
                Priority = x.Priority,
                ProjectId = x.ProjectId,
                Quantity = x.Quantity.HasValue ? x.Quantity.Value : 0,
                ReadyToShip = x.ReadyToShip,
                ReleaseDate = x.ReleaseDate,
                SalePrice = x.SalePrice,
                ShippedQuantity = x.ShippedQuantity,
                TotalWeight = x.TotalWeight,
                TotalWeightShipped = x.TotalWeightShipped,
                UnitWeight = x.UnitWeight,
                CountryOfOrigin = (x.CountryOfOrigin ?? string.Empty).ToUpperInvariant(),
                Description = (x.Description ?? string.Empty).ToUpperInvariant(),
                DrawingNumber = (x.DrawingNumber ?? string.Empty).ToUpperInvariant(),
                EquipmentName = (x.EquipmentName ?? string.Empty).ToUpperInvariant(),
                HTSCode = (x.HTSCode ?? string.Empty).ToUpperInvariant(),
                Notes = (x.Notes ?? string.Empty).ToUpperInvariant(),
                ShippedFrom = (x.ShippedFrom ?? string.Empty).ToUpperInvariant(),
                ShippingTagNumber = (x.ShippingTagNumber ?? string.Empty).ToUpperInvariant(),
                WorkOrderNumber = (x.WorkOrderNumber ?? string.Empty).ToUpperInvariant(),

                HasBillOfLading = x.HasBillOfLading,
                HasBillOfLadingInStorage = x.HasBillOfLadingInStorage,
                IsHardwareKit = x.IsHardwareKit,
                IsAssociatedToHardwareKit = x.IsAssociatedToHardwareKit,
                AssociatedHardwareKitNumber = x.AssociatedHardwareKitNumber
            });

            equipmentModels.ToList().ForEach(e =>
            {
                e.SetIndicators(projectBO.ProjectNumber, projectBO.IsCustomsProject);
            });

            equipmentModels
                .GroupBy(x => new
                {
                    DrawingNumber = x.DrawingNumber != null ? x.DrawingNumber.ToUpperInvariant() : string.Empty,
                    WorkOrderNumber = x.WorkOrderNumber != null ? x.WorkOrderNumber.ToUpperInvariant() : string.Empty,
                    Quantity = x.Quantity,
                    ShippingTagNumber = x.ShippingTagNumber != null ? x.ShippingTagNumber.ToUpperInvariant() : string.Empty,
                    Description = x.Description != null ? x.Description.ToUpperInvariant() : string.Empty
                })
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

                        var equipmentBO = new EquipmentBO
                        {
                            EquipmentId = model.EquipmentId,
                            CustomsValue = model.CustomsValue,
                            FullyShipped = model.FullyShipped,
                            LeftToShip = model.LeftToShip,
                            Priority = model.Priority,
                            ProjectId = model.ProjectId,
                            Quantity = model.Quantity,
                            ReadyToShip = model.ReadyToShip,
                            ReleaseDate = model.ReleaseDate,
                            SalePrice = model.SalePrice,
                            ShippedQuantity = model.ShippedQuantity,
                            TotalWeight = model.TotalWeight,
                            TotalWeightShipped = model.TotalWeightShipped,
                            UnitWeight = model.UnitWeight,
                            CountryOfOrigin = (model.CountryOfOrigin ?? string.Empty).ToUpperInvariant(),
                            Description = (model.Description ?? string.Empty).ToUpperInvariant(),
                            DrawingNumber = (model.DrawingNumber ?? string.Empty).ToUpperInvariant(),
                            EquipmentName = (model.EquipmentName ?? string.Empty).ToUpperInvariant(),
                            HTSCode = (model.HTSCode ?? string.Empty).ToUpperInvariant(),
                            Notes = (model.Notes ?? string.Empty).ToUpperInvariant(),
                            ShippedFrom = (model.ShippedFrom ?? string.Empty).ToUpperInvariant(),
                            ShippingTagNumber = (model.ShippingTagNumber ?? string.Empty).ToUpperInvariant(),
                            WorkOrderNumber = (model.WorkOrderNumber ?? string.Empty).ToUpperInvariant(),
                            IsHardware = model.EquipmentName.Equals("hardware", StringComparison.InvariantCultureIgnoreCase)
                        };
                        equipmentBO.UnitWeight = equipmentBO.IsHardware ? .01 : equipmentBO.UnitWeight;

                        var id = equipmentService.Save(equipmentBO);


                        var newEquipmentBO = equipmentService.GetById(id);

                        var newEquipmentModel = new EquipmentModel
                        {
                            EquipmentId = newEquipmentBO.EquipmentId,
                            CustomsValue = newEquipmentBO.CustomsValue,
                            FullyShipped = newEquipmentBO.FullyShipped,
                            LeftToShip = newEquipmentBO.LeftToShip,
                            Priority = newEquipmentBO.Priority,
                            ProjectId = newEquipmentBO.ProjectId,
                            Quantity = newEquipmentBO.Quantity.HasValue ? newEquipmentBO.Quantity.Value : 0,
                            ReadyToShip = newEquipmentBO.ReadyToShip,
                            ReleaseDate = newEquipmentBO.ReleaseDate,
                            SalePrice = newEquipmentBO.SalePrice,
                            ShippedQuantity = newEquipmentBO.ShippedQuantity,
                            TotalWeight = newEquipmentBO.TotalWeight,
                            TotalWeightShipped = newEquipmentBO.TotalWeightShipped,
                            UnitWeight = newEquipmentBO.UnitWeight,
                            CountryOfOrigin = (newEquipmentBO.CountryOfOrigin ?? string.Empty).ToUpperInvariant(),
                            Description = (newEquipmentBO.Description ?? string.Empty).ToUpperInvariant(),
                            DrawingNumber = (newEquipmentBO.DrawingNumber ?? string.Empty).ToUpperInvariant(),
                            EquipmentName = (newEquipmentBO.EquipmentName ?? string.Empty).ToUpperInvariant(),
                            HTSCode = (newEquipmentBO.HTSCode ?? string.Empty).ToUpperInvariant(),
                            Notes = (newEquipmentBO.Notes ?? string.Empty).ToUpperInvariant(),
                            ShippedFrom = (newEquipmentBO.ShippedFrom ?? string.Empty).ToUpperInvariant(),
                            ShippingTagNumber = (newEquipmentBO.ShippingTagNumber ?? string.Empty).ToUpperInvariant(),
                            WorkOrderNumber = (newEquipmentBO.WorkOrderNumber ?? string.Empty).ToUpperInvariant(),

                            HasBillOfLading = newEquipmentBO.HasBillOfLading,
                            HasBillOfLadingInStorage = newEquipmentBO.HasBillOfLadingInStorage,
                            IsHardwareKit = newEquipmentBO.IsHardwareKit,
                            IsAssociatedToHardwareKit = newEquipmentBO.IsAssociatedToHardwareKit,
                            AssociatedHardwareKitNumber = newEquipmentBO.AssociatedHardwareKitNumber
                        };

                        newEquipmentModel.SetIndicators(projectBO.ProjectNumber, projectBO.IsCustomsProject);

                        //check if there are any duplicates
                        var equipmentBOs = equipmentService.GetAll(user.ProjectId);

                        var duplicate = equipmentBOs.Any(x =>
                           x.EquipmentId != newEquipmentModel.EquipmentId &&
                           (x.DrawingNumber ?? string.Empty).Equals((newEquipmentModel.DrawingNumber ?? string.Empty), StringComparison.InvariantCultureIgnoreCase) &&
                           (x.WorkOrderNumber ?? string.Empty).Equals((newEquipmentModel.WorkOrderNumber ?? string.Empty), StringComparison.InvariantCultureIgnoreCase) &&
                           x.Quantity == newEquipmentModel.Quantity &&
                           (x.ShippingTagNumber ?? string.Empty).Equals((newEquipmentModel.ShippingTagNumber ?? string.Empty), StringComparison.InvariantCultureIgnoreCase) &&
                           (x.Description ?? string.Empty).Equals((newEquipmentModel.Description ?? string.Empty), StringComparison.InvariantCultureIgnoreCase)
                        );


                        newEquipmentModel.IsDuplicate = duplicate;

                        return newEquipmentModel;
                    }
                }

                HandleError(ModelState);

                return model;
            }
            catch (Exception e)
            {
                HandleError(e);

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

                        var equipmentBO = new EquipmentBO
                        {
                            EquipmentId = model.EquipmentId,
                            CustomsValue = model.CustomsValue,
                            FullyShipped = model.FullyShipped,
                            LeftToShip = model.LeftToShip,
                            Priority = model.Priority,
                            ProjectId = model.ProjectId,
                            Quantity = model.Quantity,
                            ReadyToShip = model.ReadyToShip,
                            ReleaseDate = model.ReleaseDate,
                            SalePrice = model.SalePrice,
                            ShippedQuantity = model.ShippedQuantity,
                            TotalWeight = model.TotalWeight,
                            TotalWeightShipped = model.TotalWeightShipped,
                            UnitWeight = model.UnitWeight,
                            CountryOfOrigin = (model.CountryOfOrigin ?? string.Empty).ToUpperInvariant(),
                            Description = (model.Description ?? string.Empty).ToUpperInvariant(),
                            DrawingNumber = (model.DrawingNumber ?? string.Empty).ToUpperInvariant(),
                            EquipmentName = (model.EquipmentName ?? string.Empty).ToUpperInvariant(),
                            HTSCode = (model.HTSCode ?? string.Empty).ToUpperInvariant(),
                            Notes = (model.Notes ?? string.Empty).ToUpperInvariant(),
                            ShippedFrom = (model.ShippedFrom ?? string.Empty).ToUpperInvariant(),
                            ShippingTagNumber = (model.ShippingTagNumber ?? string.Empty).ToUpperInvariant(),
                            WorkOrderNumber = (model.WorkOrderNumber ?? string.Empty).ToUpperInvariant(),
                            IsHardware = model.EquipmentName.Equals("hardware", StringComparison.InvariantCultureIgnoreCase)
                        };
                        equipmentBO.UnitWeight = equipmentBO.IsHardware ? .01 : equipmentBO.UnitWeight;

                        equipmentService.Update(equipmentBO);




                        var updatedEquipmentBO = equipmentService.GetById(id);
                        var updatedEquipmentModel = new EquipmentModel
                        {
                            EquipmentId = updatedEquipmentBO.EquipmentId,
                            CustomsValue = updatedEquipmentBO.CustomsValue,
                            FullyShipped = updatedEquipmentBO.FullyShipped,
                            LeftToShip = updatedEquipmentBO.LeftToShip,
                            Priority = updatedEquipmentBO.Priority,
                            ProjectId = updatedEquipmentBO.ProjectId,
                            Quantity = updatedEquipmentBO.Quantity.HasValue ? updatedEquipmentBO.Quantity.Value : 0,
                            ReadyToShip = updatedEquipmentBO.ReadyToShip,
                            ReleaseDate = updatedEquipmentBO.ReleaseDate,
                            SalePrice = updatedEquipmentBO.SalePrice,
                            ShippedQuantity = updatedEquipmentBO.ShippedQuantity,
                            TotalWeight = updatedEquipmentBO.TotalWeight,
                            TotalWeightShipped = updatedEquipmentBO.TotalWeightShipped,
                            UnitWeight = updatedEquipmentBO.UnitWeight,
                            CountryOfOrigin = (updatedEquipmentBO.CountryOfOrigin ?? string.Empty).ToUpperInvariant(),
                            Description = (updatedEquipmentBO.Description ?? string.Empty).ToUpperInvariant(),
                            DrawingNumber = (updatedEquipmentBO.DrawingNumber ?? string.Empty).ToUpperInvariant(),
                            EquipmentName = (updatedEquipmentBO.EquipmentName ?? string.Empty).ToUpperInvariant(),
                            HTSCode = (updatedEquipmentBO.HTSCode ?? string.Empty).ToUpperInvariant(),
                            Notes = (updatedEquipmentBO.Notes ?? string.Empty).ToUpperInvariant(),
                            ShippedFrom = (updatedEquipmentBO.ShippedFrom ?? string.Empty).ToUpperInvariant(),
                            ShippingTagNumber = (updatedEquipmentBO.ShippingTagNumber ?? string.Empty).ToUpperInvariant(),
                            WorkOrderNumber = (updatedEquipmentBO.WorkOrderNumber ?? string.Empty).ToUpperInvariant(),

                            HasBillOfLading = updatedEquipmentBO.HasBillOfLading,
                            HasBillOfLadingInStorage = updatedEquipmentBO.HasBillOfLadingInStorage,
                            IsHardwareKit = updatedEquipmentBO.IsHardwareKit,
                            IsAssociatedToHardwareKit = updatedEquipmentBO.IsAssociatedToHardwareKit,
                            AssociatedHardwareKitNumber = updatedEquipmentBO.AssociatedHardwareKitNumber
                        };

                        updatedEquipmentModel.SetIndicators(projectBO.ProjectNumber, projectBO.IsCustomsProject);

                        //check if there are any duplicates
                        var equipmentBOs = equipmentService.GetAll(user.ProjectId);

                        var duplicate = equipmentBOs.Any(x =>
                           x.EquipmentId != updatedEquipmentModel.EquipmentId &&
                           (x.DrawingNumber ?? string.Empty).Equals((updatedEquipmentModel.DrawingNumber ?? string.Empty), StringComparison.InvariantCultureIgnoreCase) &&
                           (x.WorkOrderNumber ?? string.Empty).Equals((updatedEquipmentModel.WorkOrderNumber ?? string.Empty), StringComparison.InvariantCultureIgnoreCase) &&
                           x.Quantity == updatedEquipmentModel.Quantity &&
                           (x.ShippingTagNumber ?? string.Empty).Equals((updatedEquipmentModel.ShippingTagNumber ?? string.Empty), StringComparison.InvariantCultureIgnoreCase) &&
                           (x.Description ?? string.Empty).Equals((updatedEquipmentModel.Description ?? string.Empty), StringComparison.InvariantCultureIgnoreCase)
                        );


                        updatedEquipmentModel.IsDuplicate = duplicate;

                        return updatedEquipmentModel;
                    }
                }

                HandleError(ModelState);

                return model;
            }
            catch (Exception e)
            {
                HandleError(e);

                return model;
            }
        }
    }
}
