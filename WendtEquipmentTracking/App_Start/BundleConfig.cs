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
                        "~/Scripts/respond.js",
                        "~/Scripts/main.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/form").Include(
                        "~/Scripts/jquery.validate*",
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.inputmask.js",
                        "~/Scripts/form.js"));

            bundles.Add(new ScriptBundle("~/bundles/table")
                .Include(
                        "~/Scripts/moment.min.js",
                        "~/Scripts/editor/datatablesAndEditor.js",
                        "~/Scripts/editor/table.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/editor")
                .Include(
                        "~/Scripts/moment.min.js",
                        "~/Scripts/editor/datatablesAndEditor.js",
                        "~/Scripts/editor/editor.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/dropzone").Include(
                     "~/Scripts/dropzone/dropzone.js"));






            bundles.Add(new ScriptBundle("~/bundles/masterShipList").Include(
                        "~/Scripts/editor/timer.js",
                        "~/Scripts/editor/editorMSL.js"
            ));
            bundles.Add(new ScriptBundle("~/bundles/billOfLadingCreate").Include(
                       "~/Scripts/editor/editorBOLCreate.js"));
            bundles.Add(new ScriptBundle("~/bundles/billOfLadingEdit").Include(
                       "~/Scripts/editor/editorBOLEdit.js"));

            bundles.Add(new ScriptBundle("~/bundles/hardwareKitCreate").Include(
                       "~/Scripts/editor/editorHardwareKitCreate.js"));
            bundles.Add(new ScriptBundle("~/bundles/hardwareKitEdit").Include(
                       "~/Scripts/editor/editorHardwareKitEdit.js"));
            bundles.Add(new ScriptBundle("~/bundles/workOrderPrices").Include(
                        "~/Scripts/editor/editorWorkOrderPrice.js"));
            bundles.Add(new ScriptBundle("~/bundles/vendors").Include(
                        "~/Scripts/editor/editorVendor.js"));
            bundles.Add(new ScriptBundle("~/bundles/brokers").Include(
                        "~/Scripts/editor/editorBroker.js"));


            bundles.Add(new ScriptBundle("~/bundles/billOfLadings").Include(
                       "~/Scripts/editor/tableBOL.js"));
            bundles.Add(new ScriptBundle("~/bundles/billOfLadingDetails").Include(
                       "~/Scripts/editor/tableBOLDetails.js"));
            bundles.Add(new ScriptBundle("~/bundles/hardwareKits").Include(
                       "~/Scripts/editor/tableHardwareKit.js"));
            bundles.Add(new ScriptBundle("~/bundles/hardwareKitDetails").Include(
                       "~/Scripts/editor/tableHardwareKitDetails.js"));
            bundles.Add(new ScriptBundle("~/bundles/projects").Include(
                       "~/Scripts/editor/tableProject.js"));
            bundles.Add(new ScriptBundle("~/bundles/hardwareCommercialCodes").Include(
                       "~/Scripts/editor/tableHardwareCommercialCode.js"));
            bundles.Add(new ScriptBundle("~/bundles/priorities").Include(
                       "~/Scripts/editor/editorPriority.js"));
            bundles.Add(new ScriptBundle("~/bundles/admin").Include(
                       "~/Scripts/editor/tableAdmin.js"));

            bundles.Add(new ScriptBundle("~/bundles/importEquipment").Include(
                       "~/Scripts/editor/importEquipmentTable.js",
                       "~/Scripts/editor/importEquipment.js"
            ));
            bundles.Add(new ScriptBundle("~/bundles/importWorkOrderPrice").Include(
                       "~/Scripts/editor/importWorkOrderPriceTable.js",
                       "~/Scripts/editor/importWorkOrderPrice.js"
            ));
            bundles.Add(new ScriptBundle("~/bundles/importRawEquipment").Include(
                       "~/Scripts/editor/importRawEquipmentTable.js",
                       "~/Scripts/editor/importRawEquipment.js"
            ));
            bundles.Add(new ScriptBundle("~/bundles/importPriority").Include(
                       "~/Scripts/editor/importPriorityTable.js",
                       "~/Scripts/editor/importPriority.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/createProject").Include(
                       "~/Scripts/editor/createProject.js"
            ));















            bundles.Add(new StyleBundle("~/Content/themes/base/jqueryui")
                .IncludeDirectory("~/Content/themes/base", "*.css"));


            bundles.Add(new StyleBundle("~/Content/css")
                .Include(
                    "~/Content/bootstrap.css",
                    "~/Content/datatablesAndEditor.css",
                    "~/Content/Site.css"
                ));

            bundles.Add(new StyleBundle("~/Content/dropzone").Include(
                     "~/Scripts/dropzone/basic.css",
                     "~/Scripts/dropzone/dropzone.css"));
        }
    }
}
