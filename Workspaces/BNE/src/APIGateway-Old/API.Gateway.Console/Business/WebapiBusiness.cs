using API.Gateway.Console.APIModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Gateway.Console.Business
{
    public static class WebapiBusiness
    {

        public static IPagedList<WebApi> Carregar(int Pagina, int PorPagina = 50)
        {
            using (var dbo = new APIGatewayContext())
            {
                return dbo.WebApi.ToPagedList(Pagina, PorPagina);
            }
        }

    }
}