using System.Configuration;
using System.IO;
using System.Web.Hosting;
using System.Web.Optimization;

namespace BNE.PessoaJuridica.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.UseCdn = true;

            var pathToStaticFiles = ConfigurationManager.AppSettings["PathToStaticFiles"];

            bundles.Add(new ScriptBundle("~/bundles/ajax", "/" + pathToStaticFiles + "/bundles/ajax" + FileVersion("~/Scripts/jquery.unobtrusive-ajax.js")).Include("~/Scripts/jquery.unobtrusive-ajax.js"));

            bundles.Add(new StyleBundle("~/bundles/css", "/" + pathToStaticFiles + "/bundles/css" + FileVersion("~/Content/styles.css")).Include("~/Content/styles.css"));
            bundles.Add(new ScriptBundle("~/bundles/site", "/" + pathToStaticFiles + "/bundles/site" + FileVersion("~/Scripts/site/util.js")).Include("~/Scripts/site/util.js"));
            bundles.Add(new ScriptBundle("~/bundles/cadastro-empresa", "/" + pathToStaticFiles + "/bundles/cadastro-empresa" + FileVersion("~/Scripts/site/cadastro-empresa.js")).Include("~/Scripts/site/cadastro-empresa.js"));
            bundles.Add(new ScriptBundle("~/bundles/cadastro-usuario", "/" + pathToStaticFiles + "/bundles/cadastro-usuario" + FileVersion("~/Scripts/site/cadastro-usuario.js")).Include("~/Scripts/site/cadastro-usuario.js"));

            var componentes = new[]
            {
                "~/Scripts/Components/BNE.Components.Web.js",
                "~/Scripts/Components/BNE.Components.Web.Util.js",
                "~/Scripts/Components/BNE.Components.Web.CNPJ.js",
                "~/Scripts/Components/BNE.Components.Web.Data.js",
                "~/Scripts/Components/BNE.Components.Web.CPF.js",
                "~/Scripts/Components/BNE.Components.Web.Telefone.js",
                "~/Scripts/Components/BNE.Components.Web.Textbox.js",
                "~/Scripts/Components/BNE.Components.Web.RadioButton.js",
                "~/Scripts/Components/BNE.Components.Web.Autocomplete.js"
            };
            bundles.Add(new ScriptBundle("~/bundles/scripts/components", "/" + pathToStaticFiles + "/bundles/scripts/components" + FileVersion(componentes)).Include(componentes));
        }

        public static string FileVersion(string[] rootRelativePaths)
        {
            long ticks = 0;
            foreach (var rootRelativePath in rootRelativePaths)
            {
                var absolutePath = HostingEnvironment.MapPath(rootRelativePath);
                if (absolutePath != null)
                {
                    var lastChangedDateTime = File.GetLastWriteTime(absolutePath);
                    if (lastChangedDateTime.Ticks > ticks)
                        ticks = lastChangedDateTime.Ticks;
                }

            }
            return "?v=" + ticks;
        }

        public static string FileVersion(string rootRelativePath)
        {
            return FileVersion(new[] { rootRelativePath });
        }

    }
}