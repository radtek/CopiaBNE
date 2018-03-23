using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace APIGateway.Authentication
{
    interface IAuthentication
    {
        Model.Usuario Authenticate(HttpRequestMessage request, out Model.SistemaCliente sistema);
    }
}