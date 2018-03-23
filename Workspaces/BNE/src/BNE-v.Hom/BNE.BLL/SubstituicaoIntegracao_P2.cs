//-- Data: 15/05/2013 15:50
//-- Autor: Gieyson Stelmak

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
namespace BNE.BLL
{
	public partial class SubstituicaoIntegracao // Tabela: TAB_Substituicao_Integracao
    {
        #region Propriedades
        public Regex Regex { get; set; }
        #endregion

        #region Consultas

        //Select para selecionar as substituicoes para o integrador
        private const string SP_SELECT_LISTA = @"
            SELECT	SI.*
            FROM BNE.TAB_Substituicao_Integracao SI WITH(NOLOCK)
            LEFT JOIN BNE.TAB_Regra_Substituicao_Integracao RSI WITH(NOLOCK) 
                    ON SI.Idf_Regra_Substituicao_Integracao = RSI.Idf_Regra_Substituicao_Integracao
            WHERE	RSI.Idf_Regra_Substituicao_Integracao IS NULL OR
		            RSI.Idf_Integrador IS NULL OR
		            RSI.Idf_Integrador = @Idf_Integrador";

        #endregion 

        #region ListarPalavrasProibidas
        /// <summary>
        /// Método responsável por retornar uma List com todas as substituicoes para determinada integracao
        /// </summary>
        /// <returns></returns>
        public static List<SubstituicaoIntegracao> ListarSubstituicoesDeIntegrador(Integrador objIntegrador)
        {
            List<SubstituicaoIntegracao> lstSubstituicoesDeIntegrador = new List<SubstituicaoIntegracao>();

            var parms = new List<SqlParameter>
            {
                new SqlParameter { ParameterName = "@Idf_Integrador", SqlDbType = SqlDbType.Int, Size = 4, Value = objIntegrador.IdIntegrador }
            };

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SP_SELECT_LISTA, parms))
            {
                String RegexIndexacaoVaga = Parametro.RecuperaValorParametro(Enumeradores.Parametro.RegexIndexacaoVaga);

                while (dr.Read()){
                    SubstituicaoIntegracao objSubstituicaoIntegracao = new SubstituicaoIntegracao();
                    SetInstanceNotDispose(dr, objSubstituicaoIntegracao);
                    if (objSubstituicaoIntegracao.RegraSubstituicaoIntegracao != null)
                    {
                        objSubstituicaoIntegracao.RegraSubstituicaoIntegracao.CompleteObject();
                    }
                    objSubstituicaoIntegracao.Regex = new Regex(RegexIndexacaoVaga.Replace("{palavra}", objSubstituicaoIntegracao.DescricaoAntiga), RegexOptions.IgnoreCase);
                    lstSubstituicoesDeIntegrador.Add(objSubstituicaoIntegracao);
                }

                if (!dr.IsClosed)
                    dr.Close();
            }

            return lstSubstituicoesDeIntegrador;
        }
        #endregion

        #region SetInstanceNotDispose
        /// <summary>
        /// Método auxiliar utilizado pelos métodos LoadObject e CompleteObject para percorrer um IDataReader e vincular as colunas com os atributos da classe.
        /// </summary>
        /// <param name="dr">Cursor de leitura do banco de dados.</param>
        /// <param name="objSubstituicaoIntegracao">Instância a ser manipulada.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static bool SetInstanceNotDispose(IDataReader dr, SubstituicaoIntegracao objSubstituicaoIntegracao)
        {
            try
            {
                objSubstituicaoIntegracao._idSubstituicaoIntegracao = Convert.ToInt32(dr["Idf_Substituicao_Integracao"]);
                objSubstituicaoIntegracao._descricaoAntiga = Convert.ToString(dr["Des_Antiga"]);
                objSubstituicaoIntegracao._descricaoNova = Convert.ToString(dr["Des_Nova"]);
                if (dr["Idf_Regra_Substituicao_Integracao"] != DBNull.Value)
                    objSubstituicaoIntegracao._regraSubstituicaoIntegracao = new RegraSubstituicaoIntegracao(Convert.ToInt32(dr["Idf_Regra_Substituicao_Integracao"]));

                objSubstituicaoIntegracao._persisted = true;
                objSubstituicaoIntegracao._modified = false;

                return true;
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region AplicarSubstituicao
        public String AplicarSubstituicao(String descricao)
        {
            return this.Regex.Replace(descricao, this.DescricaoNova);
        }
        #endregion
    }
}