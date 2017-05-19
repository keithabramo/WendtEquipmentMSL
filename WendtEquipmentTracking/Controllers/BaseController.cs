using log4net;
using System;
using System.Web.Mvc;
using WendtEquipmentTracking.App.Common;

namespace WendtEquipmentTracking.App.Controllers
{
    [WendtAuthorize]
    public class BaseController : Controller
    {
        protected ILog logger = LogManager.GetLogger("File");


        protected void LogError(Exception e)
        {
            logger.Error("MSL Controller Error", e);
        }
    }
}