using System.Web;
using System.Web.Optimization;

namespace LHSCamp
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Content/Scripts/jQuery/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Content/Scripts/jQuery/jquery.validate*"));

            bundles.Add(new StyleBundle("~/bundles/fonts").Include(
                      "~/Content/Fonts/Quickly/Quickly-font.css",
                      "~/Content/Fonts/Railway/Railway-font.css"));
        }
    }
}
