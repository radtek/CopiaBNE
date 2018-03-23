using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Globalization;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace BNE.BLL.Integracoes.Pagamento.DebitoOnline
{
    public class HSBC
    {
        public static string CriarTransacao(string CodigoLoja, string CodigoPedidoLoja, decimal Valor, out String erro){
            
            try
            {
                //You must change the path to point to your .cer file location. 
                X509Certificate Cert = X509Certificate.CreateFromCertFile("C:\\bne.com.br.cer");
                // Handle any certificate errors on the certificate from the server.
                ServicePointManager.CertificatePolicy = new CertPolicy();

                erro = String.Empty;

                var url = Parametro.RecuperaValorParametro(Enumeradores.Parametro.HSBCUrlCriacaoPedido);

                byte[] buffer = Encoding.UTF8.GetBytes(String.Format("codigoLoja={0}&codigoPedidoLoja={1}&valorPedido={2}", CodigoLoja.ToString(), CodigoPedidoLoja.ToString(), Valor.ToString("F2", CultureInfo.InvariantCulture)));

                var webRequest = (HttpWebRequest)WebRequest.Create(url);
                webRequest.ClientCertificates.Add(Cert);
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
            catch (Exception ex)
            {
                erro = ex.Message;
            }
            
            return "";
        }

        public static Enumeradores.PagamentoSituacao VerificarSituacao(string CodigoLoja, string CodigoPedidoLoja, decimal Valor, out String erro)
        {
            erro = String.Empty;
            String url = Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.HSBCUrlConsultaStatusPedido);

            byte[] buffer = Encoding.UTF8.GetBytes(String.Format("codigoLoja={0}&codigoPedidoLoja={1}&valorPedido={2}", CodigoLoja, CodigoPedidoLoja, Valor.ToString("F2", CultureInfo.InvariantCulture)));

            var webRequest = WebRequest.Create(url);
            webRequest.Method = "POST";
            webRequest.ContentLength = buffer.Length;

            var postData = webRequest.GetRequestStream();
            postData.Write(buffer, 0, buffer.Length);
            postData.Close();

            var webResponse = webRequest.GetResponse();

            string retornoHSBC;
            using (var sr = new StreamReader(webResponse.GetResponseStream()))
            {
                retornoHSBC = sr.ReadToEnd();
            }

            Enumeradores.PagamentoSituacao retorno = Enumeradores.PagamentoSituacao.EmAberto;
            switch (retornoHSBC)
            {
                case "0":
                    erro = "Pedido inexistente no HSBC!";
                    break;
                case "1":
                case "2":
                case "3":
                    erro = "Pedido não foi pago com sucesso!";
                    break;
                case "4":
                    //pagamento efetuado
                    retorno = Enumeradores.PagamentoSituacao.Pago;
                    break;
                case "5":
                    erro = "Pagamento recusado pelo HSBC!";
                    break;
                default:
                    break;
            }

            return retorno;
        }

        //Implement the ICertificatePolicy interface.
        class CertPolicy : ICertificatePolicy
        {
            public bool CheckValidationResult(ServicePoint srvPoint,
        X509Certificate certificate, WebRequest request, int certificateProblem)
            {
                // You can do your own certificate checking.
                // You can obtain the error values from WinError.h.

                // Return true so that any certificate will work with this sample.
                return true;
            }
        }
    }
}
