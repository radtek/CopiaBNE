using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace APIGateway.Model
{
    public class Authentication
    {
        public string Interface { get; set; }
        public string Descricao { get; set; }
        public OAuthConfig OAuthConfig { get; set; }
    }
}
