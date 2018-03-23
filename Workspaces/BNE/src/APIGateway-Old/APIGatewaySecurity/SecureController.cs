using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace APIGatewaySecurity
{
    [SecureFilter]
    public class SecureController : ApiController
    {
        public string ApiKey { get; set; }
    }
}
