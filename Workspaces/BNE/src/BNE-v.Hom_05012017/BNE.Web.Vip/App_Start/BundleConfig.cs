using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Optimization;

namespace BNE.Web.Vip
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            RegisterCustomBundlers(bundles);
            RegisterDefaultBundles(bundles);

            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = true;
        }
        private static void RegisterCustomBundlers(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/app/header").Include(
        "~/Scripts/ie-emulation-modes-warning.js",
                //"~/Scripts/ie8-responsive-file-warning.js",
        "~/Scripts/ie10-viewport-bug-workaround.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/app/footer").Include(
        "~/Scripts/jquery.js",
        "~/Scripts/bootstrap.js",
        "~/Scripts/app/custom.js"));

        }

        private static void RegisterDefaultBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.unobtrusive*",
                "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap.js",
                "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css/zAspNet").Include(
                 "~/Content/bootstrap.css",
                 "~/Content/zAspNet/zAspNetSite.css"));
        }
    }
}
