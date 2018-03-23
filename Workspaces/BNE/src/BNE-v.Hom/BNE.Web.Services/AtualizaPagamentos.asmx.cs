using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Services;
using BNE.BLL;
using BNE.BLL.Enumeradores;
using System.Web.Services.Protocols;
using security = BNE.BLL.Security;


namespace BNE.Web.Services
{
    /// <summary>
    /// WS responsável por atualizar as informações de pagamentos.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class AtualizaPagamentos : System.Web.Services.WebService
    {
        public security.ServiceAuthHeader CustomSoapHeader { get; set; }

        /// <summary>
        /// Método responsavel por atualizar informações relacionadas aos guids dos boletos pagos recebidos como parâmetro.
        /// </summary>
        /// <param name="pListGuids"></param>
        /// <returns></returns>
        [WebMethod]
        [SoapHeader("CustomSoapHeader")]
        public bool AtualizaInformacoesBoletosPagos(List<string> pListGuids)
        {
            try
            {
                #region Segurança - Autorização de acesso
                security.ServiceAuth.AcessoAutorizado(this);
                #endregion

                return Pagamento.AtualizaInformacoesPagamentoBoleto(pListGuids);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
