using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIGateway.Model
{
    public class SwaggerConfig
    {
        /// <summary>
        /// Última parte da URL pública. Será usada
        /// </summary>
        public String UrlSuffix
        {
            get;
            set;
        }

        public Api Api
        {
            get;
            set;
        }

        /// <summary>
        /// Relative Url for SwaggerUI
        /// </summary>
        public string UIUrl { get; set; }

        /// <summary>
        /// Relative Url for Swagger File
        /// </summary>
        public string FileUrl { get; set; }

        /// <summary>
        /// File name of json file location
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Theme to be used in Swagger
        /// </summary>
        public string Theme { get; set; }

        /// <summary>
        /// Full Url of Swagger Ui
        /// </summary>
        public string FullUIUrl
        {
            get
            {
                return Flurl.Url.Combine(ConfigurationManager.AppSettings["GatewayUrl"].ToString(), Api.UrlSuffix, UIUrl);
            }
        }

        /// <summary>
        /// Full Url of Swagger File
        /// </summary>
        public string FullFileUrl
        {
            get
            {
                return Flurl.Url.Combine(ConfigurationManager.AppSettings["GatewayUrl"].ToString(), Api.UrlSuffix, FileUrl);
            }
        }

        /// <summary>
        /// Authentication to be used in Swagger
        /// </summary>
        public Authentication Authentication { get; set; }
    }
}
