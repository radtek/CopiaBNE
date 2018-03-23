using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using System.Net.Http;

namespace APIGateway.Model
{
    public class Endpoint
    {
        public string RelativePath
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public HttpMethod Method
        {
            get
            {
                //throw new System.NotImplementedException();
                return HttpMethod.Delete;
            }
            set
            {
                

            }
        }

        public string GatewayRelativePath
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }
    }
}
