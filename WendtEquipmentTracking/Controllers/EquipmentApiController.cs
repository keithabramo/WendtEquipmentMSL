﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Http;
using WendtEquipmentTracking.App.Common;
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

        private enum EditorActions
        {
            create,
            edit,
            remove
        }

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
            List<EquipmentModel> equipmentModels = new List<EquipmentModel>();

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
                LeftToShip = x.LeftToShip.ToString(),
                Priority = x.Priority,
                ProjectId = x.ProjectId,
                Quantity = x.Quantity.HasValue ? x.Quantity.Value : 0,
                ReadyToShip = x.ReadyToShip,
                ReleaseDate = x.ReleaseDate,
                SalePrice = x.SalePrice,
                ShippedQuantity = x.ShippedQuantity.ToString(),
                TotalWeight = x.TotalWeight.ToString(),
                TotalWeightShipped = x.TotalWeightShipped.ToString(),
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
            }).ToList();

            equipmentModels.ForEach(e =>
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
        // GET: api/EquipmentApi/Editor
        [HttpGet]
        [HttpPost]
        public DtResponse Editor()
        {
            var user = userService.GetCurrentUser();
            var equipmentModels = new List<EquipmentModel>();


            if (user != null)
            {
                var project = projectService.GetById(user.ProjectId);

                var httpData = DatatableHelpers.HttpData();
                Dictionary<string, object> data = httpData["data"] as Dictionary<string, object>;
                var action = httpData["action"];

                var equipments = new List<EquipmentBO>();
                foreach (string equipmentId in data.Keys)
                {
                    var row = data[equipmentId];
                    var equipmentProperties = row as Dictionary<string, object>;

                    var equipment = new EquipmentBO();

                    equipment.EquipmentId = !string.IsNullOrWhiteSpace(equipmentProperties["EquipmentId"].ToString()) ? Convert.ToInt32(equipmentProperties["EquipmentId"]) : 0;
                    equipment.CustomsValue = equipmentProperties["CustomsValueText"].ToString().ToNullable<double>();
                    equipment.FullyShipped = equipmentProperties["FullyShippedText"].ToString() == "YES" ? true : false;
                    equipment.LeftToShip = equipmentProperties["LeftToShip"].ToString().ToNullable<double>();
                    equipment.Priority = Convert.ToInt32(equipmentProperties["Priority"]);
                    equipment.ProjectId = user.ProjectId;
                    equipment.Quantity = equipmentProperties["Quantity"].ToString().ToNullable<int>();
                    equipment.ReadyToShip = equipmentProperties["ReadyToShipText"].ToString().ToNullable<int>();
                    equipment.ReleaseDate = !string.IsNullOrWhiteSpace(equipmentProperties["ReleaseDate"].ToString()) ? (DateTime?)Convert.ToDateTime(equipmentProperties["ReleaseDate"]) : null;
                    equipment.SalePrice = equipmentProperties["SalePriceText"].ToString().ToNullable<double>();
                    equipment.ShippedQuantity = equipmentProperties["ShippedQuantity"].ToString().ToNullable<int>();
                    equipment.TotalWeight = equipmentProperties["TotalWeight"].ToString().ToNullable<double>();
                    equipment.TotalWeightShipped = equipmentProperties["TotalWeightShipped"].ToString().ToNullable<double>();
                    equipment.UnitWeight = equipmentProperties["UnitWeightText"].ToString().ToNullable<double>();
                    equipment.CountryOfOrigin = equipmentProperties["CountryOfOrigin"].ToString();
                    equipment.Description = equipmentProperties["Description"].ToString();
                    equipment.DrawingNumber = equipmentProperties["DrawingNumber"].ToString();
                    equipment.EquipmentName = equipmentProperties["EquipmentName"].ToString();
                    equipment.HTSCode = equipmentProperties["HTSCode"].ToString();
                    equipment.Notes = equipmentProperties["Notes"].ToString();
                    equipment.ShippedFrom = equipmentProperties["ShippedFrom"].ToString();
                    equipment.ShippingTagNumber = equipmentProperties["ShippingTagNumber"].ToString();
                    equipment.WorkOrderNumber = equipmentProperties["WorkOrderNumber"].ToString();

                    equipments.Add(equipment);
                }

                IEnumerable<int> equipmentIds = new List<int>();
                if (action.Equals(EditorActions.edit.ToString()))
                {
                    equipmentService.UpdateAll(equipments);
                    equipmentIds = equipments.Select(x => x.EquipmentId);
                }
                else if (action.Equals(EditorActions.create.ToString()))
                {
                    equipmentIds = equipmentService.SaveAll(equipments);
                }
                else if (action.Equals(EditorActions.remove.ToString()))
                {
                    equipmentService.Delete(equipments.FirstOrDefault().EquipmentId);
                }

                equipments = equipmentService.GetByIds(equipmentIds).ToList();


                var stopwatch = new Stopwatch();
                stopwatch.Start();

                //check all duplicates
                var equipmentBOs = equipmentService.GetForDuplicateCheck(user.ProjectId);

                stopwatch.Stop();
                // Get the elapsed time as a TimeSpan value.
                TimeSpan ts = stopwatch.Elapsed;

                string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);

                equipmentModels = equipments.Select(x => new EquipmentModel
                {
                    EquipmentId = x.EquipmentId,
                    CustomsValue = x.CustomsValue,
                    FullyShipped = x.FullyShipped,
                    LeftToShip = x.LeftToShip.ToString(),
                    Priority = x.Priority,
                    ProjectId = x.ProjectId,
                    Quantity = x.Quantity.HasValue ? x.Quantity.Value : 0,
                    ReadyToShip = x.ReadyToShip,
                    ReleaseDate = x.ReleaseDate,
                    SalePrice = x.SalePrice,
                    ShippedQuantity = x.ShippedQuantity.ToString(),
                    TotalWeight = x.TotalWeight.ToString(),
                    TotalWeightShipped = x.TotalWeightShipped.ToString(),
                    UnitWeight = x.UnitWeight,
                    CountryOfOrigin = x.CountryOfOrigin,
                    Description = x.Description,
                    DrawingNumber = x.DrawingNumber,
                    EquipmentName = x.EquipmentName,
                    HTSCode = x.HTSCode,
                    Notes = x.Notes,
                    ShippedFrom = x.ShippedFrom,
                    ShippingTagNumber = x.ShippingTagNumber,
                    WorkOrderNumber = x.WorkOrderNumber,

                    HasBillOfLading = x.HasBillOfLading,
                    HasBillOfLadingInStorage = x.HasBillOfLadingInStorage,
                    IsHardwareKit = x.IsHardwareKit,
                    IsAssociatedToHardwareKit = x.IsAssociatedToHardwareKit,
                    AssociatedHardwareKitNumber = x.AssociatedHardwareKitNumber,
                    IsDuplicate = equipmentBOs.Any(e =>
                       e.EquipmentId != x.EquipmentId &&
                       (e.DrawingNumber ?? string.Empty).Equals((x.DrawingNumber ?? string.Empty), StringComparison.InvariantCultureIgnoreCase) &&
                       (e.WorkOrderNumber ?? string.Empty).Equals((x.WorkOrderNumber ?? string.Empty), StringComparison.InvariantCultureIgnoreCase) &&
                       e.Quantity == x.Quantity &&
                       (e.ShippingTagNumber ?? string.Empty).Equals((x.ShippingTagNumber ?? string.Empty), StringComparison.InvariantCultureIgnoreCase) &&
                       (e.Description ?? string.Empty).Equals((x.Description ?? string.Empty), StringComparison.InvariantCultureIgnoreCase))

                }).ToList();

                equipmentModels.ForEach(x => x.SetIndicators(project.ProjectNumber, project.IsCustomsProject));

            }

            return new DtResponse { data = equipmentModels };
        }

    }
}
