﻿using System.Web;
using System.Web.Optimization;

namespace TruckCheckUp.WebUI
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));
            
            //Custom Script for CRUD operations on table TruckManufacturer
            bundles.Add(new ScriptBundle("~/bundles/custom").Include(
                       "~/Scripts/Custom/truckmanufacturer-1.0.js"));

            //Custom Script for CRUD operations on table TruckModel
            bundles.Add(new ScriptBundle("~/bundles/custom").Include(
                       "~/Scripts/Custom/truckmodel-1.0.js"));

            //Custom Script for CRUD operations on table Truck
            bundles.Add(new ScriptBundle("~/bundles/custom").Include(
                       "~/Scripts/Custom/truck-1.0.js"));

            //Custom Script for dropdownlist truck numbers in InspectionReport controller
            bundles.Add(new ScriptBundle("~/bundles/custom").Include(
                       "~/Scripts/Custom/inspectionreport-trucknumberselection-1.0.js"));

            //Common Script for all JS files
            bundles.Add(new ScriptBundle("~/bundles/custom").Include(
                       "~/Scripts/Custom/Common.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/bootstrap-toggle.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap-spacelabs.css",
                      "~/Content/site.css", "~/Content/bootstrap-toggle.css"));
        }
    }
}
