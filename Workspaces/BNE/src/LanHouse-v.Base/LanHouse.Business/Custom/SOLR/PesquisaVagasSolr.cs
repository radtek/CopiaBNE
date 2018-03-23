using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace LanHouse.Business.Custom.SOLR
{
    public class PesquisaVagasSolr
    {
        #region BuscarVagasLanHouse
        /// <summary>
        /// Obter Vagas para a Lan House
        /// </summary>
        /// <param name="idFuncao"></param>
        /// <param name="filtro"></param>
        /// <param name="start"></param>
        /// <param name="rows"></param>
        /// <param name="geolocalizacao"></param>
        /// <returns></returns>
        public static List<DTO.Vaga> ObterRegistros(string idFuncao, string filtro, int start, int rows,string geolocalizacao)
        {
            #region Efetuando Busca no SOLR
            int vagasPorPagina = rows;
            String urlSLOR = new Business.Parametro().GetById(Convert.ToInt32(Business.Enumeradores.Parametro.UrlVagasSolr)).Vlr_Parametro;
            urlSLOR += "?wt=json"; //retorno JSON
            urlSLOR += "&start=" + start.ToString(); //Primeiro registro

            if (idFuncao == "0")
            {
                if (!string.IsNullOrEmpty(geolocalizacao))
                    urlSLOR += "&fq={!geofilt}";
            }
            else if (Char.IsDigit(idFuncao, 0))
                urlSLOR += "&fq=Idfs_Funcoes_Sinonimo:" + idFuncao;
            else
                filtro = idFuncao; //passa a descrição da função no filtro

            if (!string.IsNullOrEmpty(geolocalizacao))
            {
                urlSLOR += "&sfield=Geo_Localizacao";
                urlSLOR += "&pt=";
                urlSLOR += geolocalizacao;
                urlSLOR += "&d=10000";
                urlSLOR += "&sort=geodist()+asc,score desc, Dta_Abertura desc";
            }
            else
            {
                urlSLOR += "&sort=score desc, Dta_Abertura desc";
            }

            urlSLOR += "&rows=" + vagasPorPagina.ToString(); //Número de registros retornados
            urlSLOR += "&spellcheck=true&spellcheck.onlyMorePopular=true"; //Configuração de Spellchecker
            urlSLOR += "&q=" + HttpContext.Current.Server.UrlEncode(filtro).Replace(" ", "+");

            WebRequest request = WebRequest.Create(urlSLOR);
            // Get the response.
            WebResponse response = request.GetResponse();
            // Get the stream containing content returned by the server.
            Stream dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);

            ResultadoBuscaVagasSolr resultado = (ResultadoBuscaVagasSolr)new JsonSerializer().Deserialize(reader, typeof(ResultadoBuscaVagasSolr));
            #endregion Efetuando Busca no SOLR

            #region Tratando retorno do SOLR
            List<DTO.Vaga> vagas = new List<DTO.Vaga>();
            DTO.Vaga vaga;

            if (resultado.response.docs.Count > 0)
            {
                //Convertendo suggestions
                foreach (Doc document in resultado.response.docs)
                {
                    vaga = new DTO.Vaga();
                    vaga.id = document.id;
                    vaga.dataCadastro = document.Dta_Cadastro;
                    vaga.dataAbertura = document.Dta_Abertura;
                    vaga.atribuicoes = document.Des_Atribuicoes;
                    vaga.requisitos = document.Des_Requisito;
                    vaga.beneficios = document.Des_Beneficio;
                    vaga.salario = FormatarSalario(document.Vlr_Salario_De, document.Vlr_Salario_Para);
                    vaga.codigo = document.Cod_Vaga;
                    vaga.funcao = document.Des_Funcao;
                    vaga.cidade = document.Nme_Cidade + "-" + document.Sig_Estado;
                    vaga.empresa = document.Raz_Social;
                    vaga.bneRecomenda = document.Flg_BNE_Recomenda;
                    vaga.spellChecker = resultado.spellcheck != null? TratarSpellCheck(resultado.spellcheck.suggestions) : null;

                    vagas.Add(vaga);
                }
            }
            else
            {
                vaga = new DTO.Vaga();
                
                vaga.spellChecker = TratarSpellCheck(resultado.spellcheck.suggestions);

                vagas.Add(vaga);
            };
            #endregion Tratando retorno do SOLR

            return vagas;
        }
        #endregion

        #region BuscarVagasAzulzinho
        /// <summary>
        /// Obter vagas para Jornal Azulzinho
        /// </summary>
        /// <param name="filtro"></param>
        /// <param name="start"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        public static List<DTO.Vaga> ObterVagasJornalAzulzinho(string filtro, int start, int rows, string idFuncao)
        {
            #region Efetuando Busca no SOLR
            int vagasPorPagina = rows;
            String urlSLOR = new Business.Parametro().GetById(Convert.ToInt32(Business.Enumeradores.Parametro.UrlVagasSolr)).Vlr_Parametro;
            urlSLOR += "?wt=json"; //retorno JSON
            urlSLOR += "&start=" + start.ToString(); //Primeiro registro

            string q = string.Empty;

            if (idFuncao == "0")
            {
                q = "&q=" + HttpContext.Current.Server.UrlEncode(filtro).Replace(" ", "+");
            }
            else if (Char.IsDigit(idFuncao, 0))
                urlSLOR += "&fq=Idfs_Funcoes_Sinonimo:" + idFuncao;
            else
                filtro = idFuncao; //passa a descrição da função no filtro

            urlSLOR += "&sort=score desc, Dta_Abertura desc";

            urlSLOR += "&rows=" + vagasPorPagina.ToString(); //Número de registros retornados
            urlSLOR += "&spellcheck=true&spellcheck.onlyMorePopular=true"; //Configuração de Spellchecker
            urlSLOR += q;
            //urlSLOR += "&q=" + HttpContext.Current.Server.UrlEncode(filtro).Replace(" ", "+");


            WebRequest request = WebRequest.Create(urlSLOR);
            // Get the response.
            WebResponse response = request.GetResponse();
            // Get the stream containing content returned by the server.
            Stream dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);

            ResultadoBuscaVagasSolr resultado = (ResultadoBuscaVagasSolr)new JsonSerializer().Deserialize(reader, typeof(ResultadoBuscaVagasSolr));
            #endregion Efetuando Busca no SOLR

            #region Tratando retorno do SOLR
            List<DTO.Vaga> vagas = new List<DTO.Vaga>();
            DTO.Vaga vaga;

            if (resultado.response.docs.Count > 0)
            {
                //Convertendo suggestions
                foreach (Doc document in resultado.response.docs)
                {
                    vaga = new DTO.Vaga();
                    vaga.id = document.id;
                    vaga.dataCadastro = document.Dta_Cadastro;
                    vaga.dataAbertura = document.Dta_Abertura;
                    vaga.atribuicoes = document.Des_Atribuicoes;
                    vaga.requisitos = document.Des_Requisito;
                    vaga.beneficios = document.Des_Beneficio;
                    vaga.salario = FormatarSalario(document.Vlr_Salario_De, document.Vlr_Salario_Para);
                    vaga.codigo = document.Cod_Vaga;
                    vaga.funcao = document.Des_Funcao;
                    vaga.cidade = document.Nme_Cidade + "-" + document.Sig_Estado;
                    vaga.empresa = document.Raz_Social;
                    vaga.bneRecomenda = document.Flg_BNE_Recomenda;
                    vaga.spellChecker = resultado.spellcheck != null ? TratarSpellCheck(resultado.spellcheck.suggestions) : null;

                    vagas.Add(vaga);
                }
            }
            else
            {
                vaga = new DTO.Vaga();

                vaga.spellChecker = TratarSpellCheck(resultado.spellcheck.suggestions);

                vagas.Add(vaga);
            };
            #endregion Tratando retorno do SOLR

            return vagas;
        }
        #endregion

        #region TratarSpellCheck
        /// <summary>
        /// Trata o resultado do spellCheck
        /// </summary>
        /// <param name="resultadoSpell"></param>
        /// <returns></returns>
        public static IList<DTO.SpellCheck> TratarSpellCheck(List<object> resultadoSpell)
        {
            IList<DTO.SpellCheck> listaSpell = new List<DTO.SpellCheck>();
            DTO.SpellCheck objSpell = null;

            var suggests = resultadoSpell.OfType<Newtonsoft.Json.Linq.JArray>();

            foreach (var t in suggests)
            {
                objSpell = new DTO.SpellCheck();

                var type = t.AsQueryable().OfType<Newtonsoft.Json.Linq.JValue>().Cast<Newtonsoft.Json.Linq.JValue>().ToList();

                objSpell.word = type[1].ToString().TrimEnd();
                objSpell.hit = (int)type[3];

                if(listaSpell.Where(i=>i.word == objSpell.word).SingleOrDefault() == null)
                    listaSpell.Add(objSpell);
            }

            listaSpell = listaSpell.Where(i => i.hit >= 1).ToList();

            return listaSpell;
        }
        #endregion

        #region FormatarSalario
        /// <summary>
        /// Formata o Retorno do salario na vaga
        /// </summary>
        /// <param name="salarioDe"></param>
        /// <param name="salarioPara"></param>
        /// <returns></returns>
        public static string FormatarSalario(decimal salarioDe, decimal salarioPara)
        {
            var retorno = "A combinar";
            if(salarioDe > 0 && salarioPara > 0)
            {
                retorno = string.Format("De {0} até {1}", salarioDe.ToString("C"), salarioPara.ToString("C"));
            }
            else if (salarioDe > 0 && salarioPara ==0)
            {
                retorno = string.Format("De {0}", salarioDe.ToString("C"));
            }
            else if (salarioPara > 0)
            {
                retorno = string.Format("Até {0}", salarioPara.ToString("C"));
            }
            
            return retorno;
        }
        #endregion


        #region FiltrarEstagiarios
        public static List<DTO.Vaga> FiltrarEstagiarios(string filtro, int start, int rows)
        {
            int vagasPorPagina = rows;
            String SolrUrl = new Business.Parametro().GetById(Convert.ToInt32(Business.Enumeradores.Parametro.UrlVagasSolr)).Vlr_Parametro;
            SolrUrl += "?wt=json"; //retorno JSON
            SolrUrl += "&start=" + start.ToString(); //Primeiro registro
            SolrUrl += "&fq=Idf_Tipo_Vinculo:4";

            SolrUrl += "&rows=" + vagasPorPagina.ToString(); //Número de registros retornados
            SolrUrl += "&spellcheck=true&spellcheck.onlyMorePopular=true"; //Configuração de Spellchecker
            SolrUrl += "&q=" + HttpContext.Current.Server.UrlEncode(filtro).Replace(" ", "+");

            WebRequest request = WebRequest.Create(SolrUrl);
            // Get the response
            WebResponse response = request.GetResponse();
            // Get the stream containing content returned by the server.
            Stream dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);

            ResultadoBuscaVagasSolr resultado = (ResultadoBuscaVagasSolr)new JsonSerializer().Deserialize(reader, typeof(ResultadoBuscaVagasSolr));
         

            List<DTO.Vaga> vagas = new List<DTO.Vaga>();
            DTO.Vaga vaga;

            if (resultado.response.docs.Count > 0)
            {
                //Convertendo suggestions
                foreach (Doc document in resultado.response.docs)
                {
                    vaga = new DTO.Vaga();
                    vaga.id = document.id;
                    vaga.dataCadastro = document.Dta_Cadastro;
                    vaga.dataAbertura = document.Dta_Abertura;
                    vaga.atribuicoes = (document.Des_Atribuicoes != null) ? document.Des_Atribuicoes : "" ;
                    vaga.requisitos = document.Des_Requisito;
                    vaga.beneficios = document.Des_Beneficio;
                    vaga.salario = FormatarSalario(document.Vlr_Salario_De, document.Vlr_Salario_Para);
                    vaga.codigo = document.Cod_Vaga;
                    vaga.funcao = document.Des_Funcao;
                    vaga.cidade = document.Nme_Cidade + "-" + document.Sig_Estado;
                    vaga.empresa = document.Raz_Social;
                    vaga.bneRecomenda = document.Flg_BNE_Recomenda;
                    vaga.spellChecker = resultado.spellcheck != null ? TratarSpellCheck(resultado.spellcheck.suggestions) : null;

                    vagas.Add(vaga);
                }
            }
            #endregion Tratando retorno do SOLR

            return vagas;
        }

    }
}
