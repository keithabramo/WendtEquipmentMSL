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
    public class BillOfLadingAttachmentApiController : BaseApiController
    {
        private IBillOfLadingAttachmentService billOfLadingAttachmentService;
        private IUserService userService;


        public BillOfLadingAttachmentApiController()
        {
            userService = new UserService();
            billOfLadingAttachmentService = new BillOfLadingAttachmentService();
        }

        [HttpGet]
        public IEnumerable<BillOfLadingAttachmentModel> Table(int billOfLadingId)
        {
            var model = new List<BillOfLadingAttachmentModel>();

            var billOfLadingAttachments = billOfLadingAttachmentService.GetByBillOfLadingId(billOfLadingId);

            if (billOfLadingAttachments == null)
            {
                return model;
            }

            model = billOfLadingAttachments.Select(x => new BillOfLadingAttachmentModel
            {
                BillOfLadingAttachmentId = x.BillOfLadingAttachmentId,
                BillOfLadingId = x.BillOfLadingId,
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

                var billOfLadingAttachmentIds = new List<int>();
                foreach (var billOfLadingAttachmentIdString in data.Keys)
                {

                    var billOfLadingAttachmentId = !string.IsNullOrEmpty(billOfLadingAttachmentIdString) ? Convert.ToInt32(billOfLadingAttachmentIdString) : 0;
                    if (billOfLadingAttachmentId != 0)
                    {
                        billOfLadingAttachmentIds.Add(billOfLadingAttachmentId);
                    }
                }

                billOfLadingAttachmentService.Delete(billOfLadingAttachmentIds.FirstOrDefault());
            }

            return new DtResponse { };
        }

    }
}
