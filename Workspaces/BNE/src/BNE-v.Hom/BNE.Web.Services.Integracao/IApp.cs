using System;
using System.Data.SqlClient;
using System.IO;
using System.Data;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace BNE.Web.Services.Integracao
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IApp" in both code and config file together.
    [ServiceContract]
    public interface IApp
    {

        [OperationContract]
        [WebGet(
           UriTemplate = "json/DadosCurriculo?cnpj={numeroCNPJ}&senha={senhaUsuario}&cliente={nomeCliente}&curriculo={codigoCurriculo}",
           ResponseFormat = WebMessageFormat.Json)]
        Stream DadosCurriculoJSON(string numeroCNPJ, string senhaUsuario, string nomeCliente, int codigoCurriculo);

        [OperationContract]
        [WebGet(
           UriTemplate = "xml/DadosCurriculo?cnpj={numeroCNPJ}&senha={senhaUsuario}&cliente={nomeCliente}&curriculo={codigoCurriculo}", BodyStyle = WebMessageBodyStyle.WrappedResponse, 
           ResponseFormat = WebMessageFormat.Xml)]
        Stream DadosCurriculoXML(string numeroCNPJ, string senhaUsuario, string nomeCliente, int codigoCurriculo);

        [OperationContract]
        [WebInvoke(
            Method = "POST",
            UriTemplate = "/AtualizaNF",
            BodyStyle = WebMessageBodyStyle.WrappedResponse,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json
            )]
        DTO.OutAtualizaNumNF AtualizaNF(DTO.InAtualizaNumNF atualizaNF);

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [WebInvoke(Method = "POST", UriTemplate = "/IntegrarVagaSINE", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        bool IntegrarVagaSINE(BLL.DTO.wsIntegracao.InVaga oVaga);

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [WebInvoke(Method = "POST", UriTemplate = "/InativarVagaOrigemSINE", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        bool InativarVagaOrigemSINE(string codigo, string oportunidade);

        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        [WebInvoke(
            Method = "POST",
            UriTemplate = "/BloquearCurriculo",
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        bool BloquearCurriculo(decimal cpf, string motivo);
        
        
    }
}
