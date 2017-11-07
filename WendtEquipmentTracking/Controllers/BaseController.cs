using log4net;
using System;
using System.Linq;
using System.Web.Mvc;
using WendtEquipmentTracking.App.Common;
using WendtEquipmentTracking.Common;

namespace WendtEquipmentTracking.App.Controllers
{
    [WendtAuthorize]
    public class BaseController : Controller
    {
        protected ILog logger = LogManager.GetLogger("File");


        protected void SuccessMessage(string message)
        {
            TempData.SetStatusMessage(message, StatusCodes.Success);
        }

        protected void InformationMessage(string message)
        {
            TempData.SetStatusMessage(message, StatusCodes.Information);
        }

        protected void ErrorMessage(string message)
        {
            TempData.SetStatusMessage(message, StatusCodes.Error);
        }


        protected void HandleError(string clientMessage, Exception e)
        {
            var guid = Guid.NewGuid();
            TempData.SetStatusMessage(clientMessage + ". If this issue continues please give the following issue ID to your system administrator: " + guid, StatusCodes.Error);

            logger.Error("MSL Controller Error - GUID: " + guid + " User: " + ActiveDirectoryHelper.CurrentUserUsername() + " Exception: ", e);
        }

        protected void HandleError(string clientMessage, ModelStateDictionary modelState)
        {
            var guid = Guid.NewGuid();
            TempData.SetStatusMessage(clientMessage + ". If this issue continues please give the following issue ID to your system administrator: " + guid, StatusCodes.Error);

            var errors = string.Join(";", ModelState.Where(v => v.Value.Errors.Count() > 0).ToList().Select(v => v.Key + " : " + v.Value.Errors.First().ErrorMessage));
            logger.Error("Custom ModelState Errors - GUID: " + guid + " User: " + ActiveDirectoryHelper.CurrentUserUsername() + " Errors: " + errors);
        }
    }
}