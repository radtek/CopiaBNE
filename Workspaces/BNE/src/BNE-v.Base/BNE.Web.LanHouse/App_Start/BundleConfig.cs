using System.Web;
using System.Web.Optimization;

namespace BNE.Web.LanHouse
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/site").Include(
                        "~/Scripts/local/Util.js",
                        "~/Scripts/local/Facebook.js",
                        "~/Scripts/local/Site.js",
                        "~/Scripts/jquery.maskMoney.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery.unobtrusive-ajax.js",
                        "~/Scripts/i18n/grid.locale-pt-br.js",
                        "~/Scripts/jquery.jqGrid.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css",
                "~/Content/jquery.fancybox.css",
                        "~/Content/jquery.jqGrid/ui.jqgrid.css"));

            bundles.Add(new ScriptBundle("~/bundles/controles/employer").Include(
                        "~/Scripts/controle/employer.js",
                        "~/Scripts/controle/employer.util.js",
                        "~/Scripts/controle/employer.event.js",
                        "~/Scripts/controle/employer.key.js",
                        "~/Scripts/controle/employer.controle.js",
                        "~/Scripts/controle/employer.controle.mvc.js",
                        "~/Scripts/controle/employer.controle.mvc.cnpj.js",
                        "~/Scripts/controle/employer.controle.mvc.cpf.js",
                        "~/Scripts/controle/employer.controle.mvc.data.js",
                        "~/Scripts/controle/employer.controle.mvc.telefone.js",
                        "~/Scripts/controle/employer.controle.mvc.cep.js",
                        "~/Scripts/controle/employer.controle.mvc.lista.js"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.theme.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css"
                        ));

        }
    }
}