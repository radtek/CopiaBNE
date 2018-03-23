using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace APIGateway.Model
{
    public class OAuthConfig
    {
        public string Interface { get; set; }

        public string TokenEndpoint { get; set; }

        public String AuthenticationEndpoint { get; set; }

        public string SecretKey { get; set; }

        public int Expiration { get; set; }

        public virtual Authentication Authentication { get; set; }
    }
}
