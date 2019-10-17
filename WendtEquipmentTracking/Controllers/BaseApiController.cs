using log4net;
using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using WendtEquipmentTracking.App.Common;
using WendtEquipmentTracking.Common;

namespace WendtEquipmentTracking.App.Controllers
{
    [WendtAuthorize]
    public class BaseApiController : ApiController
    {
        protected ILog logger = LogManager.GetLogger("File");


        protected void HandleError(Exception e)
        {

            logger.Error("MSL API Controller Error " + ActiveDirectoryHelper.CurrentUserUsername() + ": ", e);
        }

        protected void HandleError(ModelStateDictionary modelState)
        {
            var errors = string.Join(";", ModelState.Where(v => v.Value.Errors.Count() > 0).ToList().Select(v => v.Key + " : " + v.Value.Errors.First().ErrorMessage));
            logger.Error("API Custom ModelState Errors " + ActiveDirectoryHelper.CurrentUserUsername() + ": " + errors);
        }
    }
}