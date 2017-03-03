using AutoMapper;
using System.Collections.Generic;
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
            var hardwareCommercialCodeModels = new List<HardwareCommercialCodeModel>();

            //Get Data
            var hardwareCommercialCodeBOs = hardwareCommercialCodeService.GetAll();

            hardwareCommercialCodeModels = Mapper.Map<List<HardwareCommercialCodeModel>>(hardwareCommercialCodeBOs);

            return hardwareCommercialCodeModels;
        }

    }
}
