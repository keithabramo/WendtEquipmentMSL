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
                        "~/Scripts/es6-promise.js",
                        "~/Scripts/main.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/form").Include(
                        "~/Scripts/jquery.validate*",
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.inputmask.js",
                        "~/Scripts/form.js"));

            bundles.Add(new ScriptBundle("~/bundles/datatable")
                .Include(
                        "~/Scripts/moment.min.js",
                        "~/Scripts/editor/datatablesAndEditor.js"
                ));


            bundles.Add(new ScriptBundle("~/bundles/table")
                .Include(
                        "~/Scripts/editor/table.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/editor")
                .Include(
                        "~/Scripts/editor/editor.autocomplete.js",
                        "~/Scripts/editor/editor.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/dropzone").Include(
                     "~/Scripts/dropzone.js"));

            bundles.Add(new ScriptBundle("~/bundles/html2canvas").Include(
                     "~/Scripts/html2canvas.min.js"));






            bundles.Add(new ScriptBundle("~/bundles/masterShipList").Include(
                        "~/Scripts/editor/timer.js",
                        "~/Scripts/editor/editorMSL.js",
                        "~/Scripts/editor/tableEquipmentAttachment.js"
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
            bundles.Add(new ScriptBundle("~/bundles/truckingSchedules").Include(
                        "~/Scripts/editor/editorTruckingSchedule.js"));


            bundles.Add(new ScriptBundle("~/bundles/billOfLadings").Include(
                       "~/Scripts/editor/tableBOL.js",
                        "~/Scripts/editor/tableBOLAttachment.js"
            ));
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
            bundles.Add(new ScriptBundle("~/bundles/importEquipmentRevision").Include(
                       "~/Scripts/editor/importEquipmentRevisionTable.js",
                       "~/Scripts/editor/importEquipmentRevision.js"
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
            bundles.Add(new ScriptBundle("~/bundles/importVendor").Include(
                       "~/Scripts/editor/importVendorTable.js",
                       "~/Scripts/editor/importVendor.js"
            ));
            bundles.Add(new ScriptBundle("~/bundles/importBroker").Include(
                       "~/Scripts/editor/importBrokerTable.js",
                       "~/Scripts/editor/importBroker.js"
            ));
            bundles.Add(new ScriptBundle("~/bundles/importHardwareCommercialCode").Include(
                       "~/Scripts/editor/importHardwareCommercialCodeTable.js",
                       "~/Scripts/editor/importHardwareCommercialCode.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/createProject").Include(
                       "~/Scripts/editor/createProject.js"
            ));
            bundles.Add(new ScriptBundle("~/bundles/editProject").Include(
                       "~/Scripts/editor/editProject.js"
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
                     "~/Content/dropzone.css"));
        }
    }
}
