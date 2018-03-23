using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

namespace BNE.BLL.Custom
{
     class CampanhaTanque
    {
        public CampanhaTanque()
        { 
            if (client == null)
            {
                ServicePointManager.DefaultConnectionLimit = 100;
                ServicePointManager.Expect100Continue = false;

                HttpClientHandler httpClientHandler = new HttpClientHandler()
                {
                    UseProxy = false,
                    UseDefaultCredentials = false
                };

                client = new HttpClient(httpClientHandler) { Timeout = TimeSpan.FromMinutes(1) };
                client.BaseAddress = new Uri(UrlApiCampanhaTanque);
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            }
        }

        #region Parametros
        private static readonly string UrlApiCampanhaTanque = ConfigurationManager.AppSettings["ApiCampanhaTanque"].ToString();
        HttpClient client;
        Uri campanhaUri;
        #endregion

        #region GetTextoCampanha
        /// <summary>
        /// Retorna o template de texto SMS randomico de determinada campanha. As campanhas são cadastradas na tabela tanque.TAB_Campanha_Tanque_Mensagens
        /// </summary>
        /// <param name="campanha"></param>
        /// <returns></returns>
        private CampanhaTanqueMensagemDTO GetTextoCampanha(Enumeradores.CampanhaTanque campanha)
        {
            try
            {
                System.Net.Http.HttpResponseMessage response = client.GetAsync("api/campanha/GetObjMsgCampanha/" + (int)campanha).Result;

                //se retornar com sucesso busca os dados
                if (response.IsSuccessStatusCode)
                {
                    //pegando o cabeçalho
                    campanhaUri = response.Headers.Location;

                    //Pegando os dados do Rest e armazenando na variável retorno
                    var retorno = response.Content.ReadAsStringAsync().Result;

                    dynamic mensagem = JsonConvert.DeserializeObject(retorno);
                    CampanhaTanqueMensagemDTO objMensagem = new CampanhaTanqueMensagemDTO();
                    objMensagem.id = (int)mensagem.id.Value;
                    objMensagem.mensagem = mensagem.mensagem.Value;

                    //preenchendo a lista com os dados retornados da variável
                    return objMensagem;
                }

                //Se der erro na chamada, mostra o status do código de erro.
                return null;
            }
            catch(Exception ex) 
            {
                EL.GerenciadorException.GravarExcecao(ex);
                return null;
            }
        }
        #endregion

    }

    internal class CampanhaTanqueMensagemDTO
    {
        public int id { get; set; }
        public string mensagem { get; set; }
    }
}
