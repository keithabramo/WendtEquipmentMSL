
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using WendtEquipmentTracking.App.Models;
using WendtEquipmentTracking.BusinessLogic;
using WendtEquipmentTracking.BusinessLogic.Api;

namespace WendtEquipmentTracking.App.Controllers
{
    public class HardwareCommercialCodeApiController : ApiController
    {
        private IHardwareCommercialCodeService hardwareCommercialCodeService;

        public HardwareCommercialCodeApiController()
        {
            hardwareCommercialCodeService = new HardwareCommercialCodeService();
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

    }
}
