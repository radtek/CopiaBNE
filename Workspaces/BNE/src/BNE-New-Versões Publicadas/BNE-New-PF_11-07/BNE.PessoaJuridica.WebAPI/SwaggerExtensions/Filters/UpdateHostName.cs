using Swashbuckle.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Description;

namespace BNE.PessoaJuridica.WebAPI.SwaggerExtensions.Filters
{
    public class UpdateHostName : IDocumentFilter
    {
        public void Apply(SwaggerDocument swaggerDoc, SchemaRegistry schemaRegistry, IApiExplorer apiExplorer)
        {
            // Set this to your API host name
            swaggerDoc.host = System.Configuration.ConfigurationManager.AppSettings["host"];
            swaggerDoc.basePath = System.Configuration.ConfigurationManager.AppSettings["basepath"];
        }
    }
}