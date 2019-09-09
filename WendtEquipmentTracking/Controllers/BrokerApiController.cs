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
    public class BrokerApiController : ApiController
    {
        private IBrokerService brokerService;
        private IUserService userService;
        private IProjectService projectService;

        public BrokerApiController()
        {
            brokerService = new BrokerService();
            userService = new UserService();
            projectService = new ProjectService();
        }

        //
        // GET: api/Broker/Search
        [HttpGet]
        public IEnumerable<string> Search()
        {
            var brokers = new List<string>();

            var user = userService.GetCurrentUser();

            if (user == null)
            {
                return brokers;
            }

            //Get Data
            var brokerBOs = brokerService.GetAll();

            brokers = brokerBOs.Select(x => x.Name)
                                .OrderBy(e => e)
                                .ToList();

            return brokers;
        }

        //
        // GET: api/Broker/FindByName
        [HttpGet]
        public BrokerModel FindByName(string name)
        {
            var brokerBO = brokerService.GetByName(name);

            var model = new BrokerModel
            {
                Address = brokerBO.Address,
                BrokerId = brokerBO.BrokerId,
                Contact1 = brokerBO.Contact1,
                Email = brokerBO.Email,
                Name = brokerBO.Name,
                PhoneFax = brokerBO.PhoneFax
            };

            return model;
        }

        //
        // GET: api/Broker/Table
        [HttpGet]
        public IEnumerable<BrokerModel> Table()
        {
            var brokerBOs = brokerService.GetAll();

            var brokerModels = brokerBOs.Select(x => new BrokerModel
            {
                Name = x.Name,
                Address = x.Address,
                Contact1 = x.Contact1,
                Email = x.Email,
                PhoneFax = x.PhoneFax,
                BrokerId = x.BrokerId
            }).ToList();

            brokerModels
                .GroupBy(x => x.Name != null ? x.Name.ToUpperInvariant() : string.Empty)
                .Where(g => g.Count() > 1)
                .SelectMany(y => y)
                .ToList().ForEach(e => e.IsDuplicate = true);

            brokerModels = brokerModels.OrderBy(r => r.Name).ToList();

            return brokerModels;
        }

        //
        // GET: api/Broker/Editor
        [HttpGet]
        [HttpPost]
        public DtResponse Editor()
        {
            var user = userService.GetCurrentUser();
            var brokerModels = new List<BrokerModel>();

            if (user != null)
            {
                var project = projectService.GetById(user.ProjectId);

                var httpData = DatatableHelpers.HttpData();
                Dictionary<string, object> data = httpData["data"] as Dictionary<string, object>;
                var action = httpData["action"];


                var brokers = new List<BrokerBO>();
                foreach (string brokerId in data.Keys)
                {
                    var row = data[brokerId];
                    var brokerProperties = row as Dictionary<string, object>;

                    BrokerBO broker = new BrokerBO();

                    broker.BrokerId = !string.IsNullOrWhiteSpace(brokerProperties["BrokerId"].ToString()) ? Convert.ToInt32(brokerProperties["BrokerId"]) : 0;
                    broker.Address = brokerProperties["Address"].ToString();
                    broker.Contact1 = brokerProperties["Contact1"].ToString();
                    broker.Email = brokerProperties["Email"].ToString();
                    broker.Name = brokerProperties["Name"].ToString();
                    broker.PhoneFax = brokerProperties["PhoneFax"].ToString();

                    brokers.Add(broker);
                }


                IEnumerable<int> brokerIds = new List<int>();
                if (action.Equals(EditorActions.edit.ToString()))
                {
                    brokerService.UpdateAll(brokers);

                    //return all rows so editor does not remove any from the ui
                    brokerIds = brokers.Select(x => x.BrokerId);
                }
                else if (action.Equals(EditorActions.create.ToString()))
                {
                    brokerIds = brokerService.SaveAll(brokers);
                }
                else if (action.Equals(EditorActions.remove.ToString()))
                {
                    brokerService.Delete(brokers.FirstOrDefault().BrokerId);
                }

                brokers = brokerService.GetByIds(brokerIds).ToList();

                var allBrokers = brokerService.GetAll();
                brokerModels = brokers.Select(x => new BrokerModel
                {
                    PhoneFax = x.PhoneFax,
                    Name = x.Name,
                    Email = x.Email,
                    Contact1 = x.Contact1,
                    Address = x.Address,
                    BrokerId = x.BrokerId,
                    IsDuplicate = allBrokers.Any(w =>
                       w.BrokerId != x.BrokerId &&
                       (w.Name ?? string.Empty).Equals((x.Name ?? string.Empty), StringComparison.InvariantCultureIgnoreCase))
                }).ToList();
            }

            return new DtResponse { data = brokerModels };
        }
    }
}
