using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Web;

namespace BNE.Web.Services.Integracao
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ITanque" in both code and config file together.
    [ServiceContract]
    public interface ITanque
    {
        #region UsuarioTanque
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [WebInvoke(
                   Method = "POST",
                   BodyStyle = WebMessageBodyStyle.Bare,
                   RequestFormat = WebMessageFormat.Json,
                   ResponseFormat = WebMessageFormat.Json)]
        DTO.OutUsuarioTanque UsuarioTanque(DTO.InUsuarioTanque curriculo);
        #endregion

        #region LiberarUsuarioTanque
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [WebInvoke(
                    Method = "POST",
                    BodyStyle = WebMessageBodyStyle.Bare,
                    RequestFormat = WebMessageFormat.Json,
                    ResponseFormat = WebMessageFormat.Json)]

        DTO.OutLiberaUsuarioTanque liberaUsuarioTanque(DTO.InLiberaUsuarioTanque liberaUsuario, bool novoUsuario = true);
        #endregion

        #region RetornaCelularSelecionador
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [WebInvoke(
                   Method = "POST",
                   BodyStyle = WebMessageBodyStyle.Bare,
                   RequestFormat = WebMessageFormat.Json,
                   ResponseFormat = WebMessageFormat.Json)]
        DTO.CelularUsuarioDTO RetornaCelularSelecionador(int IdUsuarioFilialPerfil);
        #endregion
    }
}
