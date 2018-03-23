using APIGateway.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIGateway.Domain
{
    public class OAuthConfig
    {
        public static String ObterSecretKey(String interfaceName)
        {
            using (var _context = new APIGatewayContext())
            {
                return (from c in _context.OAuthConfig
                        where c.Interface.Equals(interfaceName)
                        select c.SecretKey).FirstOrDefault();
            }
        }
    }
}
