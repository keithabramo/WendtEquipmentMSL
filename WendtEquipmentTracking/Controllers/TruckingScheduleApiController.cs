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
    public class TruckingScheduleApiController : ApiController
    {
        private ITruckingScheduleService truckingScheduleService;
        private IUserService userService;
        private IProjectService projectService;
        private IVendorService vendorService;

        public TruckingScheduleApiController()
        {
            truckingScheduleService = new TruckingScheduleService();
            userService = new UserService();
            projectService = new ProjectService();
            vendorService = new VendorService();
        }

        //
        // GET: api/TruckingSchedule/Table
        [HttpGet]
        public IEnumerable<TruckingScheduleModel> Table()
        {
            var truckingScheduleBOs = truckingScheduleService.GetAll();

            var truckingScheduleModels = truckingScheduleBOs.Select(x => new TruckingScheduleModel
            {
                Carrier = x.Carrier,
                Comments = x.Comments,
                Description = x.Description,
                Dimensions = x.Dimensions,
                NumPieces = x.NumPieces,
                PickUpDate = x.PickUpDate,
                ProjectNumber = x.Project != null ? (double?)x.Project.ProjectNumber : null,
                PurchaseOrder = x.PurchaseOrder,
                ShipFrom = getShippedFromValue(x),
                ShipTo = getShippedToValue(x),
                Status = x.Status,
                TruckingScheduleId = x.TruckingScheduleId,
                Weight = x.Weight,
                RequestDate = x.RequestDate,
                RequestedBy = x.RequestedBy
            }).ToList();

            truckingScheduleModels = truckingScheduleModels.OrderBy(r => r.RequestedBy).ToList();

            return truckingScheduleModels;
        }

        //
        // GET: api/TruckingSchedule/Editor
        [HttpGet]
        [HttpPost]
        public DtResponse Editor()
        {
            var user = userService.GetCurrentUser();
            var truckingScheduleModels = new List<TruckingScheduleModel>();

            if (user != null)
            {
                var projects = projectService.GetAll();
                var vendors = vendorService.GetAll();

                var httpData = DatatableHelpers.HttpData();
                Dictionary<string, object> data = httpData["data"] as Dictionary<string, object>;
                var action = httpData["action"];


                var truckingSchedules = new List<TruckingScheduleBO>();
                foreach (string truckingScheduleId in data.Keys)
                {
                    var row = data[truckingScheduleId];
                    var truckingScheduleProperties = row as Dictionary<string, object>;

                    TruckingScheduleBO truckingSchedule = new TruckingScheduleBO();
                    truckingSchedule.TruckingScheduleId = !string.IsNullOrWhiteSpace(truckingScheduleProperties["TruckingScheduleId"].ToString()) ? Convert.ToInt32(truckingScheduleProperties["TruckingScheduleId"]) : 0;
                    truckingSchedule.Carrier = truckingScheduleProperties["Carrier"].ToString();
                    truckingSchedule.Comments = truckingScheduleProperties["Comments"].ToString();
                    truckingSchedule.Description = truckingScheduleProperties["Description"].ToString();
                    truckingSchedule.Dimensions = truckingScheduleProperties["Dimensions"].ToString();
                    truckingSchedule.PurchaseOrder = truckingScheduleProperties["PurchaseOrder"].ToString();
                    truckingSchedule.Status = truckingScheduleProperties["Status"].ToString();
                    truckingSchedule.RequestedBy = truckingScheduleProperties["RequestedBy"].ToString();
                    truckingSchedule.Weight = truckingScheduleProperties["WeightText"].ToString().ToNullable<double>();
                    truckingSchedule.NumPieces = truckingScheduleProperties["NumPiecesText"].ToString().ToNullable<double>();
                    truckingSchedule.PickUpDate = !string.IsNullOrWhiteSpace(truckingScheduleProperties["PickUpDate"].ToString()) ? (DateTime?)Convert.ToDateTime(truckingScheduleProperties["PickUpDate"]) : null;
                    truckingSchedule.RequestDate = !string.IsNullOrWhiteSpace(truckingScheduleProperties["RequestDate"].ToString()) ? (DateTime?)Convert.ToDateTime(truckingScheduleProperties["RequestDate"]) : null;

                    var shipFromText = truckingScheduleProperties["ShipFrom"].ToString();
                    var shipFromVendor = vendors.FirstOrDefault(x => shipFromText.Contains(x.Name));
                    if (shipFromVendor != null)
                    {
                        truckingSchedule.ShipFromVendorId = shipFromVendor.VendorId;
                    }

                    var shipToText = truckingScheduleProperties["ShipTo"].ToString();
                    var shipToVendor = vendors.FirstOrDefault(x => shipToText.Contains(x.Name));
                    if (shipToVendor != null)
                    {
                        truckingSchedule.ShipToVendorId = shipToVendor.VendorId;
                    }


                    var projectNumberString = truckingScheduleProperties["ProjectNumber"].ToString();
                    double projectNumber = 0;
                    if (!string.IsNullOrWhiteSpace(projectNumberString) && Double.TryParse(projectNumberString, out projectNumber))
                    {
                        var project = projects.FirstOrDefault(x => x.ProjectNumber == projectNumber);
                        if (project != null)
                        {
                            truckingSchedule.ProjectId = project.ProjectId;
                        }
                    }

                    truckingSchedules.Add(truckingSchedule);
                }


                IEnumerable<int> truckingScheduleIds = new List<int>();
                if (action.Equals(EditorActions.edit.ToString()))
                {
                    truckingScheduleService.UpdateAll(truckingSchedules);

                    //return all rows so editor does not remove any from the ui
                    truckingScheduleIds = truckingSchedules.Select(x => x.TruckingScheduleId);
                }
                else if (action.Equals(EditorActions.create.ToString()))
                {
                    truckingScheduleIds = truckingScheduleService.SaveAll(truckingSchedules);
                }
                else if (action.Equals(EditorActions.remove.ToString()))
                {
                    truckingScheduleService.Delete(truckingSchedules.FirstOrDefault().TruckingScheduleId);
                }

                truckingSchedules = truckingScheduleService.GetByIds(truckingScheduleIds).ToList();

                truckingScheduleModels = truckingSchedules.Select(x => new TruckingScheduleModel
                {
                    Carrier = x.Carrier,
                    Comments = x.Comments,
                    Description = x.Description,
                    Dimensions = x.Dimensions,
                    NumPieces = x.NumPieces,
                    PickUpDate = x.PickUpDate,
                    PurchaseOrder = x.PurchaseOrder,
                    ShipFrom = getShippedFromValue(x),
                    ShipTo = getShippedToValue(x),
                    Status = x.Status,
                    TruckingScheduleId = x.TruckingScheduleId,
                    Weight = x.Weight,
                    RequestDate = x.RequestDate,
                    RequestedBy = x.RequestedBy,
                    ProjectNumber = x.Project != null ? (double?)x.Project.ProjectNumber : null
                }).ToList();
            }

            return new DtResponse { data = truckingScheduleModels };
        }

        public string getShippedFromValue(TruckingScheduleBO truckingScheduleBO)
        {
            string result = null;

            if (truckingScheduleBO.ShipFromVendor != null)
            {
                result = truckingScheduleBO.ShipFromVendor.Name + " " + truckingScheduleBO.ShipFromVendor.Address;
            }
            else
            {
                result = "Wendt 2555 Walden Ave, Buffalo, NY 14075";
            }

            return result;

        }

        public string getShippedToValue(TruckingScheduleBO truckingScheduleBO)
        {
            string result = null;

            if (truckingScheduleBO.ShipToVendor != null)
            {
                result = truckingScheduleBO.ShipToVendor.Name + " " + truckingScheduleBO.ShipToVendor.Address;
            }
            else if (truckingScheduleBO.Project != null)
            {
                result = (truckingScheduleBO.Project.ShipToCompany ?? "");
                if (!string.IsNullOrEmpty(truckingScheduleBO.Project.ShipToAddress))
                {
                    result += " " + truckingScheduleBO.Project.ShipToAddress;
                }
                if (!string.IsNullOrEmpty(truckingScheduleBO.Project.ShipToCSZ))
                {
                    result += " " + truckingScheduleBO.Project.ShipToCSZ;
                }
                if (!string.IsNullOrEmpty(truckingScheduleBO.Project.ReceivingHours))
                {
                    result += " : " + truckingScheduleBO.Project.ReceivingHours;
                }
            }

            return result;

        }
    }
}
