using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace BNE.BLL.Custom
{
    public class ApiSuggestResult
    {
        public List<ApiSuggestFuncoesResult> funcoes { get; set; }

        public static int? getFuncaoSinonimo(string fun)
        {
            int? result = null;

            string ApiUrl = BNE.BLL.Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.UrlAPISalarioBR);
            if (!ApiUrl.Contains("http"))
                ApiUrl = String.Concat("http:", ApiUrl);
            string url = "{0}/Funcoes/RetornarFuncaoBNE?funcao={1}";

            WebRequest request = WebRequest.Create(String.Format(url, ApiUrl, fun));

            request.Proxy = WebRequest.DefaultWebProxy;
            request.Proxy.Credentials = CredentialCache.DefaultCredentials;

            request.ContentType = "application/json;charset=UTF-8";
            request.Method = "GET";

            try
            {
                HttpWebResponse webResponse = (HttpWebResponse)request.GetResponse();

                if (webResponse.StatusCode == HttpStatusCode.OK)
                {
                    Stream dataStream = webResponse.GetResponseStream();
                    StreamReader reader = new StreamReader(dataStream);

                    ApiSuggestResult apiSuggestResult = new JavaScriptSerializer().Deserialize<ApiSuggestResult>(reader.ReadToEnd());

                    if (apiSuggestResult.funcoes != null)
                    {
                        if (apiSuggestResult.funcoes.Count > 0)
                            result = apiSuggestResult.funcoes[0].id;
                    }

                    reader.Close();
                    dataStream.Close();
                    webResponse.Close();
                }
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, "Falha na busca de função na api do salario br");
            }

            return result;
        }
    }

    public class ApiSuggestFuncoesResult
    {
        public int id { get; set; }
        public string descricao { get; set; }
    }

}
