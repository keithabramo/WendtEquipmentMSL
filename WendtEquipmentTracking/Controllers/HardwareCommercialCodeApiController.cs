
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using WendtEquipmentTracking.App.Common;
using WendtEquipmentTracking.App.Models;
using WendtEquipmentTracking.BusinessLogic;
using WendtEquipmentTracking.BusinessLogic.Api;

namespace WendtEquipmentTracking.App.Controllers
{
    public class HardwareCommercialCodeApiController : ApiController
    {
        private IHardwareCommercialCodeService hardwareCommercialCodeService;
        private IUserService userService;

        public HardwareCommercialCodeApiController()
        {
            hardwareCommercialCodeService = new HardwareCommercialCodeService();
            userService = new UserService();
        }

        //
        // GET: api/HardwareCommercialCodeApi/Table
        [HttpGet]
        public IEnumerable<HardwareCommercialCodeModel> Table()
        {

            //Get Data
            var hardwareCommercialCodeBOs = hardwareCommercialCodeService.GetAll();

            var hardwareCommercialCodeModels = hardwareCommercialCodeBOs.Select(x => new HardwareCommercialCodeModel
            {
                CommodityCode = x.CommodityCode,
                Description = x.Description,
                HardwareCommercialCodeId = x.HardwareCommercialCodeId,
                PartNumber = x.PartNumber
            });

            return hardwareCommercialCodeModels;
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

                var hardwareCommercialCodes = new List<int>();
                foreach (var hardwareCommecialCodeIdString in data.Keys)
                {

                    var hardwareCommecialCodeId = !string.IsNullOrEmpty(hardwareCommecialCodeIdString) ? Convert.ToInt32(hardwareCommecialCodeIdString) : 0;
                    if (hardwareCommecialCodeId != 0)
                    {
                        hardwareCommercialCodes.Add(hardwareCommecialCodeId);
                    }
                }

                hardwareCommercialCodeService.Delete(hardwareCommercialCodes.FirstOrDefault());


            }

            return new DtResponse { };
        }
    }
}