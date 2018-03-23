using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BNE.PessoaFisica.Web.DTO
{
    public class ApiGatewayToken
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public string scope { get; set; }
        public DateTime expires_in { get; set; }
    }
}