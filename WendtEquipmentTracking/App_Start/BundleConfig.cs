using System.Web.Optimization;

namespace WendtEquipmentTracking.App
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/main").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-ui-{version}.js",
                        "~/Scripts/bootstrap.js",
                        "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/form").Include(
                        "~/Scripts/jquery.validate*",
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.inputmask.js",
                        "~/Scripts/form.js"));

            bundles.Add(new ScriptBundle("~/bundles/table").Include(
                        "~/Scripts/datatablesAndEditor.min.js",
                        "~/Scripts/jquery.floatThead.js",
                        "~/Scripts/table.js"));

            bundles.Add(new ScriptBundle("~/bundles/editor").Include(
                        "~/Scripts/datatablesAndEditor.min.js",
                        "~/Scripts/jquery.floatThead.js",
                        "~/Scripts/editor.js"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/masterShipList").Include(
                       "~/Scripts/mslRender.js",
                       "~/Scripts/masterShipList.js"));

            bundles.Add(new ScriptBundle("~/bundles/hardwareKit").Include(
                       "~/Scripts/hardwareKit.js"));

            bundles.Add(new ScriptBundle("~/bundles/billOfLading").Include(
                       "~/Scripts/billOfLading.js"));

            bundles.Add(new ScriptBundle("~/bundles/import").Include(
                       "~/Scripts/import.js"));

            bundles.Add(new ScriptBundle("~/bundles/workOrderPrice").Include(
                       "~/Scripts/workOrderPrice.js"));

            bundles.Add(new ScriptBundle("~/bundles/project").Include(
                       "~/Scripts/project.js"));

            bundles.Add(new ScriptBundle("~/bundles/dropzone").Include(
                     "~/Scripts/dropzone/dropzone.js"));

            bundles.Add(new StyleBundle("~/Content/css")
                .IncludeDirectory("~/Content/themes/base", "*.css")
                .Include(
                    "~/Content/bootstrap.css",
                    "~/Content/datatablesAndEditor.min.css",
                    "~/Content/Site.css"
                ));

            bundles.Add(new StyleBundle("~/Content/dropzone").Include(
                     "~/Scripts/dropzone/basic.css",
                     "~/Scripts/dropzone/dropzone.css"));
        }
    }
}
