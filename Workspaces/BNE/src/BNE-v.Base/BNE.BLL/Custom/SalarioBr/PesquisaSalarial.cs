using System;
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace BNE.BLL.Custom.SalarioBr
{
    public class PesquisaSalarial
    {

        public class ResultadoPesquisa
        {
            public decimal t;
            public decimal ValorTrainee { get { return t; } }
            public decimal m;
            public decimal ValorMaster { get { return m; } }
        }

        #region EfetuarPesquisa
        public static ResultadoPesquisa EfetuarPesquisa(Funcao objFuncao, Estado objEstado)
        {
            try
            {
                var salarioBrWSURL = Parametro.RecuperaValorParametro(Enumeradores.Parametro.SalarioBRWSURL);
                var salarioBrNomeMetodo = Parametro.RecuperaValorParametro(Enumeradores.Parametro.SalarioBRPesquisaSalarialNomeMetodo);
                var formattedURL = string.Format("{0}/{1}?f={2}&u={3}", salarioBrWSURL, salarioBrNomeMetodo, objFuncao.IdFuncao, objEstado.SiglaEstado);
                
                var webRequest = HttpWebRequest.Create(formattedURL);

                webRequest.ContentLength = 0;
                webRequest.Method = "post";

                webRequest.UseDefaultCredentials = false;
                webRequest.Credentials = new NetworkCredential { UserName = @"wsSalarioBR", Password = @"0pO32ikem3428s&8392%3#2821" };

                var webResponse = webRequest.GetResponse();
                var sr = new StreamReader(webResponse.GetResponseStream());
                try
                {
                    string retorno = sr.ReadToEnd();

                    return JsonConvert.DeserializeObject<ResultadoPesquisa>(retorno);
                }
                finally
                {
                    sr.Close();
                }
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
            }

            return null;
        }
        #endregion

    }
}
