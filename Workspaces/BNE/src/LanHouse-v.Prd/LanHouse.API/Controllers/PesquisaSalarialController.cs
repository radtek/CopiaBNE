using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LanHouse.Entities.BNE;
using LanHouse.Entities.SalarioBR;
using System.Xml.Serialization;
using LanHouse.Business.DTO;
using System.IO;
using System.Xml;
using System.Collections;
using Newtonsoft.Json;

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
                string retorno = "Não foi possível encontrar a média salarial.";

                var array = cidade.Split('-');

                if (array.Length == 2)
                {
                    string _cid = array[0];
                    string _uf = array[1];

                    TAB_Cidade objCidade;
                    Business.Cidade.CarregarCidadeporNome(_cid, _uf, out objCidade);

                    int idFuncao = Business.Funcao.RecuperarIDporNome(funcao);

                    TAB_Resultado_Pesquisa_Salarial objPesquisaSalarial;
                    Business.PesquisaSalarial.CarregarMedialSalarialFuncao(idFuncao, objCidade.Sig_Estado, out objPesquisaSalarial);

                    if (objPesquisaSalarial != null)
                    {
                        //Serializar xml Pesquisa para classe
                        XmlSerializer serializer = new XmlSerializer(typeof(PesquisaSalarial));
                        IList<PesquisaSalarial> pesquisas = new List<PesquisaSalarial>();
                        PesquisaSalarial pesquisa = null;
                        XmlDocument xml = new XmlDocument();
                        string mediaDe = string.Empty;
                        string mediaAte = string.Empty;

                        xml.LoadXml("<ResultadoPesquisaSalarial>" + objPesquisaSalarial.Xml_Resultado_Pesquisa + "</ResultadoPesquisaSalarial>");

                        using (StringReader reader = new StringReader(xml.InnerXml))
                        {
                            XmlSerializer deserializer = new XmlSerializer(typeof(ResultadoPesquisaSalarial));
                            object obj = deserializer.Deserialize(reader);
                            ResultadoPesquisaSalarial XmlData = (ResultadoPesquisaSalarial)obj;

                            mediaDe = XmlData.pesquisaSalarialList.FirstOrDefault(x => x.Idf_Tipo_Porte == 1 && x.Idf_Nivel_Experiencia == 1).Media.ToString("C");
                            mediaAte = XmlData.pesquisaSalarialList.FirstOrDefault(x => x.Idf_Tipo_Porte == 3 && x.Idf_Nivel_Experiencia == 5).Media.ToString("C");

                            reader.Close();
                        }

                        if (mediaDe != "R$ 0,00" && mediaAte != "R$ 0,00") 
                        {
                            retorno = string.Format("{0} em {1} ganha entre {2} e {3}.", funcao, cidade, mediaDe, mediaAte);
                        }
                    }
                }

                return retorno;
            }
            catch (Exception ex)
            {
                string msgErro;
                Business.EL.GerenciadorException.GravarExcecao(ex, out msgErro, "Lan House API - Carregar a média salarial");
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
