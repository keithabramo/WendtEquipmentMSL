﻿using System.Web.Optimization;

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
                        "~/Scripts/waitingDialog.js"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/billOfLanding").Include(
                       "~/Scripts/BillOfLanding.js"));



            bundles.Add(new StyleBundle("~/Content/css")
                .Include(
                    "~/Content/bootstrap.css",
                    "~/Content/Site.css"
                ));
        }
    }
}
