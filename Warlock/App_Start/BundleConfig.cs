using System.Web;
using System.Web.Optimization;

namespace Warlock
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                "~/Scripts/jquery-1.11.2.js",
                "~/Scripts/bootstrap.js",
                "~/Scripts/warlock.js"));

            bundles.Add(new StyleBundle("~/bundles/css").Include(
                "~/Content/css/bootstrap.css",
                "~/Content/css/warlock.css"));
        }
    }
}