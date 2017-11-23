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
    public class BillOfLadingApiController : BaseApiController
    {
        private IEquipmentService equipmentService;
        private IBillOfLadingService billOfLadingService;
        private IProjectService projectService;
        private IUserService userService;
        private IPriorityService priorityService;


        public BillOfLadingApiController()
        {
            equipmentService = new EquipmentService();
            projectService = new ProjectService();
            userService = new UserService();
            billOfLadingService = new BillOfLadingService();
            priorityService = new PriorityService();
        }


        [HttpGet]
        public IEnumerable<BillOfLadingModel> Table()
        {
            IEnumerable<BillOfLadingModel> billOfLadingModels = new List<BillOfLadingModel>();

            var user = userService.GetCurrentUser();

            if (user == null)
            {
                return billOfLadingModels;
            }

            var billOfLadingBOs = billOfLadingService.GetCurrentByProject(user.ProjectId);
            billOfLadingModels = billOfLadingBOs.Select(x => new BillOfLadingModel
            {
                BillOfLadingId = x.BillOfLadingId,
                BillOfLadingNumber = x.BillOfLadingNumber,
                Carrier = x.Carrier,
                DateShipped = x.DateShipped,
                FreightTerms = x.FreightTerms,
                IsCurrentRevision = x.IsCurrentRevision,
                ProjectId = x.ProjectId,
                Revision = x.Revision,
                ToStorage = x.ToStorage,
                TrailerNumber = x.TrailerNumber
            });

            return billOfLadingModels;
        }

        [HttpGet]
        public IEnumerable<BillOfLadingModel> DetailsTable(string billOfLadingNumber)
        {
            IEnumerable<BillOfLadingModel> billOfLadingModels = new List<BillOfLadingModel>();

            var user = userService.GetCurrentUser();

            if (user == null)
            {
                return billOfLadingModels;
            }

            var billOfLadings = billOfLadingService.GetByBillOfLadingNumber(user.ProjectId, billOfLadingNumber);

            if (billOfLadings == null)
            {
                return billOfLadingModels;
            }


            billOfLadingModels = billOfLadings.Select(x => new BillOfLadingModel
            {
                BillOfLadingId = x.BillOfLadingId,
                BillOfLadingNumber = x.BillOfLadingNumber,
                Carrier = x.Carrier,
                DateShipped = x.DateShipped,
                FreightTerms = x.FreightTerms,
                IsCurrentRevision = x.IsCurrentRevision,
                ProjectId = x.ProjectId,
                Revision = x.Revision,
                ToStorage = x.ToStorage,
                TrailerNumber = x.TrailerNumber

            });

            billOfLadingModels = billOfLadingModels.OrderByDescending(b => b.Revision);

            return billOfLadingModels;
        }

        //
        // GET: api/EquipmentApi/CreateTable
        [HttpGet]
        public IEnumerable<BillOfLadingEquipmentModel> CreateTable()
        {
            List<BillOfLadingEquipmentModel> billOfLadingEquipmentModels = new List<BillOfLadingEquipmentModel>();

            var user = userService.GetCurrentUser();

            if (user == null)
            {
                return billOfLadingEquipmentModels;
            }

            var projectBO = projectService.GetById(user.ProjectId);

            //Get Data
            var equipmentBOs = equipmentService.GetAll(user.ProjectId).Where(e => e.ReadyToShip != null
                                                                                && e.ReadyToShip > 0
                                                                                && !e.FullyShipped
                                                                                && !e.IsHardware).ToList();

            billOfLadingEquipmentModels = equipmentBOs.Select(x => new BillOfLadingEquipmentModel
            {
                EquipmentId = x.EquipmentId,
                Equipment = new EquipmentModel
                {
                    EquipmentId = x.EquipmentId,
                    CustomsValue = x.CustomsValue,
                    FullyShipped = x.FullyShipped,
                    LeftToShip = x.LeftToShip.ToString(),
                    PriorityNumber = x.Priority != null ? (int?)x.Priority.PriorityNumber : null,
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
                }
            }).ToList();

            billOfLadingEquipmentModels.ForEach(x =>
            {
                x.Equipment.SetIndicators(projectBO.ProjectNumber, projectBO.IsCustomsProject);
            });

            billOfLadingEquipmentModels
                .GroupBy(x => new
                {
                    DrawingNumber = x.Equipment.DrawingNumber != null ? x.Equipment.DrawingNumber.ToUpperInvariant() : string.Empty,
                    WorkOrderNumber = x.Equipment.WorkOrderNumber != null ? x.Equipment.WorkOrderNumber.ToUpperInvariant() : string.Empty,
                    ShippingTagNumber = x.Equipment.ShippingTagNumber != null ? x.Equipment.ShippingTagNumber.ToUpperInvariant() : string.Empty,
                })
                .Where(g => g.Count() > 1)
                .SelectMany(y => y)
                .ToList().ForEach(e => e.Equipment.IsDuplicate = true);


            return billOfLadingEquipmentModels;
        }



        //
        // GET: api/EquipmentApi/EditTable
        [HttpGet]
        public IEnumerable<BillOfLadingEquipmentModel> EditTable(int billOfLadingId)
        {
            List<BillOfLadingEquipmentModel> billOfLadingEquipmentModels = new List<BillOfLadingEquipmentModel>();

            var user = userService.GetCurrentUser();

            if (user == null)
            {
                return billOfLadingEquipmentModels;
            }

            var projectBO = projectService.GetById(user.ProjectId);


            //Get existing bill of lading information
            var billOfLading = billOfLadingService.GetById(billOfLadingId);

            billOfLadingEquipmentModels = billOfLading.BillOfLadingEquipments.Select(x => new BillOfLadingEquipmentModel
            {
                EquipmentId = x.EquipmentId,
                BillOfLadingEquipmentId = x.BillOfLadingEquipmentId,
                CountryOfOrigin = x.CountryOfOrigin,
                HTSCode = x.HTSCode,
                ShippedFrom = x.ShippedFrom,
                Quantity = x.Quantity,
                Equipment = new EquipmentModel
                {
                    EquipmentId = x.Equipment.EquipmentId,
                    CustomsValue = x.Equipment.CustomsValue,
                    FullyShipped = x.Equipment.FullyShipped,
                    LeftToShip = x.Equipment.LeftToShip.ToString(),
                    PriorityNumber = x.Equipment.Priority != null ? (int?)x.Equipment.Priority.PriorityNumber : null,
                    ProjectId = x.Equipment.ProjectId,
                    Quantity = x.Equipment.Quantity.HasValue ? x.Equipment.Quantity.Value : 0,
                    ReadyToShip = x.Equipment.ReadyToShip,
                    ReleaseDate = x.Equipment.ReleaseDate,
                    SalePrice = x.Equipment.SalePrice,
                    ShippedQuantity = x.Equipment.ShippedQuantity.ToString(),
                    TotalWeight = x.Equipment.TotalWeight.ToString(),
                    TotalWeightShipped = x.Equipment.TotalWeightShipped.ToString(),
                    UnitWeight = x.Equipment.UnitWeight,
                    CountryOfOrigin = (x.Equipment.CountryOfOrigin ?? string.Empty).ToUpperInvariant(),
                    Description = (x.Equipment.Description ?? string.Empty).ToUpperInvariant(),
                    DrawingNumber = (x.Equipment.DrawingNumber ?? string.Empty).ToUpperInvariant(),
                    EquipmentName = (x.Equipment.EquipmentName ?? string.Empty).ToUpperInvariant(),
                    HTSCode = (x.Equipment.HTSCode ?? string.Empty).ToUpperInvariant(),
                    Notes = (x.Equipment.Notes ?? string.Empty).ToUpperInvariant(),
                    ShippedFrom = (x.Equipment.ShippedFrom ?? string.Empty).ToUpperInvariant(),
                    ShippingTagNumber = (x.Equipment.ShippingTagNumber ?? string.Empty).ToUpperInvariant(),
                    WorkOrderNumber = (x.Equipment.WorkOrderNumber ?? string.Empty).ToUpperInvariant(),

                    HasBillOfLading = x.Equipment.HasBillOfLading,
                    HasBillOfLadingInStorage = x.Equipment.HasBillOfLadingInStorage,
                    IsHardwareKit = x.Equipment.IsHardwareKit,
                    IsAssociatedToHardwareKit = x.Equipment.IsAssociatedToHardwareKit,
                    AssociatedHardwareKitNumber = x.Equipment.AssociatedHardwareKitNumber
                }
            }).ToList();




            //Get all not added equipments
            var nonAddedEquipmentBOs = equipmentService.GetAll(user.ProjectId).Where(e => e.ReadyToShip != null
                                                                                && e.ReadyToShip > 0
                                                                                && !e.FullyShipped
                                                                                && !e.IsHardware
                                                                                && !billOfLading.BillOfLadingEquipments.Select(x => x.EquipmentId).Contains(e.EquipmentId)
                                                                                ).ToList();

            billOfLadingEquipmentModels.AddRange(nonAddedEquipmentBOs.Select(x => new BillOfLadingEquipmentModel
            {
                EquipmentId = x.EquipmentId,
                CountryOfOrigin = x.CountryOfOrigin,
                HTSCode = x.HTSCode,
                ShippedFrom = x.ShippedFrom,
                Equipment = new EquipmentModel
                {
                    EquipmentId = x.EquipmentId,
                    CustomsValue = x.CustomsValue,
                    FullyShipped = x.FullyShipped,
                    LeftToShip = x.LeftToShip.ToString(),
                    PriorityNumber = x.Priority != null ? (int?)x.Priority.PriorityNumber : null,
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
                }
            }));

            billOfLadingEquipmentModels.ForEach(x =>
            {
                x.Equipment.SetIndicators(projectBO.ProjectNumber, projectBO.IsCustomsProject);
            });

            billOfLadingEquipmentModels
                .GroupBy(x => new
                {
                    DrawingNumber = x.Equipment.DrawingNumber != null ? x.Equipment.DrawingNumber.ToUpperInvariant() : string.Empty,
                    WorkOrderNumber = x.Equipment.WorkOrderNumber != null ? x.Equipment.WorkOrderNumber.ToUpperInvariant() : string.Empty,
                    ShippingTagNumber = x.Equipment.ShippingTagNumber != null ? x.Equipment.ShippingTagNumber.ToUpperInvariant() : string.Empty,
                })
                .Where(g => g.Count() > 1)
                .SelectMany(y => y)
                .ToList().ForEach(e => e.Equipment.IsDuplicate = true);


            return billOfLadingEquipmentModels;
        }

        [HttpGet]
        public void Lock(int id)
        {
            billOfLadingService.Lock(id);
        }

        [HttpGet]
        public void Unlock(int id)
        {
            billOfLadingService.Unlock(id);
        }

        [HttpGet]
        public bool IsLocked(int id)
        {
            var billOfLading = billOfLadingService.GetById(id);

            return billOfLading.IsLocked;
        }

        //
        // GET: api/EquipmentApi/Editor
        [HttpGet]
        [HttpPost]
        public DtResponse Editor()
        {
            var user = userService.GetCurrentUser();
            var billOfLadingEquipmentModels = new List<BillOfLadingEquipmentModel>();


            if (user != null)
            {
                var project = projectService.GetById(user.ProjectId);
                var priorities = priorityService.GetAll(user.ProjectId);

                var httpData = DatatableHelpers.HttpData();
                Dictionary<string, object> data = httpData["data"] as Dictionary<string, object>;
                var action = httpData["action"];

                var billOfLadingEquipments = new List<BillOfLadingEquipmentBO>();
                foreach (string equipmentId in data.Keys)
                {
                    var row = data[equipmentId];
                    var billOfLadingEquipmentProperties = row as Dictionary<string, object>;
                    var equipmentProperties = billOfLadingEquipmentProperties["Equipment"] as Dictionary<string, object>;

                    var equipment = new EquipmentBO();

                    equipment.EquipmentId = !string.IsNullOrWhiteSpace(equipmentProperties["EquipmentId"].ToString()) ? Convert.ToInt32(equipmentProperties["EquipmentId"]) : 0;
                    equipment.CustomsValue = equipmentProperties["CustomsValueText"].ToString().ToNullable<double>();
                    equipment.FullyShipped = equipmentProperties["FullyShippedText"].ToString() == "YES" ? true : false;
                    equipment.LeftToShip = equipmentProperties["LeftToShip"].ToString().ToNullable<double>();
                    equipment.ProjectId = user.ProjectId;
                    equipment.Quantity = equipmentProperties["Quantity"].ToString().ToNullable<int>();
                    equipment.ReadyToShip = Convert.ToDouble(equipmentProperties["ReadyToShip"].ToString());
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
                    equipment.IsHardware = equipment.EquipmentName.Equals("hardware", StringComparison.InvariantCultureIgnoreCase);

                    var priorityNumber = equipment.PriorityId = Convert.ToInt32(equipmentProperties["PriorityNumber"]);
                    var priority = priorities.FirstOrDefault(x => x.PriorityNumber == priorityNumber);
                    if (priority != null)
                    {
                        equipment.PriorityId = priority.PriorityId;
                        equipment.Priority = new PriorityBO
                        {
                            PriorityId = priority.PriorityId,
                            PriorityNumber = priority.PriorityNumber
                        };
                    }

                    var billOfLadingEquipmentBO = new BillOfLadingEquipmentBO();
                    billOfLadingEquipmentBO.Equipment = equipment;
                    billOfLadingEquipmentBO.EquipmentId = equipment.EquipmentId;
                    billOfLadingEquipmentBO.CountryOfOrigin = equipment.CountryOfOrigin;
                    billOfLadingEquipmentBO.HTSCode = equipment.HTSCode;
                    billOfLadingEquipmentBO.ShippedFrom = equipment.ShippedFrom;
                    billOfLadingEquipmentBO.Quantity = Convert.ToInt32(billOfLadingEquipmentProperties["Quantity"].ToString());
                    billOfLadingEquipmentBO.BillOfLadingEquipmentId = !string.IsNullOrEmpty(billOfLadingEquipmentProperties["BillOfLadingEquipmentId"].ToString()) ? Convert.ToInt32(billOfLadingEquipmentProperties["BillOfLadingEquipmentId"].ToString()) : 0;




                    billOfLadingEquipments.Add(billOfLadingEquipmentBO);
                }

                var doSubmit = httpData["doSubmit"];
                if (doSubmit.ToString() == "true")
                {
                    var billOfLading = new BillOfLadingBO();
                    billOfLading.BillOfLadingEquipments = billOfLadingEquipments;
                    billOfLading.BillOfLadingNumber = httpData["BillOfLadingNumber"].ToString();
                    billOfLading.Carrier = httpData["Carrier"].ToString();
                    billOfLading.DateShipped = !string.IsNullOrEmpty(httpData["DateShipped"].ToString()) ? (DateTime?)Convert.ToDateTime(httpData["DateShipped"].ToString()) : null;
                    billOfLading.FreightTerms = httpData["FreightTerms"].ToString();
                    billOfLading.ProjectId = user.ProjectId;
                    billOfLading.ToStorage = httpData["ToStorage"].ToString() == "true" ? true : false;
                    billOfLading.TrailerNumber = httpData["TrailerNumber"].ToString();
                    billOfLading.BillOfLadingId = !string.IsNullOrEmpty(httpData["BillOfLadingId"].ToString()) ? Convert.ToInt32(httpData["BillOfLadingId"].ToString()) : 0;

                    if (billOfLading.BillOfLadingId != 0)
                    {
                        billOfLadingService.Update(billOfLading);
                    }
                    else
                    {
                        billOfLadingService.Save(billOfLading);
                    }
                }



                //check all duplicates
                var equipmentBOs = equipmentService.GetForDuplicateCheck(user.ProjectId);
                billOfLadingEquipmentModels = billOfLadingEquipments.Select(x => new BillOfLadingEquipmentModel
                {
                    EquipmentId = x.EquipmentId,
                    Quantity = x.Quantity,
                    CountryOfOrigin = x.CountryOfOrigin,
                    HTSCode = x.HTSCode,
                    ShippedFrom = x.ShippedFrom,
                    BillOfLadingEquipmentId = x.BillOfLadingEquipmentId,
                    Equipment = new EquipmentModel
                    {
                        EquipmentId = x.Equipment.EquipmentId,
                        CustomsValue = x.Equipment.CustomsValue,
                        FullyShipped = x.Equipment.FullyShipped,
                        LeftToShip = x.Equipment.LeftToShip.ToString(),
                        PriorityNumber = x.Equipment.Priority != null ? (int?)x.Equipment.Priority.PriorityNumber : null,
                        ProjectId = x.Equipment.ProjectId,
                        Quantity = x.Equipment.Quantity.HasValue ? x.Equipment.Quantity.Value : 0,
                        ReadyToShip = x.Equipment.ReadyToShip,
                        ReleaseDate = x.Equipment.ReleaseDate,
                        SalePrice = x.Equipment.SalePrice,
                        ShippedQuantity = x.Equipment.ShippedQuantity.ToString(),
                        TotalWeight = x.Equipment.TotalWeight.ToString(),
                        TotalWeightShipped = x.Equipment.TotalWeightShipped.ToString(),
                        UnitWeight = x.Equipment.UnitWeight,
                        CountryOfOrigin = x.Equipment.CountryOfOrigin,
                        Description = x.Equipment.Description,
                        DrawingNumber = x.Equipment.DrawingNumber,
                        EquipmentName = x.Equipment.EquipmentName,
                        HTSCode = x.Equipment.HTSCode,
                        Notes = x.Equipment.Notes,
                        ShippedFrom = x.Equipment.ShippedFrom,
                        ShippingTagNumber = x.Equipment.ShippingTagNumber,
                        WorkOrderNumber = x.Equipment.WorkOrderNumber,

                        HasBillOfLading = x.Equipment.HasBillOfLading,
                        HasBillOfLadingInStorage = x.Equipment.HasBillOfLadingInStorage,
                        IsHardwareKit = x.Equipment.IsHardwareKit,
                        IsAssociatedToHardwareKit = x.Equipment.IsAssociatedToHardwareKit,
                        AssociatedHardwareKitNumber = x.Equipment.AssociatedHardwareKitNumber,
                        IsDuplicate = equipmentBOs.Any(e =>
                           e.EquipmentId != x.Equipment.EquipmentId &&
                           (e.DrawingNumber ?? string.Empty).Equals((x.Equipment.DrawingNumber ?? string.Empty), StringComparison.InvariantCultureIgnoreCase) &&
                           (e.WorkOrderNumber ?? string.Empty).Equals((x.Equipment.WorkOrderNumber ?? string.Empty), StringComparison.InvariantCultureIgnoreCase) &&
                           (e.ShippingTagNumber ?? string.Empty).Equals((x.Equipment.ShippingTagNumber ?? string.Empty), StringComparison.InvariantCultureIgnoreCase))
                    }
                }).ToList();

                billOfLadingEquipmentModels.ForEach(x =>
                {
                    x.Equipment.SetIndicators(project.ProjectNumber, project.IsCustomsProject);
                });
            }

            return new DtResponse { data = billOfLadingEquipmentModels };
        }

        [HttpGet]
        [HttpPost]
        public DtResponse Delete()
        {
            var user = userService.GetCurrentUser();


            if (user != null)
            {
                var httpData = DatatableHelpers.HttpData();
                Dictionary<string, object> data = httpData["data"] as Dictionary<string, object>;

                var billOfLadings = new List<int>();
                foreach (var billOfLadingIdString in data.Keys)
                {

                    var billOfLadingId = !string.IsNullOrEmpty(billOfLadingIdString) ? Convert.ToInt32(billOfLadingIdString) : 0;
                    if (billOfLadingId != 0)
                    {
                        billOfLadings.Add(billOfLadingId);
                    }
                }

                billOfLadingService.Delete(billOfLadings.FirstOrDefault());


            }

            return new DtResponse { };
        }

    }
}
