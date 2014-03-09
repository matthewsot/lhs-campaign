using System.Web;
using System.Web.Optimization;

namespace LHSCamp
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery-raw").Include(
                        "~/Content/Scripts/jQuery/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Content/Scripts/GSAP/CSSPlugin.min.js",
                "~/Content/Scripts/GSAP/EasePack.min.js",
                "~/Content/Scripts/GSAP/TweenLite.min.js",
                "~/Content/Scripts/jQuery/jquery-{version}.js",
                "~/Content/Scripts/GSAP/jquery.gsap.min.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/wedge").Include(
                //"~/Content/Scripts/Flauntly/Core.js",
                "~/Content/Scripts/Flauntly/Wedge/Core.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Content/Scripts/jQuery/jquery.validate*"));
        }
    }
}
