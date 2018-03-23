using BNE.CEP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace BNE.Web.Service.CEP
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ICEP" in both code and config file together.
    [ServiceContract]
    [ServiceKnownType(typeof(BNE.CEP.DirecaoOrdenacao))]
    [ServiceKnownType(typeof(BNE.CEP.Colunas))]
    public interface ICEP
    {
        [OperationContract]
        void CompletarCEP(ref Cep cep);

        [OperationContract]
        int RecuperarQuantidadeEnderecosPorCEP(Cep cep);

        [OperationContract]
        IList<Cep> ConsultaCEP(string cep, string logradouro, string tipoLogradouro, string bairro, string estado, string cidade, string ordenacao);

        [OperationContract]
        IList<Cep> ConsultaCEPPaginacao(
                string cep, string logradouro, string tipoLogradouro, string bairro,
                string estado, string cidade, BNE.CEP.Colunas nomeColuna,
                BNE.CEP.DirecaoOrdenacao direcaoOrdenacao, int paginaCorrente,
                int tamanhoPagina, out int totalRegistros);

        [OperationContract]
        IList<Cep> ListarCeps(string csvCEPs);
    }
}
