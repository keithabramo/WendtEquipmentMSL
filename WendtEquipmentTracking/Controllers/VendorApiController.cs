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
    public class VendorApiController : ApiController
    {
        private IVendorService vendorService;
        private IUserService userService;

        public VendorApiController()
        {
            vendorService = new VendorService();
            userService = new UserService();
        }

        //
        // GET: api/Vendor/Table
        [HttpGet]
        public IEnumerable<VendorModel> Table()
        {
            var user = userService.GetCurrentUser();

            var vendorBOs = vendorService.GetAll();

            var vendorModels = vendorBOs.Select(x => new VendorModel
            {
                Name = x.Name,
                Address = x.Address,
                Contact1 = x.Contact1,
                Email = x.Email,
                PhoneFax = x.PhoneFax,
                VendorId = x.VendorId
            }).ToList();

            vendorModels
                .GroupBy(x => x.Name != null ? x.Name.ToUpperInvariant() : string.Empty)
                .Where(g => g.Count() > 1)
                .SelectMany(y => y)
                .ToList().ForEach(e => e.IsDuplicate = true);

            vendorModels = vendorModels.OrderBy(r => r.Name).ToList();

            return vendorModels;
        }

        //
        // GET: api/Vendor/Editor
        [HttpGet]
        [HttpPost]
        public DtResponse Editor()
        {
            var user = userService.GetCurrentUser();
            var vendorModels = new List<VendorModel>();

            if (user != null)
            {

                var httpData = DatatableHelpers.HttpData();
                Dictionary<string, object> data = httpData["data"] as Dictionary<string, object>;
                var action = httpData["action"];


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
                    vendor.Name = vendorProperties["Name"].ToString();
                    vendor.PhoneFax = vendorProperties["PhoneFax"].ToString();

                    vendors.Add(vendor);
                }


                IEnumerable<int> vendorIds = new List<int>();
                if (action.Equals(EditorActions.edit.ToString()))
                {
                    vendorService.UpdateAll(vendors);

                    //return all rows so editor does not remove any from the ui
                    vendorIds = vendors.Select(x => x.VendorId);
                }
                else if (action.Equals(EditorActions.create.ToString()))
                {
                    vendorIds = vendorService.SaveAll(vendors);
                }
                else if (action.Equals(EditorActions.remove.ToString()))
                {
                    vendorService.Delete(vendors.FirstOrDefault().VendorId);
                }

                vendors = vendorService.GetByIds(vendorIds).ToList();

                var allVendors = vendorService.GetAll();
                vendorModels = vendors.Select(x => new VendorModel
                {
                    PhoneFax = x.PhoneFax,
                    Name = x.Name,
                    Email = x.Email,
                    Contact1 = x.Contact1,
                    Address = x.Address,
                    VendorId = x.VendorId,
                    IsDuplicate = allVendors.Any(w =>
                       w.VendorId != x.VendorId &&
                       (w.Name ?? string.Empty).Equals((x.Name ?? string.Empty), StringComparison.InvariantCultureIgnoreCase))
                }).ToList();
            }

            return new DtResponse { data = vendorModels };
        }
    }
}
