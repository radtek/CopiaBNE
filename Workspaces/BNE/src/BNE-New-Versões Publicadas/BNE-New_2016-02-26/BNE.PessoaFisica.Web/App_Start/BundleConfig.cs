using System.Configuration;
using System.IO;
using System.Web.Hosting;
using System.Web.Optimization;

namespace BNE.PessoaFisica.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.UseCdn = true;

            var pathToStaticFiles = ConfigurationManager.AppSettings["PathToStaticFiles"];

            bundles.Add(new ScriptBundle("~/bundles/ajax", "/" + pathToStaticFiles + "/bundles/ajax" + FileVersion("~/Scripts/jquery.unobtrusive-ajax.js")).Include("~/Scripts/jquery.unobtrusive-ajax.js"));
            bundles.Add(new StyleBundle("~/bundles/css", "/" + pathToStaticFiles + "/bundles/css" + FileVersion("~/Content/sass/styles.css")).Include("~/Content/sass/styles.css"));
            bundles.Add(new ScriptBundle("~/bundles/site", "/" + pathToStaticFiles + "/bundles/site" + FileVersion("~/Scripts/site/util.js")).Include("~/Scripts/site/util.js"));

            bundles.Add(new ScriptBundle("~/bundles/validate", "/" + pathToStaticFiles + "/bundles/validate" + FileVersion("~/Scripts/jquery.validate.custom.pt-br.js")).Include("~/Scripts/jquery.validate.custom.pt-br.js"));

            //material
            bundles.Add(new StyleBundle("~/bundles/cssmaterial", "https://storage.googleapis.com/code.getmdl.io/1.0.5/material.indigo-red.min.css").Include("~/Content/material/material.css"));
            bundles.Add(new ScriptBundle("~/bundles/jsmaterial", "https://storage.googleapis.com/code.getmdl.io/1.0.4/material.min.js").Include("~/Content/material/material.js"));
        
            var componentes = new[]
            {
                "~/Scripts/Components/BNE.Components.Web.js",
                "~/Scripts/Components/BNE.Components.Web.Util.js",
                "~/Scripts/Components/BNE.Components.Web.Data.js",
                "~/Scripts/Components/BNE.Components.Web.CPF.js",
                "~/Scripts/Components/BNE.Components.Web.Telefone.js",
                "~/Scripts/Components/BNE.Components.Web.Decimal.js",
                "~/Scripts/Components/BNE.Components.Web.RadioButton.js",
                "~/Scripts/Components/BNE.Components.Web.DropDownList.js",
                "~/Scripts/Components/BNE.Components.Web.Textbox.js",
                "~/Scripts/Components/BNE.Components.Web.Autocomplete.js"
            };
            bundles.Add(new ScriptBundle("~/bundles/scripts/components", "/" + pathToStaticFiles + "/bundles/scripts/components" + FileVersion(componentes)).Include(componentes));

            bundles.Add(new ScriptBundle("~/bundles/pre-curriculo", "/" + pathToStaticFiles + "/bundles/pre-curriculo" + FileVersion("~/Scripts/pre-curriculo.js")).Include("~/Scripts/pre-curriculo.js"));
            bundles.Add(new ScriptBundle("~/bundles/pessoa-fisica", "/" + pathToStaticFiles + "/bundles/pessoa-fisica" + FileVersion("~/Scripts/site/pessoa-fisica.js")).Include("~/Scripts/site/pessoa-fisica.js"));

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