using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LanHouse.Entities.BNE;

namespace LanHouse.API.Controllers
{
    [Authorize]
    public class PesquisaSalarialController : LanHouse.API.Code.BaseController
    {
        // GET: api/PesquisaSalarial
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpPost]
        // POST: api/PesquisaSalarial
        public string FazerPesquisaSalarial(string cidade, string funcao)
        {
            try
            {
                string retorno = "";
                int idEstado = 0;

                var array = cidade.Split('-');

                if (array.Length == 2)
                {
                    string _cid = array[0];
                    string _uf = array[1];

                    TAB_Cidade objCidade;
                    Business.Cidade.CarregarCidadeporNome(_cid, _uf, out objCidade);

                    TAB_Estado objEstado;
                    Business.Estado.CarregarEstadoporSigla(objCidade.Sig_Estado, out objEstado);

                    int idFuncao = Business.Funcao.RecuperarIDporNome(funcao);

                    TAB_Pesquisa_Salarial objPesquisaSalarial;
                    Business.PesquisaSalarial.CarregarMedialSalarialFuncao(idFuncao, objEstado.Idf_Estado, out objPesquisaSalarial);

                    if (objPesquisaSalarial != null)
                        retorno = string.Format("{0} em {1} ganha entre {2} e {3}.", funcao, cidade, objPesquisaSalarial.Vlr_Junior.ToString("C"), objPesquisaSalarial.Vlr_Master.ToString("C"));
                    else
                        retorno = "Não foi possível encontrar a média salarial.";
                }

                return retorno;
            }
            catch (Exception ex)
            {
                string msgErro;
                Business.EL.GerenciadorException.GravarExcecao(ex, out msgErro, "Lan House API - Carregar a média salarial.s");
                return "Não foi possível encontrar a média salarial.";
            }
        }

        // PUT: api/PesquisaSalarial/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/PesquisaSalarial/5
        public void Delete(int id)
        {
        }
    }
}
