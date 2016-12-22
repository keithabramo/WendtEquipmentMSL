using System.IO;
using System.Web.Mvc;

namespace WendtEquipmentTracking.App.Controllers
{
    public class ValidateController : BaseController
    {

        // GET: ValidImportFile
        public ActionResult ValidImportFile(string file)
        {
            var extension = Path.GetExtension(file);

            if (!extension.Equals(".xlsx"))
            {
                return Json("Only xlsx files are allowed.", JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }

    }
}
