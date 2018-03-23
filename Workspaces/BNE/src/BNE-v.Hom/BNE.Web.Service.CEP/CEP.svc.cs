/*  Autor: Gustavo Marty Sroka
 *  Data: 13/04/2016
 *  Classe copiada do web service de CEP da webfopag, com pequenos ajustes no código
 */

using BNE.CEP;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;

namespace BNE.Web.Service.CEP
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "CEP" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select CEP.svc or CEP.svc.cs at the Solution Explorer and start debugging.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class CEP : ICEP
    {
        public void CompletarCEP(ref Cep cep)
        {
            Cep.CompletarCEP(ref cep);
        }

        public int RecuperarQuantidadeEnderecosPorCEP(Cep cep)
        {
            return Cep.ContaConsulta(cep);
        }

        public IList<Cep> ConsultaCEP(string cep, string logradouro, string tipoLogradouro, string bairro, string estado, string cidade, string ordenacao)
        {
            return Cep.ConsultaCEPDS(cep, logradouro, tipoLogradouro, bairro, estado, cidade, ordenacao);
        }

        public IList<Cep> ConsultaCEPPaginacao(
                string cep, string logradouro, string tipoLogradouro, string bairro,
                string estado, string cidade, Colunas nomeColuna,
                DirecaoOrdenacao direcaoOrdenacao, int paginaCorrente,
                int tamanhoPagina, out int totalRegistros)
        {
            return Cep.ConsultaCEPPaginacao(cep, logradouro, tipoLogradouro, bairro, estado, cidade, nomeColuna, direcaoOrdenacao, paginaCorrente, tamanhoPagina, out totalRegistros);
        }

        /// <summary>
        /// Lista os cep de uma lista de CEPs.
        /// </summary>
        /// <param name="csvCEPs">Lista de CEPs separados por virgula - ','</param>
        /// <returns></returns>
        public IList<Cep> ListarCeps(string csvCEPs)
        {
            return Cep.ListarTipoLogradouro(csvCEPs);
        }

    }
}
