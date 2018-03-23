using System;
using System.Xml;
using System.Configuration;
using System.Runtime;
using System.Web;
using System.Web.Configuration;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace BNE.Componentes.Validadores
{
    class Helpers
    {
        public static bool EnableLegacyRendering()
        {
            bool result;
            try
            {
                string webConfigFile = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "web.config");
                var webConfigReader = new XmlTextReader(new StreamReader(webConfigFile));
                result = ((webConfigReader.ReadToFollowing("xhtmlConformance")) && (webConfigReader.GetAttribute("mode") == "Legacy"));
                webConfigReader.Close();
            }
            catch
            {
                result = false;
            }
            return result;
        }
    }
}

