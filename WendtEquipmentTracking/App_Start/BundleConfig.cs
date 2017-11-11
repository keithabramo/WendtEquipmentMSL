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
                        "~/Scripts/moment.min.js",
                        "~/Scripts/editor/datatablesAndEditor.min.js",
                        "~/Scripts/editor/table.js"));

            bundles.Add(new ScriptBundle("~/bundles/editor").Include(
                        "~/Scripts/moment.min.js",
                        "~/Scripts/editor/datatablesAndEditor.min.js",
                        "~/Scripts/editor/editor.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/dropzone").Include(
                     "~/Scripts/dropzone/dropzone.js"));






            bundles.Add(new ScriptBundle("~/bundles/masterShipList").Include(
                        "~/Scripts/editor/editorMSL.js"
            ));
            bundles.Add(new ScriptBundle("~/bundles/billOfLadingCreate").Include(
                       "~/Scripts/editor/editorBOLCreate.js"));
            bundles.Add(new ScriptBundle("~/bundles/billOfLadingEdit").Include(
                       "~/Scripts/editor/editorBOLEdit.js"));
            bundles.Add(new ScriptBundle("~/bundles/workOrderPrices").Include(
                        "~/Scripts/editor/editorWorkOrderPrice.js"));


            bundles.Add(new ScriptBundle("~/bundles/billOfLadings").Include(
                       "~/Scripts/editor/tableBOL.js"));
            bundles.Add(new ScriptBundle("~/bundles/billOfLadingDetails").Include(
                       "~/Scripts/editor/tableBOLDetails.js"));
            bundles.Add(new ScriptBundle("~/bundles/projects").Include(
                       "~/Scripts/editor/tableProject.js"));
            bundles.Add(new ScriptBundle("~/bundles/hardwareCommercialCodes").Include(
                       "~/Scripts/editor/tableHardwareCommercialCode.js"));
            bundles.Add(new ScriptBundle("~/bundles/priorities").Include(
                       "~/Scripts/editor/tablePriority.js"));

            bundles.Add(new ScriptBundle("~/bundles/importEquipment").Include(
                       "~/Scripts/editor/importEquipmentTable.js",
                       "~/Scripts/editor/importEquipment.js"
            ));
            bundles.Add(new ScriptBundle("~/bundles/importWorkOrderPrice").Include(
                       "~/Scripts/editor/importWorkOrderPrice.js"));




            bundles.Add(new ScriptBundle("~/bundles/hardwareKit").Include(
                       "~/Scripts/hardwareKit.js"));





















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
