using System.Web.Mvc;
using WendtEquipmentTracking.App.Common;

namespace WendtEquipmentTracking.App.Controllers
{
    [WendtAuthorize]
    public class BaseController : Controller
    {
    }
}