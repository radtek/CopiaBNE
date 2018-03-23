using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIGateway.Model
{
    public class Authorization
    {

        public Endpoint Endpoint { get; set; }
        public Perfil Perfil { get; set; }

        public short EndpointId { get; set; }
    }
}