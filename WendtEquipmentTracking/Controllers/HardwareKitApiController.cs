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
    public class HardwareKitApiController : BaseApiController
    {
        private IEquipmentService equipmentService;
        private IHardwareKitService hardwareKitService;
        private IProjectService projectService;
        private IUserService userService;

        private const float DEFAULT_PERCENT = 10;


        public HardwareKitApiController()
        {
            equipmentService = new EquipmentService();
            projectService = new ProjectService();
            userService = new UserService();
            hardwareKitService = new HardwareKitService();
        }


        [HttpGet]
        public IEnumerable<HardwareKitModel> Table()
        {
            IEnumerable<HardwareKitModel> hardwareKitModels = new List<HardwareKitModel>();

            var user = userService.GetCurrentUser();

            if (user == null)
            {
                return hardwareKitModels;
            }

            var hardwareKitBOs = hardwareKitService.GetCurrentByProject(user.ProjectId);
            hardwareKitModels = hardwareKitBOs.Select(x => new HardwareKitModel
            {
                ExtraQuantityPercentage = x.ExtraQuantityPercentage,
                HardwareKitId = x.HardwareKitId,
                HardwareKitNumber = x.HardwareKitNumber,
                IsCurrentRevision = x.IsCurrentRevision,
                ProjectId = x.ProjectId,
                Revision = x.Revision
            });

            return hardwareKitModels;
        }

        [HttpGet]
        public IEnumerable<HardwareKitModel> DetailsTable(string hardwareKitNumber)
        {
            IList<HardwareKitModel> hardwareKitModels = new List<HardwareKitModel>();

            var user = userService.GetCurrentUser();

            if (user == null)
            {
                return hardwareKitModels;
            }

            var hardwareKits = hardwareKitService.GetByHardwareKitNumber(user.ProjectId, hardwareKitNumber);

            if (hardwareKits == null)
            {
                return hardwareKitModels;
            }

            foreach (var hardwareKitBO in hardwareKits)
            {
                var hardwareGroupModels = hardwareKitBO.HardwareKitEquipments
                .GroupBy(e => new { e.Equipment.ShippingTagNumber, e.Equipment.Description }, (key, g) => new HardwareKitGroupModel
                {
                    ShippingTagNumber = key.ShippingTagNumber,
                    Description = key.Description,
                    Quantity = g.Sum(e => e.Equipment.Quantity.HasValue ? e.Equipment.Quantity.Value : 0),
                    QuantityToShip = (int)Math.Ceiling(g.Sum(e => e.QuantityToShip))
                }).ToList();

                var hardwareKitModel = new HardwareKitModel
                {
                    ExtraQuantityPercentage = hardwareKitBO.ExtraQuantityPercentage,
                    HardwareKitId = hardwareKitBO.HardwareKitId,
                    HardwareKitNumber = hardwareKitBO.HardwareKitNumber,
                    IsCurrentRevision = hardwareKitBO.IsCurrentRevision,
                    ProjectId = hardwareKitBO.ProjectId,
                    Revision = hardwareKitBO.Revision,
                    HardwareGroups = hardwareGroupModels
                };

                hardwareKitModels.Add(hardwareKitModel);
            }

            hardwareKitModels = hardwareKitModels.OrderByDescending(h => h.Revision).ToList();

            return hardwareKitModels;
        }

        //
        // GET: api/EquipmentApi/CreateTable
        [HttpGet]
        public IEnumerable<HardwareKitGroupModel> CreateTable()
        {
            IList<HardwareKitGroupModel> hardwareKitGroupModels = new List<HardwareKitGroupModel>();

            var user = userService.GetCurrentUser();

            if (user == null)
            {
                return hardwareKitGroupModels;
            }

            var projectBO = projectService.GetById(user.ProjectId);

            //Get Data
            var equipmentBOs = equipmentService.GetAll(user.ProjectId)
                .Where(e => e.IsHardware && !e.IsAssociatedToHardwareKit && !e.IsHardwareKit);

            var random = new Random();
            var hardwareGroups = equipmentBOs
                .GroupBy(e => new { e.ShippingTagNumber, e.Description }, (key, g) => new HardwareKitGroupModel
                {
                    HardwareKitGroupId = random.Next(),
                    ShippingTagNumber = key.ShippingTagNumber,
                    Description = key.Description,
                    Quantity = g.Sum(e => e.Quantity.HasValue ? e.Quantity.Value : 0)
                }).ToList();

            hardwareGroups.ForEach(hg => hg.QuantityToShip = (int)Math.Ceiling(hg.Quantity + (hg.Quantity * (DEFAULT_PERCENT / 100))));

            return hardwareGroups;
        }



        //
        // GET: api/HardwareKitApi/EditTable
        [HttpGet]
        public IEnumerable<HardwareKitGroupModel> EditTable(int hardwareKitId)
        {
            IList<HardwareKitGroupModel> hardwareKitGroupModels = new List<HardwareKitGroupModel>();

            var user = userService.GetCurrentUser();

            if (user == null)
            {
                return hardwareKitGroupModels;
            }


            //Get existing bill of lading information
            var hardwareKit = hardwareKitService.GetById(hardwareKitId);

            var random = new Random();
            var hardwareGroupModels = hardwareKit.HardwareKitEquipments
                .GroupBy(e => new { e.Equipment.ShippingTagNumber, e.Equipment.Description }, (key, g) => new HardwareKitGroupModel
                {
                    HardwareKitGroupId = random.Next(),
                    ShippingTagNumber = key.ShippingTagNumber,
                    Description = key.Description,
                    Quantity = g.Sum(e => e.Equipment.Quantity.HasValue ? e.Equipment.Quantity.Value : 0),
                    QuantityToShip = (int)Math.Ceiling(g.Sum(e => e.QuantityToShip))
                }).ToList();


            //get not added hardware groups
            var equipmentBOs = equipmentService.GetAll(user.ProjectId).Where(e =>
                e.IsHardware
                && !e.IsAssociatedToHardwareKit
                && !e.IsHardwareKit
                && !hardwareGroupModels.Any(mhg => mhg.ShippingTagNumber == e.ShippingTagNumber && mhg.Description == e.Description));


            var newHardwareGroups = equipmentBOs
                .GroupBy(e => new { e.ShippingTagNumber, e.Description }, (key, g) => new HardwareKitGroupModel
                {
                    HardwareKitGroupId = random.Next(),
                    ShippingTagNumber = key.ShippingTagNumber,
                    Description = key.Description,
                    Quantity = g.Sum(e => e.Quantity.HasValue ? e.Quantity.Value : 0)
                }).ToList();

            newHardwareGroups.ForEach(hg => hg.QuantityToShip = (int)Math.Ceiling(hg.Quantity + (hg.Quantity * (hardwareKit.ExtraQuantityPercentage / 100))));


            hardwareGroupModels.AddRange(newHardwareGroups);

            return hardwareGroupModels;
        }










        //
        // GET: api/EquipmentApi/Editor
        [HttpGet]
        [HttpPost]
        public DtResponse Editor()
        {
            var user = userService.GetCurrentUser();
            var hardwareKitGroupModels = new List<HardwareKitGroupModel>();


            if (user != null)
            {
                var project = projectService.GetById(user.ProjectId);

                var httpData = DatatableHelpers.HttpData();
                Dictionary<string, object> data = httpData["data"] as Dictionary<string, object>;
                var action = httpData["action"];

                foreach (string hardwareKitGroupKey in data.Keys)
                {
                    var row = data[hardwareKitGroupKey];
                    var hardwareKitGroupProperties = row as Dictionary<string, object>;

                    var hardwareKitGroupModel = new HardwareKitGroupModel();
                    hardwareKitGroupModel.HardwareKitGroupId = Convert.ToInt32(hardwareKitGroupProperties["HardwareKitGroupId"].ToString());
                    hardwareKitGroupModel.Description = hardwareKitGroupProperties["Description"].ToString();
                    hardwareKitGroupModel.Quantity = Convert.ToInt32(hardwareKitGroupProperties["Quantity"].ToString());
                    hardwareKitGroupModel.QuantityToShip = Convert.ToInt32(hardwareKitGroupProperties["QuantityToShip"].ToString());
                    hardwareKitGroupModel.ShippingTagNumber = hardwareKitGroupProperties["ShippingTagNumber"].ToString();

                    hardwareKitGroupModels.Add(hardwareKitGroupModel);
                }

                var doSubmit = httpData["doSubmit"];
                if (doSubmit.ToString() == "true")
                {
                    var hardwareKitId = !string.IsNullOrEmpty(httpData["HardwareKitId"].ToString()) ? Convert.ToInt32(httpData["HardwareKitId"].ToString()) : 0;
                    var extraQuantityPercentage = !string.IsNullOrEmpty(httpData["ExtraQuantityPercentage"].ToString()) ? Convert.ToDouble(httpData["ExtraQuantityPercentage"].ToString()) : 0;
                    var hardwareKitNumber = httpData["HardwareKitNumber"].ToString();


                    var hardwareKitEquipmentsBOs = new List<HardwareKitEquipmentBO>();
                    foreach (var hardwareGroup in hardwareKitGroupModels)
                    {
                        var hardwareInWorkOrderBOs = equipmentService.GetHardwareByShippingTagNumberAndDescription(user.ProjectId, hardwareGroup.ShippingTagNumber, hardwareGroup.Description);
                        hardwareKitEquipmentsBOs.AddRange(hardwareInWorkOrderBOs.Select(h => new HardwareKitEquipmentBO
                        {
                            EquipmentId = h.EquipmentId,
                            HardwareKitId = hardwareKitId,
                            QuantityToShip = h.Quantity.HasValue ? h.Quantity.Value + (h.Quantity.Value * (extraQuantityPercentage / 100)) : 0
                        }));
                    }

                    var hardwareKitBO = new HardwareKitBO
                    {
                        ExtraQuantityPercentage = extraQuantityPercentage,
                        HardwareKitId = hardwareKitId,
                        HardwareKitNumber = hardwareKitNumber,
                        ProjectId = user.ProjectId,
                        HardwareKitEquipments = hardwareKitEquipmentsBOs
                    };


                    if (hardwareKitBO.HardwareKitId != 0)
                    {
                        hardwareKitService.Update(hardwareKitBO);
                    }
                    else
                    {
                        hardwareKitService.Save(hardwareKitBO);
                    }
                }
            }

            return new DtResponse { data = hardwareKitGroupModels };
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

                var hardwareKits = new List<int>();
                foreach (var hardwareKitIdString in data.Keys)
                {

                    var hardwareKitId = !string.IsNullOrEmpty(hardwareKitIdString) ? Convert.ToInt32(hardwareKitIdString) : 0;
                    if (hardwareKitId != 0)
                    {
                        hardwareKits.Add(hardwareKitId);
                    }
                }

                hardwareKitService.Delete(hardwareKits.FirstOrDefault());
            }

            return new DtResponse { };
        }

    }
}
