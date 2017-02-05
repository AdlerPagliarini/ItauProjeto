using System.Web;
using System.Web.Optimization;

namespace ItauProjeto
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*",
                        "~/Scripts/custom/methods_pt.js"));// para validar data em portugues

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap-337/bootstrap.js",
                      "~/Scripts/respond.js"));

            //http://getbootstrap.com/getting-started/#download
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap-337/bootstrap.min.css",
                      "~/Content/bootstrap-337/bootstrap-theme.min.css",
                      "~/Content/Site.css"));

            //https://plugins.jquery.com/mask/
            bundles.Add(new ScriptBundle("~/bundles/mask").Include(
                      "~/Scripts/mask/zepto/zepto.min.js",
                      "~/Scripts/mask/zepto/data.js",
                      "~/Scripts/mask/jquery.mask.js"));

            
            bundles.Add(new ScriptBundle("~/bundles/autocep").Include(
                      "~/Scripts/custom/auto-cep.js"));

            bundles.Add(new ScriptBundle("~/bundles/ajxandjs").Include(
                      "~/Scripts/custom/ajax-js-functions.js"));

        }
    }
}
