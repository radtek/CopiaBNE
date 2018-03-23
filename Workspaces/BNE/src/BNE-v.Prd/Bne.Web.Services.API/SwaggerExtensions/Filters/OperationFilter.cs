using Swashbuckle.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bne.Web.Services.API.SwaggerExtensions.Filters
{
    /// <summary>
    /// Classe para customizar a geracao das operacoes
    /// </summary>
    public class OperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, System.Web.Http.Description.ApiDescription apiDescription)
        {
            if (operation.operationId == "Curriculo_PostFoto")  // controller and action name
            {
                operation.consumes.Add("multipart/form-data");
                operation.parameters = new List<Parameter>()
                {
                    new Parameter
                    {
                        name = "file",
                        @in = "formData",
                        required = false,
                        type = "file"
                    }
                };
            }
            else
                operation.consumes = operation.consumes.Where(o => o == "application/json").ToList();
        }
    }
}