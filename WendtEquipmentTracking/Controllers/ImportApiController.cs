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
    public class ImportApiController : BaseApiController
    {
        private IImportService importService;
        private IWorkOrderPriceService workOrderPriceService;
        private IUserService userService;
        private IPriorityService priorityService;
        private IProjectService projectService;
        private IEquipmentService equipmentService;


        public ImportApiController()
        {
            importService = new ImportService();
            workOrderPriceService = new WorkOrderPriceService();
            userService = new UserService();
            priorityService = new PriorityService();
            projectService = new ProjectService();
            equipmentService = new EquipmentService();
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


                var random = new Random();
                equipmentModels = equipmentBOs.Select(x => new EquipmentModel
                {

                    EquipmentId = random.Next(),
                    PriorityNumber = x.PriorityId != null ? (int?)priorities.FirstOrDefault().PriorityNumber : null,
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
                    equipment.ReadyToShip = Convert.ToDouble(equipmentProperties["ReadyToShip"].ToString());


                    var priorityNumber = equipment.PriorityId = Convert.ToInt32(equipmentProperties["PriorityNumber"]);
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
                    equipmentService.SaveAll(equipments);
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
            }

            return new DtResponse { data = equipmentModels };
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
                        PriorityNumber = x.PriorityId != null ? (int?)priorities.FirstOrDefault().PriorityNumber : null,
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
                    
                    var priorityNumber = equipment.PriorityId = Convert.ToInt32(equipmentProperties["PriorityNumber"]);
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
                    equipmentService.SaveAll(equipments);
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
            }

            return new DtResponse { data = equipmentModels };

        }
    }
}
