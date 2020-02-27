using System.Web.Optimization;

namespace IdentiGo.WebManagement
{
    public class BundleConfig
    {
        // Para obtener más información sobre Bundles, visite http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //BundleTable.EnableOptimizations = true;

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*",
                        "~/Scripts/validate.fixes.js"));

            bundles.Add(new ScriptBundle("~/bundles/theme").Include(
                "~/Scripts/app.min.js",
                "~/Scripts/bootstrap.js"//,
                //"~/Scripts/plugins/fastclick/fastclick.js",
                //"~/Scripts/plugins/slimScroll/query.slimscroll.min.js"
            ));

            // Utilice la versión de desarrollo de Modernizr para desarrollar y obtener información. De este modo, estará
            // preparado para la producción y podrá utilizar la herramienta de compilación disponible en http://modernizr.com para seleccionar solo las pruebas que necesite.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/fileinput.js",
                      "~/Scripts/bootstrap-tag/js/bootstrap-tag.js",
                      "~/Scripts/fastclick.js",
                      "~/Scripts/Chart.js",
                      "~/Scripts/moment.min.js",
                      "~/Scripts/daterangepicker.js",
                      "~/Scripts/bootstrap-colorpicker.js",
                      "~/Scripts/jquery.dataTables.js",
                      "~/Scripts/dataTables.bootstrap4.js",
                      "~/Scripts/custom.js"));
            

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.min.css",
                      "~/Content/skins/flat/green.css",
                      "~/Content/daterangepicker.css",
                      "~/Content/bootstrap-fileinput/css/fileinput.css",
                      "~/Content/bootstrap-tag/bootstrap-tag.css",
                      "~/Content/bootstrap-colorpicker.css",
                      "~/Content/dataTables.bootstrap4.css",
                      "~/Content/custom.css"));

        }
    }
}
