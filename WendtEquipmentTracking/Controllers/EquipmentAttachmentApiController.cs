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
    public class EquipmentAttachmentApiController : BaseApiController
    {
        private IEquipmentAttachmentService equipmentAttachmentService;
        private IUserService userService;


        public EquipmentAttachmentApiController()
        {
            userService = new UserService();
            equipmentAttachmentService = new EquipmentAttachmentService();
        }

        [HttpGet]
        public IEnumerable<EquipmentAttachmentModel> Table(int equipmentId)
        {
            var model = new List<EquipmentAttachmentModel>();

            var equipmentAttachments = equipmentAttachmentService.GetByEquipmentId(equipmentId);

            if (equipmentAttachments == null)
            {
                return model;
            }

            model = equipmentAttachments.Select(x => new EquipmentAttachmentModel
            {
                EquipmentAttachmentId = x.EquipmentAttachmentId,
                EquipmentId = x.EquipmentId,
                FileName = x.FileName,
                FileTitle = x.FileTitle
            }).ToList();

            model = model.OrderBy(h => h.FileTitle).ToList();

            return model;
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

                var equipmentAttachmentIds = new List<int>();
                foreach (var equipmentAttachmentIdString in data.Keys)
                {

                    var equipmentAttachmentId = !string.IsNullOrEmpty(equipmentAttachmentIdString) ? Convert.ToInt32(equipmentAttachmentIdString) : 0;
                    if (equipmentAttachmentId != 0)
                    {
                        equipmentAttachmentIds.Add(equipmentAttachmentId);
                    }
                }

                equipmentAttachmentService.Delete(equipmentAttachmentIds.FirstOrDefault());
            }

            return new DtResponse { };
        }

    }
}
