using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace BNE.Web.Services.Mobile
{
    [ServiceContract]
    public interface IApp
    {
        #region Operações

        #region Candidatar
        [WebInvoke(
                    Method = "POST",
                    BodyStyle = WebMessageBodyStyle.Bare,
                    RequestFormat = WebMessageFormat.Json,
                    ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        OutStatusDTO Candidatar(InCandidatarDTO candidatar);
        #endregion

        #region CandidatarTanque
        [WebInvoke(
                    Method = "POST",
                    BodyStyle = WebMessageBodyStyle.Bare,
                    RequestFormat = WebMessageFormat.Json,
                    ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        OutCandidaturaTanqueDTO CandidatarTanque(InCandidatarDTO candidatar);
        #endregion

        #region DadosUsuario
        [WebInvoke(
            Method = "POST",
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        OutDadosUsuarioDTO DadosUsuario(int c);
        #endregion

        #region PesquisaVagas
        [WebInvoke(
            Method = "POST",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        OutPesquisaVagasDTO PesquisaVagas(InPesquisaVagasDTO pesquisaVagas);
        #endregion

        #region DetalhesVaga
        [WebInvoke(
            Method = "POST",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        OutDetalhesVagaDTO DetalhesVaga(InDetalhesVagaDTO detalhes);
        #endregion

        #region PerguntasVaga
        [WebInvoke(
            Method = "POST",
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        OutPerguntasVagaDTO PerguntasVaga(int v);
        #endregion

        #region CadastroMiniCurriculo
        [WebInvoke(
            Method = "POST",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        OutCadastroMiniCurriculoDTO CadastroMiniCurriculo(InCadastroMiniCurriculoDTO cadastro);
        #endregion

        #region DetalhesEmpresa
        [WebInvoke(
            Method = "POST",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        OutDetalhesEmpresaDTO DetalhesEmpresa(InDetalhesEmpresaDTO detalhes);
        #endregion

        #region RegistraTokenGCM
        [WebInvoke(
            Method = "POST",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        OutStatusDTO RegistraTokenGCM(InTokenGCMDTO registra);
        #endregion

        #region RemoveTokenGCM
        [WebInvoke(
            Method = "POST",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        OutStatusDTO RemoveTokenGCM(InTokenGCMDTO remove);
        #endregion

        #region Login
        [WebInvoke(
            Method = "POST",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        OutLoginDTO Login(InLoginDTO login);
        #endregion

        #region TimeLine
        [WebInvoke(
            Method = "POST",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        [ServiceKnownType(typeof(TimelineJaEnvieiDTO))]
        [ServiceKnownType(typeof(TimelineMensagemDTO))]
        [ServiceKnownType(typeof(TimelineQuemMeViuDTO))]
        [ServiceKnownType(typeof(TimelineVagaDTO))]
        OutTimeLineDTO TimeLine(InTimeLineDTO timeline);
        #endregion

        #region RegistraTokenAPNS
        [WebInvoke(
            Method = "POST",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        OutStatusDTO RegistraTokenAPNS(InTokenAPNSDTO registra);
        #endregion

        #region RemoveTokenAPNS
        [WebInvoke(
            Method = "POST",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        OutStatusDTO RemoveTokenAPNS(InTokenAPNSDTO remove);
        #endregion

        #region LiberarVIP
        [WebInvoke(
            Method = "POST",
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        OutStatusDTO LiberarVIP(int c, int p, string pt, string oid);
        #endregion

        #region ValidaNotificacao
        [WebInvoke(
             Method = "POST",
             BodyStyle = WebMessageBodyStyle.Bare,
             RequestFormat = WebMessageFormat.Json,
             ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        OutStatusDTO ValidaNotificacao(InValidaNotificacaoDTO valida);
        #endregion

        #region Teste
        [WebInvoke(
            Method = "POST",
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        OutStatusDTO Teste();
        #endregion

        #endregion
    }
}
