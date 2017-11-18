using System.Web.Mvc;
using WendtEquipmentTracking.BusinessLogic;
using WendtEquipmentTracking.BusinessLogic.Api;

namespace WendtEquipmentTracking.App.Controllers
{
    public class AdminController : BaseController
    {
        private IProjectService projectService;
        private IUserService userService;

        public AdminController()
        {
            projectService = new ProjectService();
            userService = new UserService();
        }

        //
        // GET: /Project/

        public ViewResult Index()
        {
            return View();
        }


    }
}
