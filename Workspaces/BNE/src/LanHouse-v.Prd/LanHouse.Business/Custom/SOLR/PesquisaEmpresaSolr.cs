using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace LanHouse.Business.Custom.SOLR
{
    public class PesquisaEmpresaSolr
    {
        public static List<DTO.Empresa> ObterRegistros(string idFuncao, string filtro, int start, int rows, string geolocalizacao)
        {
            #region Efetuando Busca no SOLR
            int registrosPorPagina = rows;
            String urlSLOR = new Business.Parametro().GetById(Convert.ToInt32(Business.Enumeradores.Parametro.UrlVagasSolr)).Vlr_Parametro;
            urlSLOR += "?wt=json"; //retorno JSON
            urlSLOR += "&start=" + start.ToString(); //Primeiro registro

            if (idFuncao == "0")
                urlSLOR += "&fq={!geofilt}";
            else if (Char.IsDigit(idFuncao, 0))
                urlSLOR += "&fq=Idfs_Funcoes_Sinonimo:" + idFuncao;
            else
                filtro = idFuncao; //passa a descrição da função no filtro

            urlSLOR += "&sfield=Geo_Localizacao";
            urlSLOR += "&pt=";
            urlSLOR += geolocalizacao;

            urlSLOR += "&d=10000";
            urlSLOR += "&sort=geodist()+asc,score desc, Dta_Abertura desc";

            urlSLOR += "&rows=" + registrosPorPagina.ToString(); //Número de registros retornados
            urlSLOR += "&spellcheck=true&spellcheck.onlyMorePopular=true"; //Configuração de Spellchecker
            urlSLOR += "&q=" + HttpContext.Current.Server.UrlEncode(filtro).Replace(" ", "+");

            WebRequest request = WebRequest.Create(urlSLOR);
            // Get the response.
            WebResponse response = request.GetResponse();
            // Get the stream containing content returned by the server.
            Stream dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);

            ResultadoBuscaEmpresaSolr resultado = (ResultadoBuscaEmpresaSolr)new JsonSerializer().Deserialize(reader, typeof(ResultadoBuscaEmpresaSolr));
            #endregion Efetuando Busca no SOLR

            #region Tratando retorno do SOLR
            List<DTO.Empresa> empresas = new List<DTO.Empresa>();
            DTO.Empresa empresa;

            //Convertendo suggestions
            foreach (DocEmpresa document in resultado.response.docs)
            {
                empresa = new DTO.Empresa();
                empresa.id = document.id;
                empresa.nomeFantasia = document.Nme_Fantasia;
                empresa.apresentacao = document.Des_Carta_Apresentacao;
                empresa.emailResponsavel = document.Eml_Comercial;

                empresas.Add(empresa);
            }
            #endregion Tratando retorno do SOLR

            return empresas;
        }
    }
}
