using BNE.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bne.Web.Services.API.Controllers
{
    public class TabelasController : ApiController
    {
        /// <summary>
        /// Returns a list of jobs title.
        /// </summary>
        /// <param name="objPesquisaCurriculo"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("Funcoes")]
        public string[] Funcoes(String nomeParcial, int numeroRegistros = 10)
        {
            if (numeroRegistros > 50)
                numeroRegistros = 50;
            
            return Funcao.RecuperarFuncoes(nomeParcial, numeroRegistros, null);
        }

        /// <summary>
        /// Returns a list of cities.
        /// </summary>
        /// <param name="objPesquisaCurriculo"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("Cidades")]
        public string[] Cidades(String nomeParcial, int numeroRegistros = 10)
        {
            if (numeroRegistros > 50)
                numeroRegistros = 50;

            List<String> retorno = new List<string>();
            foreach (var item in Cidade.RecuperarNomesCidadesEstado(nomeParcial, null, numeroRegistros))
            {
                retorno.Add(item.Value);
            }

            return retorno.ToArray();
        }

        
    }
}
