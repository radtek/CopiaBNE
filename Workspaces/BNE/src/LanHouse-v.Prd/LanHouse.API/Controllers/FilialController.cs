using LanHouse.Business;
using LanHouse.Business.EL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using LanHouse.Entities.BNE;
using LanHouse.Business.DTO;
using System.Web.Http.Cors;

namespace LanHouse.API.Controllers
{
    public class FilialController : LanHouse.API.Code.BaseController
    {
        private Code.ICacheService cache = new Code.InMemoryCache();

        /// <summary>
        /// Carregar a filial (lan house)
        /// </summary>
        /// <param name="nomeFilial"></param>
        /// <returns>JSON com os dados da Filial</returns>
        public HttpResponseMessage Get(string partialName)
        {
            try
            {
                //Lan house para teste no dev.
                string diretorio = partialName;
                //string diretorio = "globosat";
                FilialOrigemFilial objFilialDTO = null;

                TAB_Origem_Filial objOrigemFilial;
                if(Filial.CarregarPorDiretorio(diretorio, out objOrigemFilial))
                {
                    objFilialDTO = new FilialOrigemFilial();

                    objFilialDTO.Diretorio = objOrigemFilial.Des_Diretorio;
                    objFilialDTO.NomeFantasia = objOrigemFilial.TAB_Filial.Nme_Fantasia;
                    objFilialDTO.Cnpj = objOrigemFilial.TAB_Filial.Num_CNPJ;
                    objFilialDTO.IdFilial = objOrigemFilial.TAB_Filial.Idf_Filial;
                    objFilialDTO.IdFilialOrigem = objOrigemFilial.Idf_Origem;

                    //Busca GeoLocalização da filial
                    objFilialDTO.GeoLocalizacao = Filial.RecuperarGeoLocalizacao(objFilialDTO.IdFilial);

                    //Buscar array de byte da logo.
                    objFilialDTO.logoLan = Companhia.RecuperarLogo(objFilialDTO.Cnpj.Value);
                }

                return Request.CreateResponse(HttpStatusCode.OK, objFilialDTO);
            }
            catch(Exception ex)
            {
                string msgErro;
                Business.EL.GerenciadorException.GravarExcecao(ex, out msgErro, "Lan House API - Carregar a filial");
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }
    }
}
