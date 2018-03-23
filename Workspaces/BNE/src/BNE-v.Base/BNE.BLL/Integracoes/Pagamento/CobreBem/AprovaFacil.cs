using System.IO;
using System.Net;
using System.Text;

namespace BNE.BLL.Integracoes.Pagamento.CobreBem
{
    public class AprovaFacil
    {

        #region Requisitar
        public static string Requisitar(string dadosRequisicao)
        {
            var url = Parametro.RecuperaValorParametro(Enumeradores.Parametro.URLWebserviceIntegracaoCobreBem);

            return Custom.Helper.EfetuarRequisicao(url, dadosRequisicao);
        }
        #endregion

        #region Capturar
        public static string Capturar(string transacao)
        {
            var url = Parametro.RecuperaValorParametro(Enumeradores.Parametro.URLWebserviceCapturaCobreBem);

            byte[] buffer = Encoding.ASCII.GetBytes("Transacao=" + transacao);

            var webRequest = WebRequest.Create(url);
            webRequest.Method = "POST";
            webRequest.ContentLength = buffer.Length;

            var postData = webRequest.GetRequestStream();
            postData.Write(buffer, 0, buffer.Length);
            postData.Close();

            var webResponse = webRequest.GetResponse();

            string retorno;
            using (var sr = new StreamReader(webResponse.GetResponseStream()))
            {
                retorno = sr.ReadToEnd();
            }

            return retorno;
        }
        #endregion

    }
}
