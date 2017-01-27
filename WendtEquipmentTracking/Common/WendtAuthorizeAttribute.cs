using System.Web;
using System.Web.Mvc;
using WendtEquipmentTracking.Common;

namespace WendtEquipmentTracking.App.Common
{
    public class WendtAuthorizeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var httpContext = context.HttpContext;

            string username = ActiveDirectoryHelper.CurrentUserUsername();
            var user = ActiveDirectoryHelper.GetUser(username);

            var authenticated = user.Role != UserRoles.None;
            if (!authenticated)
            {
                var urlHelper = new UrlHelper(context.RequestContext);
                var url = urlHelper.Action("Unauthorized", "Home", null);
                httpContext.Response.Redirect(url, true);
            }
            

            base.OnActionExecuting(context);
        }
        
    }
}