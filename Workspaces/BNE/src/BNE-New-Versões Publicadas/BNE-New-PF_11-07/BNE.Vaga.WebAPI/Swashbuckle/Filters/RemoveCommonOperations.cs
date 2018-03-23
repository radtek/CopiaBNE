using Swashbuckle.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace BNE.Vaga.WebAPI.Swashbuckle.Filters
{
    public class RemoveCommonOperations : IOperationFilter 
    {
        Regex rRemoveCommonOp = new Regex(@"/(Get|Post|Delete|Put)(?=\?|/{id}|$)", RegexOptions.IgnoreCase);

        /// <summary>
        /// Removendo operações de GET, POST, DELETE e PUT
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="schemaRegistry"></param>
        /// <param name="apiDescription"></param>
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, System.Web.Http.Description.ApiDescription apiDescription)
        {
            apiDescription.RelativePath = rRemoveCommonOp.Replace(apiDescription.RelativePath, "");
        }
    }
}