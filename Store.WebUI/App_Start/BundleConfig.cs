using System.Web.Optimization;

namespace Store.WebUI.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery.validate").Include(
            "~/Scripts/jquery.validate.js", "~/Scripts/jquery.validate.unobtrusive.js"));

            bundles.Add(new StyleBundle("~/Content/bootstrap").Include("~/Content/bootstrap.css", "~/Content/bootstrap-theme.css"));
            
            bundles.Add(new StyleBundle("~/Content/error").Include("~/Content/error-styles.css"));

            bundles.Add(new StyleBundle("~/Content/fancybox").Include("~/Content/jquery.fancybox.css"));

            bundles.Add(new ScriptBundle("~/bundles/fancybox").Include("~/Scripts/jquery.fancybox.pack.js"));
        }
    }
}