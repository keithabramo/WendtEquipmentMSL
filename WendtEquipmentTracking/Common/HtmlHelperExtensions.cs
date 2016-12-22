using System.Web.Mvc;
using WendtEquipmentTracking.BusinessLogic;
using WendtEquipmentTracking.Common;

namespace WendtEquipmentTracking.App.Common
{
    public static class HtmlHelperExtensions {
        public static string CurrentVersion(this HtmlHelper html) {
            return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        public static string CurrentUsername(this HtmlHelper html) {
            string username = ActiveDirectoryHelper.CurrentUserUsername();

            return ActiveDirectoryHelper.DisplayName(username);
        }

        public static bool UserIsInRole(this HtmlHelper html, UserRoles role)
        {
            return true;
        }
    }
}