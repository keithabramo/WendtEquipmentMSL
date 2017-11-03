﻿using System.Web.Mvc;

namespace WendtEquipmentTracking.App.Common
{
    public static class StatusMessage
    {
        public static void SetStatusMessage(this ViewDataDictionary viewData, string message, StatusCodes? statusCode = StatusCodes.Success)
        {
            viewData["StatusMessage"] = message;
            viewData["StatusCode"] = statusCode;
        }
    }
}