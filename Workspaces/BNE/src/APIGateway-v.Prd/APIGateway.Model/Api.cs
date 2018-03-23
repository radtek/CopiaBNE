using System;
using System.Web;
using System.ComponentModel;
using System.Collections.Generic;

namespace APIGateway.Model
{
    public class Api
    {
        /// <summary>
        /// Última parte da URL pública. Será usada
        /// </summary>
        public String UrlSuffix
        {
            get;
            set;
        }

        public Authentication AuthenticationType
        {
            get;
            set;
        }

        public string Url
        {
            get
            {
#if DEBUG
                return DevUrl;
#else
                return BaseUrl;
#endif
            }
        }

        public string BaseUrl
        {
            get;
            set;
        }

        public string DevUrl
        {
            get;
            set;
        }

        public List<SistemaCliente> Sistemas
        {
            get;
            set;
        }

        public List<Endpoint> Endpoints
        {
            get;
            set;
        }

        public SwaggerConfig SwaggerConfig { get; set; }
    }
}
