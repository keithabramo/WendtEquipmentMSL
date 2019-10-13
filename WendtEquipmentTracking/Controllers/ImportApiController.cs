﻿using System;
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
    public class ImportApiController : BaseApiController
    {
        private IImportService importService;
        private IWorkOrderPriceService workOrderPriceService;
        private IUserService userService;
        private IPriorityService priorityService;
        private IProjectService projectService;
        private IEquipmentService equipmentService;
        private IVendorService vendorService;
        private IBrokerService brokerService;
        private IHardwareCommercialCodeService hardwareCommercialCodeService;


        public ImportApiController()
        {
            importService = new ImportService();
            workOrderPriceService = new WorkOrderPriceService();
            userService = new UserService();
            priorityService = new PriorityService();
            projectService = new ProjectService();
            equipmentService = new EquipmentService();
            vendorService = new VendorService();
            brokerService = new BrokerService();
            hardwareCommercialCodeService = new HardwareCommercialCodeService();
        }


        // GET: GetWorkOrderPricesFromImport/
        [HttpGet]
        [HttpPost]
        public IEnumerable<WorkOrderPriceModel> GetWorkOrderPricesFromImport(string filePath)
        {

            IEnumerable<WorkOrderPriceModel> model = new List<WorkOrderPriceModel>();
            try
            {
                var user = userService.GetCurrentUser();

                var allWorkOrderPrices = workOrderPriceService.GetAll(user.ProjectId);

                var importBOs = importService.GetWorkOrderPricesImport(filePath);
                var random = new Random();
                model = importBOs.Select(x => new WorkOrderPriceModel
                {
                    CostPrice = x.CostPrice,
                    ReleasedPercent = x.ReleasedPercent,
                    SalePrice = x.SalePrice,
                    ShippedPercent = x.ShippedPercent,
                    WorkOrderNumber = x.WorkOrderNumber,
                    WorkOrderPriceId = random.Next(),
                    IsDuplicate = allWorkOrderPrices.Any(w =>
                       (w.WorkOrderNumber ?? string.Empty).Equals((x.WorkOrderNumber ?? string.Empty), StringComparison.InvariantCultureIgnoreCase))
                }).ToList();

                return model;

            }
            catch (Exception e)
            {
                HandleError(e);
                return model;
            }
        }

        //
        // GET: api/ImportApi/WorkOrderPriceEditor
        [HttpGet]
        [HttpPost]
        public DtResponse WorkOrderPriceEditor()
        {
            var user = userService.GetCurrentUser();
            var workOrderPriceModels = new List<WorkOrderPriceModel>();

            if (user != null)
            {
                var project = projectService.GetById(user.ProjectId);

                var httpData = DatatableHelpers.HttpData();


                Dictionary<string, object> data = httpData["data"] as Dictionary<string, object>;

                var workOrderPrices = new List<WorkOrderPriceBO>();
                foreach (string workOrderPriceId in data.Keys)
                {
                    var row = data[workOrderPriceId];
                    var workOrderPriceProperties = row as Dictionary<string, object>;

                    WorkOrderPriceBO workOrderPrice = new WorkOrderPriceBO();

                    workOrderPrice.WorkOrderPriceId = !string.IsNullOrWhiteSpace(workOrderPriceProperties["WorkOrderPriceId"].ToString()) ? Convert.ToInt32(workOrderPriceProperties["WorkOrderPriceId"]) : 0;
                    workOrderPrice.CostPrice = !string.IsNullOrWhiteSpace(workOrderPriceProperties["CostPrice"].ToString()) ? Convert.ToDouble(workOrderPriceProperties["CostPrice"]) : 0;
                    workOrderPrice.ProjectId = user.ProjectId;
                    workOrderPrice.ReleasedPercent = !string.IsNullOrWhiteSpace(workOrderPriceProperties["ReleasedPercent"].ToString()) ? Convert.ToDouble(workOrderPriceProperties["ReleasedPercent"]) : 0;
                    workOrderPrice.SalePrice = !string.IsNullOrWhiteSpace(workOrderPriceProperties["SalePrice"].ToString()) ? Convert.ToDouble(workOrderPriceProperties["SalePrice"]) : 0;
                    workOrderPrice.ShippedPercent = !string.IsNullOrWhiteSpace(workOrderPriceProperties["ShippedPercent"].ToString()) ? Convert.ToDouble(workOrderPriceProperties["ShippedPercent"]) : 0;
                    workOrderPrice.WorkOrderNumber = workOrderPriceProperties["WorkOrderNumber"].ToString();


                    workOrderPrices.Add(workOrderPrice);
                }

                var doSubmit = httpData["doSubmit"];
                if (doSubmit.ToString() == "true")
                {
                    workOrderPriceService.SaveAll(workOrderPrices);
                }

                var allWorkOrderPrices = workOrderPriceService.GetAll(user.ProjectId);

                workOrderPriceModels = workOrderPrices.Select(x => new WorkOrderPriceModel
                {
                    CostPrice = x.CostPrice,
                    ProjectId = x.ProjectId,
                    ReleasedPercent = x.ReleasedPercent,
                    SalePrice = x.SalePrice,
                    ShippedPercent = x.ShippedPercent,
                    WorkOrderNumber = x.WorkOrderNumber,
                    WorkOrderPriceId = x.WorkOrderPriceId,
                    IsDuplicate = allWorkOrderPrices.Any(w =>
                       (w.WorkOrderNumber ?? string.Empty).Equals((x.WorkOrderNumber ?? string.Empty), StringComparison.InvariantCultureIgnoreCase))
                }).ToList();
            }

            return new DtResponse { data = workOrderPriceModels };
        }







        // GET: GetWorkOrderPricesFromImport/
        [HttpGet]
        [HttpPost]
        public IEnumerable<EquipmentModel> GetRawEquipmentFromImport(string filePath)
        {

            List<EquipmentModel> equipmentModels = new List<EquipmentModel>();
            try
            {
                var user = userService.GetCurrentUser();

                var equipmentBOs = importService.GetRawEquipmentImport(filePath);
                var priorities = priorityService.GetAll(user.ProjectId);
                var project = projectService.GetById(user.ProjectId);

                var random = new Random();
                equipmentModels = equipmentBOs.Select(x => new EquipmentModel
                {

                    EquipmentId = random.Next(),
                    PriorityNumber = x.PriorityNumber,
                    ProjectId = user.ProjectId,
                    Quantity = x.Quantity.HasValue ? x.Quantity.Value : 0,
                    ReadyToShip = x.ReadyToShip,
                    ReleaseDate = x.ReleaseDate,
                    UnitWeight = x.UnitWeight,
                    CountryOfOrigin = (x.CountryOfOrigin ?? string.Empty).ToUpperInvariant(),
                    Description = (x.Description ?? string.Empty).ToUpperInvariant(),
                    DrawingNumber = (x.DrawingNumber ?? string.Empty).ToUpperInvariant(),
                    EquipmentName = (x.EquipmentName ?? string.Empty).ToUpperInvariant(),
                    HTSCode = (x.HTSCode ?? string.Empty).ToUpperInvariant(),
                    Notes = (x.Notes ?? string.Empty).ToUpperInvariant(),
                    ShippedFrom = (x.ShippedFrom ?? string.Empty).ToUpperInvariant(),
                    ShippingTagNumber = (x.ShippingTagNumber ?? string.Empty).ToUpperInvariant(),
                    WorkOrderNumber = (x.WorkOrderNumber ?? string.Empty).ToUpperInvariant()
                }).ToList();

                equipmentModels.ForEach(x => x.SetIndicators(project.ProjectNumber, project.IsCustomsProject));

                return equipmentModels;

            }
            catch (Exception e)
            {
                HandleError(e);
                return equipmentModels;
            }
        }

        //
        // GET: api/ImportApi/WorkOrderPriceEditor
        [HttpGet]
        [HttpPost]
        public DtResponse RawEquipmentEditor()
        {
            var user = userService.GetCurrentUser();
            var equipmentModels = new List<EquipmentModel>();

            if (user != null)
            {
                var project = projectService.GetById(user.ProjectId);
                var priorities = priorityService.GetAll(user.ProjectId);


                var httpData = DatatableHelpers.HttpData();

                Dictionary<string, object> data = httpData["data"] as Dictionary<string, object>;

                var equipments = new List<EquipmentBO>();
                foreach (string equipmentId in data.Keys)
                {
                    var row = data[equipmentId];
                    var equipmentProperties = row as Dictionary<string, object>;

                    EquipmentBO equipment = new EquipmentBO();

                    equipment.EquipmentId = !string.IsNullOrWhiteSpace(equipmentProperties["EquipmentId"].ToString()) ? Convert.ToInt32(equipmentProperties["EquipmentId"]) : 0;
                    equipment.ProjectId = user.ProjectId;
                    equipment.Quantity = equipmentProperties["Quantity"].ToString().ToNullable<int>();
                    equipment.ReleaseDate = !string.IsNullOrWhiteSpace(equipmentProperties["ReleaseDate"].ToString()) ? (DateTime?)Convert.ToDateTime(equipmentProperties["ReleaseDate"]) : null;
                    equipment.UnitWeight = equipmentProperties["UnitWeightText"].ToString().ToNullable<double>();
                    equipment.Description = equipmentProperties["Description"].ToString().Trim();
                    equipment.DrawingNumber = equipmentProperties["DrawingNumber"].ToString();
                    equipment.EquipmentName = equipmentProperties["EquipmentName"].ToString();
                    equipment.ShippingTagNumber = equipmentProperties["ShippingTagNumber"].ToString();
                    equipment.WorkOrderNumber = equipmentProperties["WorkOrderNumber"].ToString();
                    equipment.CountryOfOrigin = equipmentProperties["CountryOfOrigin"].ToString();
                    equipment.ShippedFrom = equipmentProperties["ShippedFrom"].ToString();
                    equipment.HTSCode = equipmentProperties["HTSCode"].ToString();
                    equipment.Notes = equipmentProperties["Notes"].ToString();
                    equipment.IsHardware = equipment.EquipmentName.Equals("hardware", StringComparison.InvariantCultureIgnoreCase);
                    equipment.ReadyToShip = Convert.ToDouble(equipmentProperties["ReadyToShip"].ToString());
                    equipment.Order = !string.IsNullOrWhiteSpace(equipmentProperties["Order"].ToString()) ? Convert.ToInt32(equipmentProperties["Order"]) : 0;


                    var priorityNumber = equipmentProperties["PriorityNumber"].ToString().ToNullable<int>();
                    var priority = priorities.FirstOrDefault(x => x.PriorityNumber == priorityNumber);
                    if (priority != null)
                    {
                        equipment.PriorityId = priority.PriorityId;
                    }

                    equipments.Add(equipment);
                }

                var doSubmit = httpData["doSubmit"];
                if (doSubmit.ToString() == "true")
                {
                    equipmentService.SaveAll(equipments.OrderByDescending(x => x.Order));
                }

                equipmentModels = equipments.Select(x => new EquipmentModel
                {
                    EquipmentId = x.EquipmentId,
                    PriorityNumber = x.PriorityId != null ? (int?)priorities.FirstOrDefault(e => e.PriorityId == x.PriorityId).PriorityNumber : null,
                    ProjectId = x.ProjectId,
                    Quantity = x.Quantity.HasValue ? x.Quantity.Value : 0,
                    ReleaseDate = x.ReleaseDate,
                    UnitWeight = x.UnitWeight,
                    Description = x.Description,
                    DrawingNumber = x.DrawingNumber,
                    EquipmentName = x.EquipmentName,
                    ShippingTagNumber = x.ShippingTagNumber,
                    WorkOrderNumber = x.WorkOrderNumber,
                    ReadyToShip = x.ReadyToShip,
                    CountryOfOrigin = x.CountryOfOrigin,
                    ShippedFrom = x.ShippedFrom,
                    HTSCode = x.HTSCode,
                    Notes = x.Notes,
                }).ToList();

                equipmentModels.ForEach(x => x.SetIndicators(project.ProjectNumber, project.IsCustomsProject));
            }

            return new DtResponse { data = equipmentModels };
        }






        // GET: GetPrioritiesFromImport/
        [HttpGet]
        [HttpPost]
        public IEnumerable<PriorityModel> GetPrioritiesFromImport(string filePath)
        {

            IEnumerable<PriorityModel> model = new List<PriorityModel>();
            try
            {
                var user = userService.GetCurrentUser();

                var allPriorities = priorityService.GetAll(user.ProjectId);

                var importBOs = importService.GetPrioritiesImport(filePath);
                var random = new Random();
                model = importBOs.Select(x => new PriorityModel
                {
                    ContractualShipDate = x.ContractualShipDate,
                    DueDate = x.DueDate,
                    EndDate = x.EndDate,
                    EquipmentName = x.EquipmentName,
                    PriorityNumber = x.PriorityNumber,
                    PriorityId = random.Next(),
                    IsDuplicate = allPriorities.Any(w => w.PriorityNumber == x.PriorityNumber)
                }).ToList();

                return model;
            }
            catch (Exception e)
            {
                HandleError(e);
                return model;
            }
        }

        //
        // GET: api/ImportApi/PriorityEditor
        [HttpGet]
        [HttpPost]
        public DtResponse PriorityEditor()
        {
            var user = userService.GetCurrentUser();
            var priorityModels = new List<PriorityModel>();

            if (user != null)
            {
                var project = projectService.GetById(user.ProjectId);

                var httpData = DatatableHelpers.HttpData();


                Dictionary<string, object> data = httpData["data"] as Dictionary<string, object>;

                var priorities = new List<PriorityBO>();
                foreach (string priorityId in data.Keys)
                {
                    var row = data[priorityId];
                    var priorityProperties = row as Dictionary<string, object>;

                    PriorityBO priority = new PriorityBO();

                    priority.PriorityId = !string.IsNullOrWhiteSpace(priorityProperties["PriorityId"].ToString()) ? Convert.ToInt32(priorityProperties["PriorityId"]) : 0;
                    priority.DueDate = !string.IsNullOrWhiteSpace(priorityProperties["DueDate"].ToString()) ? Convert.ToDateTime(priorityProperties["DueDate"]) : DateTime.Now;
                    priority.EndDate = !string.IsNullOrWhiteSpace(priorityProperties["EndDate"].ToString()) ? (DateTime?)Convert.ToDateTime(priorityProperties["DueDate"]) : null;
                    priority.ContractualShipDate = !string.IsNullOrWhiteSpace(priorityProperties["ContractualShipDate"].ToString()) ? (DateTime?)Convert.ToDateTime(priorityProperties["ContractualShipDate"]) : null;
                    priority.EquipmentName = priorityProperties["EquipmentName"].ToString();
                    priority.PriorityNumber = !string.IsNullOrWhiteSpace(priorityProperties["PriorityNumber"].ToString()) ? Convert.ToInt32(priorityProperties["PriorityNumber"]) : 0;
                    priority.ProjectId = user.ProjectId;

                    priorities.Add(priority);
                }

                var doSubmit = httpData["doSubmit"];
                if (doSubmit.ToString() == "true")
                {
                    priorityService.SaveAll(priorities);
                }

                var allPriorities = priorityService.GetAll(user.ProjectId);

                priorityModels = priorities.Select(x => new PriorityModel
                {
                    ContractualShipDate = x.ContractualShipDate,
                    ProjectId = x.ProjectId,
                    DueDate = x.DueDate,
                    EndDate = x.EndDate,
                    EquipmentName = x.EquipmentName,
                    PriorityNumber = x.PriorityNumber,
                    PriorityId = x.PriorityId,
                    IsDuplicate = allPriorities.Any(w => w.PriorityNumber == x.PriorityNumber)
                }).ToList();
            }

            return new DtResponse { data = priorityModels };
        }


        [HttpGet]
        [HttpPost]
        public IEnumerable<EquipmentModel> GetEquipmentFromImport(EquipmentImportModel model)
        {
            List<EquipmentModel> equipmentModels = new List<EquipmentModel>();
            try
            {
                var user = userService.GetCurrentUser();
                if (user != null)
                {

                    var importBO = new EquipmentImportBO
                    {
                        //DrawingNumber = model.DrawingNumber,
                        Equipment = model.Equipment,
                        FilePaths = model.FilePaths.ToDictionary(x => x.Split('+')[0], x => x.Split('+')[1]),
                        PriorityId = model.PriorityId,
                        QuantityMultiplier = model.QuantityMultiplier,
                        WorkOrderNumber = model.WorkOrderNumber
                    };

                    var equipmentBOs = importService.GetEquipmentImport(importBO);
                    var priorities = priorityService.GetAll(user.ProjectId);


                    var random = new Random();
                    equipmentModels = equipmentBOs.Select(x => new EquipmentModel
                    {

                        Description = x.Description.Trim(),
                        DrawingNumber = x.DrawingNumber,
                        EquipmentId = random.Next(),
                        EquipmentName = x.EquipmentName,
                        PriorityNumber = x.PriorityId != null ? (int?)priorities.FirstOrDefault(p => p.PriorityId == x.PriorityId).PriorityNumber : null,
                        ProjectId = x.ProjectId,
                        Quantity = x.Quantity.HasValue ? x.Quantity.Value : 0,
                        ReleaseDate = x.ReleaseDate,
                        ShippingTagNumber = x.ShippingTagNumber,
                        UnitWeight = x.UnitWeight,
                        WorkOrderNumber = x.WorkOrderNumber,
                        ShippedFrom = "WENDT" //Default to this and allow them to change
                    }).ToList();
                }
                return equipmentModels;
            }
            catch (Exception e)
            {
                HandleError(e);
                return equipmentModels;
            }
        }

        // Post: EquipmentEditor
        [HttpGet]
        [HttpPost]
        public DtResponse EquipmentEditor()
        {

            var user = userService.GetCurrentUser();
            var equipmentModels = new List<EquipmentModel>();

            if (user != null)
            {
                var project = projectService.GetById(user.ProjectId);
                var priorities = priorityService.GetAll(user.ProjectId);


                var httpData = DatatableHelpers.HttpData();

                Dictionary<string, object> data = httpData["data"] as Dictionary<string, object>;

                var equipments = new List<EquipmentBO>();
                foreach (string equipmentId in data.Keys)
                {
                    var row = data[equipmentId];
                    var equipmentProperties = row as Dictionary<string, object>;

                    EquipmentBO equipment = new EquipmentBO();

                    equipment.EquipmentId = !string.IsNullOrWhiteSpace(equipmentProperties["EquipmentId"].ToString()) ? Convert.ToInt32(equipmentProperties["EquipmentId"]) : 0;
                    equipment.ProjectId = user.ProjectId;
                    equipment.Quantity = equipmentProperties["Quantity"].ToString().ToNullable<int>();
                    equipment.ReleaseDate = !string.IsNullOrWhiteSpace(equipmentProperties["ReleaseDate"].ToString()) ? (DateTime?)Convert.ToDateTime(equipmentProperties["ReleaseDate"]) : null;
                    equipment.UnitWeight = equipmentProperties["UnitWeightText"].ToString().ToNullable<double>();
                    equipment.Description = equipmentProperties["Description"].ToString().Trim();
                    equipment.DrawingNumber = equipmentProperties["DrawingNumber"].ToString();
                    equipment.EquipmentName = equipmentProperties["EquipmentName"].ToString();
                    equipment.ShippingTagNumber = equipmentProperties["ShippingTagNumber"].ToString();
                    equipment.WorkOrderNumber = equipmentProperties["WorkOrderNumber"].ToString();
                    equipment.IsHardware = equipment.EquipmentName.Equals("hardware", StringComparison.InvariantCultureIgnoreCase);
                    equipment.ShippedFrom = equipmentProperties["ShippedFrom"].ToString();
                    equipment.Order = !string.IsNullOrWhiteSpace(equipmentProperties["Order"].ToString()) ? Convert.ToInt32(equipmentProperties["Order"]) : 0;

                    var priorityNumber = equipmentProperties["PriorityNumber"].ToString().ToNullable<int>();
                    var priority = priorities.FirstOrDefault(x => x.PriorityNumber == priorityNumber);
                    if (priority != null)
                    {
                        equipment.PriorityId = priority.PriorityId;
                    }

                    equipments.Add(equipment);
                }

                var doSubmit = httpData["doSubmit"];
                if (doSubmit.ToString() == "true")
                {
                    equipmentService.SaveAll(equipments.OrderByDescending(x => x.Order));
                }

                equipmentModels = equipments.Select(x => new EquipmentModel
                {
                    EquipmentId = x.EquipmentId,
                    PriorityNumber = x.PriorityId != null ? (int?)priorities.FirstOrDefault(e => e.PriorityId == x.PriorityId).PriorityNumber : null,
                    ProjectId = x.ProjectId,
                    Quantity = x.Quantity.HasValue ? x.Quantity.Value : 0,
                    ReleaseDate = x.ReleaseDate,
                    UnitWeight = x.UnitWeight,
                    Description = x.Description,
                    DrawingNumber = x.DrawingNumber,
                    EquipmentName = x.EquipmentName,
                    ShippingTagNumber = x.ShippingTagNumber,
                    WorkOrderNumber = x.WorkOrderNumber,
                    ShippedFrom = x.ShippedFrom
                }).ToList();

                equipmentModels.ForEach(x => x.SetIndicators(project.ProjectNumber, project.IsCustomsProject));
            }



            return new DtResponse { data = equipmentModels };

        }















        [HttpGet]
        [HttpPost]
        public IEnumerable<EquipmentModel> GetEquipmentRevisionFromImport(EquipmentImportModel model)
        {
            List<EquipmentModel> equipmentModels = new List<EquipmentModel>();
            try
            {
                var user = userService.GetCurrentUser();
                if (user != null)
                {

                    var importBO = new EquipmentImportBO
                    {
                        //DrawingNumber = model.DrawingNumber,
                        Equipment = model.Equipment,
                        FilePaths = model.FilePaths.ToDictionary(x => x.Split('+')[0], x => x.Split('+')[1]),
                        PriorityId = model.PriorityId,
                        QuantityMultiplier = model.QuantityMultiplier,
                        WorkOrderNumber = model.WorkOrderNumber
                    };

                    var equipmentBOs = importService.GetEquipmentImport(importBO);
                    var priorities = priorityService.GetAll(user.ProjectId);


                    var random = new Random();
                    equipmentModels = equipmentBOs.Select(x => new EquipmentModel
                    {

                        Description = x.Description.Trim(),
                        DrawingNumber = x.DrawingNumber,
                        EquipmentId = random.Next(),
                        EquipmentName = x.EquipmentName,
                        PriorityNumber = x.PriorityId != null ? (int?)priorities.FirstOrDefault(p => p.PriorityId == x.PriorityId).PriorityNumber : null,
                        ProjectId = x.ProjectId,
                        Quantity = x.Quantity.HasValue ? x.Quantity.Value : 0,
                        ReleaseDate = x.ReleaseDate,
                        ShippingTagNumber = x.ShippingTagNumber,
                        UnitWeight = x.UnitWeight,
                        WorkOrderNumber = x.WorkOrderNumber,
                        ShippedFrom = "WENDT" //Default to this and allow them to change
                    }).ToList();
                }
                return equipmentModels;
            }
            catch (Exception e)
            {
                HandleError(e);
                return equipmentModels;
            }
        }

        // Post: EquipmentRevisionEditor
        [HttpGet]
        [HttpPost]
        public DtResponse EquipmentRevisionEditor()
        {

            var user = userService.GetCurrentUser();
            var equipmentModels = new List<EquipmentModel>();

            if (user != null)
            {
                var project = projectService.GetById(user.ProjectId);
                var priorities = priorityService.GetAll(user.ProjectId);


                var httpData = DatatableHelpers.HttpData();

                Dictionary<string, object> data = httpData["data"] as Dictionary<string, object>;

                var equipments = new List<EquipmentBO>();
                foreach (string equipmentId in data.Keys)
                {
                    var row = data[equipmentId];
                    var equipmentProperties = row as Dictionary<string, object>;

                    EquipmentBO equipment = new EquipmentBO();

                    equipment.EquipmentId = !string.IsNullOrWhiteSpace(equipmentProperties["EquipmentId"].ToString()) ? Convert.ToInt32(equipmentProperties["EquipmentId"]) : 0;
                    equipment.ProjectId = user.ProjectId;
                    equipment.Quantity = equipmentProperties["Quantity"].ToString().ToNullable<int>();
                    equipment.ReleaseDate = !string.IsNullOrWhiteSpace(equipmentProperties["ReleaseDate"].ToString()) ? (DateTime?)Convert.ToDateTime(equipmentProperties["ReleaseDate"]) : null;
                    equipment.UnitWeight = equipmentProperties["UnitWeightText"].ToString().ToNullable<double>();
                    equipment.Description = equipmentProperties["Description"].ToString().Trim();
                    equipment.DrawingNumber = equipmentProperties["DrawingNumber"].ToString();
                    equipment.EquipmentName = equipmentProperties["EquipmentName"].ToString();
                    equipment.ShippingTagNumber = equipmentProperties["ShippingTagNumber"].ToString();
                    equipment.WorkOrderNumber = equipmentProperties["WorkOrderNumber"].ToString();
                    equipment.IsHardware = equipment.EquipmentName.Equals("hardware", StringComparison.InvariantCultureIgnoreCase);
                    equipment.ShippedFrom = equipmentProperties["ShippedFrom"].ToString();
                    equipment.Order = !string.IsNullOrWhiteSpace(equipmentProperties["Order"].ToString()) ? Convert.ToInt32(equipmentProperties["Order"]) : 0;

                    var priorityNumber = equipmentProperties["PriorityNumber"].ToString().ToNullable<int>();
                    var priority = priorities.FirstOrDefault(x => x.PriorityNumber == priorityNumber);
                    if (priority != null)
                    {
                        equipment.PriorityId = priority.PriorityId;
                    }

                    equipments.Add(equipment);
                }

                var doSubmit = httpData["doSubmit"];
                if (doSubmit.ToString() == "true")
                {
                    equipmentService.SaveAll(equipments.OrderByDescending(x => x.Order));
                }

                equipmentModels = equipments.Select(x => new EquipmentModel
                {
                    EquipmentId = x.EquipmentId,
                    PriorityNumber = x.PriorityId != null ? (int?)priorities.FirstOrDefault(e => e.PriorityId == x.PriorityId).PriorityNumber : null,
                    ProjectId = x.ProjectId,
                    Quantity = x.Quantity.HasValue ? x.Quantity.Value : 0,
                    ReleaseDate = x.ReleaseDate,
                    UnitWeight = x.UnitWeight,
                    Description = x.Description,
                    DrawingNumber = x.DrawingNumber,
                    EquipmentName = x.EquipmentName,
                    ShippingTagNumber = x.ShippingTagNumber,
                    WorkOrderNumber = x.WorkOrderNumber,
                    ShippedFrom = x.ShippedFrom
                }).ToList();

                equipmentModels.ForEach(x => x.SetIndicators(project.ProjectNumber, project.IsCustomsProject));
            }



            return new DtResponse { data = equipmentModels };

        }
















        // GET: GetVendorsFromImport/
        [HttpGet]
        [HttpPost]
        public IEnumerable<VendorModel> GetVendorsFromImport(string filePath)
        {

            IEnumerable<VendorModel> model = new List<VendorModel>();
            try
            {
                var allVendors = vendorService.GetAll();

                var importBOs = importService.GetVendorsImport(filePath);
                var random = new Random();
                model = importBOs.Select(x => new VendorModel
                {
                    Address = x.Address,
                    Contact1 = x.Contact1,
                    Email = x.Email,
                    Name = x.Name,
                    PhoneFax = x.PhoneFax,
                    VendorId = random.Next(),
                    IsDuplicate = allVendors.Any(w =>
                       (w.Name ?? string.Empty).Equals((x.Name ?? string.Empty), StringComparison.InvariantCultureIgnoreCase))
                }).ToList();

                return model;

            }
            catch (Exception e)
            {
                HandleError(e);
                return model;
            }
        }

        //
        // GET: api/ImportApi/VendorEditor
        [HttpGet]
        [HttpPost]
        public DtResponse VendorEditor()
        {
            var user = userService.GetCurrentUser();
            var vendorModels = new List<VendorModel>();

            if (user != null)
            {
                var httpData = DatatableHelpers.HttpData();

                Dictionary<string, object> data = httpData["data"] as Dictionary<string, object>;

                var vendors = new List<VendorBO>();
                foreach (string vendorId in data.Keys)
                {
                    var row = data[vendorId];
                    var vendorProperties = row as Dictionary<string, object>;

                    VendorBO vendor = new VendorBO();

                    vendor.VendorId = !string.IsNullOrWhiteSpace(vendorProperties["VendorId"].ToString()) ? Convert.ToInt32(vendorProperties["VendorId"]) : 0;
                    vendor.Address = vendorProperties["Address"].ToString();
                    vendor.Contact1 = vendorProperties["Contact1"].ToString();
                    vendor.Email = vendorProperties["Email"].ToString();
                    vendor.PhoneFax = vendorProperties["PhoneFax"].ToString();
                    vendor.Name = vendorProperties["Name"].ToString();


                    vendors.Add(vendor);
                }

                var doSubmit = httpData["doSubmit"];
                if (doSubmit.ToString() == "true")
                {
                    vendorService.SaveAll(vendors);
                }

                var allVendors = vendorService.GetAll();

                vendorModels = vendors.Select(x => new VendorModel
                {
                    Address = x.Address,
                    Contact1 = x.Contact1,
                    Email = x.Email,
                    Name = x.Name,
                    PhoneFax = x.PhoneFax,
                    VendorId = x.VendorId,
                    IsDuplicate = allVendors.Any(w =>
                       (w.Name ?? string.Empty).Equals((x.Name ?? string.Empty), StringComparison.InvariantCultureIgnoreCase))
                }).ToList();
            }

            return new DtResponse { data = vendorModels };
        }




        // GET: GetBrokersFromImport/
        [HttpGet]
        [HttpPost]
        public IEnumerable<BrokerModel> GetBrokersFromImport(string filePath)
        {

            IEnumerable<BrokerModel> model = new List<BrokerModel>();
            try
            {
                var user = userService.GetCurrentUser();

                var allBrokers = brokerService.GetAll();

                var importBOs = importService.GetBrokersImport(filePath);
                var random = new Random();
                model = importBOs.Select(x => new BrokerModel
                {
                    Address = x.Address,
                    Contact1 = x.Contact1,
                    Email = x.Email,
                    Name = x.Name,
                    PhoneFax = x.PhoneFax,
                    BrokerId = random.Next(),
                    IsDuplicate = allBrokers.Any(w =>
                       (w.Name ?? string.Empty).Equals((x.Name ?? string.Empty), StringComparison.InvariantCultureIgnoreCase))
                }).ToList();

                return model;

            }
            catch (Exception e)
            {
                HandleError(e);
                return model;
            }
        }

        //
        // GET: api/ImportApi/BrokerEditor
        [HttpGet]
        [HttpPost]
        public DtResponse BrokerEditor()
        {
            var user = userService.GetCurrentUser();
            var brokerModels = new List<BrokerModel>();

            if (user != null)
            {
                var httpData = DatatableHelpers.HttpData();


                Dictionary<string, object> data = httpData["data"] as Dictionary<string, object>;

                var brokers = new List<BrokerBO>();
                foreach (string brokerId in data.Keys)
                {
                    var row = data[brokerId];
                    var brokerProperties = row as Dictionary<string, object>;

                    BrokerBO broker = new BrokerBO();

                    broker.BrokerId = !string.IsNullOrWhiteSpace(brokerProperties["BrokerId"].ToString()) ? Convert.ToInt32(brokerProperties["BrokerId"]) : 0;
                    broker.Address = brokerProperties["Address"].ToString();
                    broker.Contact1 = brokerProperties["Contact1"].ToString();
                    broker.Email = brokerProperties["Email"].ToString();
                    broker.PhoneFax = brokerProperties["PhoneFax"].ToString();
                    broker.Name = brokerProperties["Name"].ToString();


                    brokers.Add(broker);
                }

                var doSubmit = httpData["doSubmit"];
                if (doSubmit.ToString() == "true")
                {
                    brokerService.SaveAll(brokers);
                }

                var allBrokers = brokerService.GetAll();

                brokerModels = brokers.Select(x => new BrokerModel
                {
                    Address = x.Address,
                    Contact1 = x.Contact1,
                    Email = x.Email,
                    Name = x.Name,
                    PhoneFax = x.PhoneFax,
                    BrokerId = x.BrokerId,
                    IsDuplicate = allBrokers.Any(w =>
                       (w.Name ?? string.Empty).Equals((x.Name ?? string.Empty), StringComparison.InvariantCultureIgnoreCase))
                }).ToList();
            }

            return new DtResponse { data = brokerModels };
        }

        // GET: GetHardwareCommercialCodesFromImport/
        [HttpGet]
        [HttpPost]
        public IEnumerable<HardwareCommercialCodeModel> GetHardwareCommercialCodesFromImport(string filePath)
        {

            IEnumerable<HardwareCommercialCodeModel> model = new List<HardwareCommercialCodeModel>();
            try
            {
                var user = userService.GetCurrentUser();

                var allHardwareCommercialCodes = hardwareCommercialCodeService.GetAll();

                var importBOs = importService.GetHardwareCommercialCodesImport(filePath);
                var random = new Random();
                model = importBOs.Select(x => new HardwareCommercialCodeModel
                {
                    PartNumber = x.PartNumber,
                    CommodityCode = x.CommodityCode,
                    Description = x.Description,
                    HardwareCommercialCodeId = random.Next(),
                    IsDuplicate = allHardwareCommercialCodes.Any(w =>
                       (w.PartNumber ?? string.Empty).Equals((x.PartNumber ?? string.Empty), StringComparison.InvariantCultureIgnoreCase))
                })
                .Where(x => !x.IsDuplicate) //Requirement for rev 4 is to "ignore duplicates"
                .ToList();

                return model;

            }
            catch (Exception e)
            {
                HandleError(e);
                return model;
            }
        }

        //
        // GET: api/ImportApi/HardwareCommercialCodeEditor
        [HttpGet]
        [HttpPost]
        public DtResponse HardwareCommercialCodeEditor()
        {
            var user = userService.GetCurrentUser();
            var hardwareCommercialCodeModels = new List<HardwareCommercialCodeModel>();

            if (user != null)
            {
                var httpData = DatatableHelpers.HttpData();


                Dictionary<string, object> data = httpData["data"] as Dictionary<string, object>;

                var hardwareCommercialCodes = new List<HardwareCommercialCodeBO>();
                foreach (string hardwareCommercialCodeId in data.Keys)
                {
                    var row = data[hardwareCommercialCodeId];
                    var hardwareCommercialCodeProperties = row as Dictionary<string, object>;

                    HardwareCommercialCodeBO hardwareCommercialCode = new HardwareCommercialCodeBO();

                    hardwareCommercialCode.HardwareCommercialCodeId = !string.IsNullOrWhiteSpace(hardwareCommercialCodeProperties["HardwareCommercialCodeId"].ToString()) ? Convert.ToInt32(hardwareCommercialCodeProperties["HardwareCommercialCodeId"]) : 0;
                    hardwareCommercialCode.PartNumber = hardwareCommercialCodeProperties["PartNumber"].ToString();
                    hardwareCommercialCode.Description = hardwareCommercialCodeProperties["Description"].ToString();
                    hardwareCommercialCode.CommodityCode = hardwareCommercialCodeProperties["CommodityCode"].ToString();


                    hardwareCommercialCodes.Add(hardwareCommercialCode);
                }

                var doSubmit = httpData["doSubmit"];
                if (doSubmit.ToString() == "true")
                {
                    hardwareCommercialCodeService.SaveAll(hardwareCommercialCodes);
                }

                var allHardwareCommercialCodes = hardwareCommercialCodeService.GetAll();

                hardwareCommercialCodeModels = hardwareCommercialCodes.Select(x => new HardwareCommercialCodeModel
                {
                    PartNumber = x.PartNumber,
                    CommodityCode = x.CommodityCode,
                    Description = x.Description,
                    HardwareCommercialCodeId = x.HardwareCommercialCodeId,
                    IsDuplicate = allHardwareCommercialCodes.Any(w =>
                       (w.PartNumber ?? string.Empty).Equals((x.PartNumber ?? string.Empty), StringComparison.InvariantCultureIgnoreCase))
                }).ToList();
            }

            return new DtResponse { data = hardwareCommercialCodeModels };
        }
    }
}
