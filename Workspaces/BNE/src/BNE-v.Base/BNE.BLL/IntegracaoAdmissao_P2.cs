//-- Data: 20/02/2014 15:09
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;

namespace BNE.BLL
{
    public partial class IntegracaoAdmissao // Tabela: plataforma.BNE_Integracao_Admissao
    {

        #region Sprecuperarintegracoes
        private const string Sprecuperarintegracoes = @"
        SELECT * FROM plataforma.BNE_Integracao_Admissao WHERE Dta_Integracao IS NULL AND ( Idf_Integracao_Situacao = 1 OR Idf_Integracao_Situacao = 3 )
        ";
        #endregion

        #region RecuperarIntegracoes
        /// <summary>
        /// Recupera as integracoes não integradas
        /// </summary>
        /// <returns>Uma datatable com os emails não enviados</returns>
        public static List<IntegracaoAdmissao> RecuperarIntegracoes()
        {
            var lista = new List<IntegracaoAdmissao>();
            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Sprecuperarintegracoes, null))
            {
                while (dr.Read())
                {
                    var objIntegracao = new IntegracaoAdmissao();

                    if (SetInstance_NonDispose(dr, objIntegracao))
                        lista.Add(objIntegracao);
                }
            }
            return lista;
        }
        #endregion

        #region SetInstance_NonDispose
        /// <summary>
        /// Método auxiliar utilizado pelos métodos LoadObject e CompleteObject para percorrer um IDataReader e vincular as colunas com os atributos da classe.
        /// </summary>
        /// <param name="dr">Cursor de leitura do banco de dados.</param>
        /// <param name="objIntegracaoAdmissao">Instância a ser manipulada.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static bool SetInstance_NonDispose(IDataReader dr, IntegracaoAdmissao objIntegracaoAdmissao)
        {
            objIntegracaoAdmissao._idIntegracaoAdmissao = Convert.ToInt32(dr["Idf_Integracao_Admissao"]);
            objIntegracaoAdmissao._numeroCPF = Convert.ToDecimal(dr["Num_CPF"]);
            objIntegracaoAdmissao._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
            if (dr["Dta_Integracao"] != DBNull.Value)
                objIntegracaoAdmissao._dataIntegracao = Convert.ToDateTime(dr["Dta_Integracao"]);
            objIntegracaoAdmissao._integracaoSituacao = new IntegracaoSituacao(Convert.ToInt32(dr["Idf_Integracao_Situacao"]));

            objIntegracaoAdmissao._persisted = true;
            objIntegracaoAdmissao._modified = false;

            return true;
        }

        #endregion

    }
}