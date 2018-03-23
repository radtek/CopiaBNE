using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using APIGateway.Data;

namespace APIGateway.Domain
{
    public class SwaggerConfig
    {
        private static List<Model.SwaggerConfig> _swaggerConfigs = null;

        public static List<Model.SwaggerConfig> SwaggerConfigs
        {
            get
            {
                if (_swaggerConfigs == null)
                    _swaggerConfigs = GetSwaggerUIEndpoints();
                return _swaggerConfigs;
            }
        }

        /// <summary>
        /// Returns the formated URL for BNE Swagger.
        /// </summary>
        /// <param name="Theme">Theme to be used in BNE Swagger</param>
        /// <param name="Authentication">Authentication to be used in BNE Swagger</param>
        /// <param name="FileUrl">File url to be used in BNE Swagger</param>
        /// <returns>Formatted Url of BNE Swagger.</returns>
        public static String GetUrl(string Theme, string Authentication, string FileUrl)
        {
            return Settings.Default.SwaggerUiUrl + String.Format("?theme={0}&url={1}&auth={2}", Theme, FileUrl, Authentication);
        }

        /// <summary>
        /// Returns the formated URL for BNE Swagger.
        /// </summary>
        /// <param name="Theme">Theme to be used in BNE Swagger</param>
        /// <param name="Authentication">Authentication to be used in BNE Swagger</param>
        /// <param name="FileUrl">File url to be used in BNE Swagger</param>
        /// <returns>Formatted Url of BNE Swagger.</returns>
        public static String GetSwaggerUrl()
        {
            return Settings.Default.SwaggerUiUrl;
        }

        /// <summary>
        /// Reads all bytes from a json swagger file present in swagger files dir.
        /// </summary>
        /// <param name="file">Filename</param>
        /// <returns>Array of bytes retrived from file.</returns>
        public static String GetFile(string file)
        {
            if (!Directory.Exists(Settings.Default.SwaggerFilesDir))
                Directory.CreateDirectory(Settings.Default.SwaggerFilesDir);

            string path = Path.Combine(Settings.Default.SwaggerFilesDir, file);
            if (!File.Exists(path))
                return null;

            return File.ReadAllText(path);
        }

        public static List<Model.SwaggerConfig> GetSwaggerUIEndpoints()
        {
            using (var _ctx = new APIGatewayContext())
            {
                var lst = from c in _ctx.SwaggerConfig.Include("Authentication").Include("Api")
                          where c.UIUrl != null
                          select c;

                return lst.ToList();
            }
        }
    }
}
