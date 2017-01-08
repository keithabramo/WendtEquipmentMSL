using System.Web;
using System.Web.Mvc;
using WendtEquipmentTracking.Common;
using WendtEquipmentTracking.Common.DTO;

namespace WendtEquipmentTracking.App.Common
{
    public static class HtmlHelperExtensions
    {
        public static string CurrentVersion(this HtmlHelper html)
        {
            return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        public static string CurrentUsername(this HtmlHelper html)
        {
            string username = ActiveDirectoryHelper.CurrentUserUsername();

            return ActiveDirectoryHelper.DisplayName(username);
        }

        public static bool UserIsInRole(this HtmlHelper html, UserRoles role)
        {
            ActiveDirectoryUser user = HttpContext.Current.Cache["currentUser"] as ActiveDirectoryUser;

            if (user == null)
            {
                string username = ActiveDirectoryHelper.CurrentUserUsername();
                user = ActiveDirectoryHelper.GetUser(username);

                HttpContext.Current.Cache["currentUser"] = user;

            }

            return role == user.Role;
        }
    }
}