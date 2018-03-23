using System.ServiceModel;
using System.ServiceModel.Web;
using BNE.Web.Services.Mobile.DTO.BNEEnvia;

namespace BNE.Web.Services.Mobile
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IBNEEnviaApp" in both code and config file together.
    [ServiceContract]
    public interface IBNEEnvia
    {

        #region ValidaIMEI
        [WebInvoke(
            Method = "POST",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        RetornoValidaIMEI ValidaIMEI(EntradaValidaIMEI validaIMEI);
        #endregion

        #region VerificaCampanhas
        [WebInvoke(
            Method = "POST",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        RetornoVerificaCampanhas VerificaCampanhas(EntradaVerificaCampanhas verificaCampanhas);
        #endregion

        #region ConfirmaCampanhaEnviada
        [WebInvoke(
            Method = "POST",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        RetornoConfirmaCampanhaEnviada ConfirmaCampanhaEnviada(EntradaConfirmaCampanhaEnviada confirmaCampanhaEnviada);
        #endregion

        #region ReceberSMSConversacao
        [WebInvoke(
            Method = "GET",
            UriTemplate = "ReceberSMSConversa?Mensagem={Mensagem}&NumeroCelular={NumeroCelular}&CodigoUsuario={CodigoUsuario}",
            ResponseFormat = WebMessageFormat.Json
            )]
        [OperationContract]
        void ReceberSMSConversacao(string Mensagem, decimal NumeroCelular, string CodigoUsuario);
        #endregion
    }

}
