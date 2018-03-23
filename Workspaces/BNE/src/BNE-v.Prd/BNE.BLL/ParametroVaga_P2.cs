//-- Data: 18/02/2016 17:07
//-- Autor: Mailson

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Caching;
using System.Text;
namespace BNE.BLL
{
	public partial class ParametroVaga // Tabela: BNE_Parametro_Vaga
	{

        #region Consultas

        #region spDeleteParametro
        private const string spDeleteParametro = @"SET NOCOUNT ON
                            SELECT pv.Idf_Vaga, Idf_Parametro
                            INTO #temp
                            FROM  BNE.BNE_Parametro_Vaga pv WITH (NOLOCK)
                                JOIN BNE_Vaga v ON v.Idf_Vaga = pv.Idf_Vaga
                            WHERE  v.Idf_Filial = @Idf_Filial
                                AND pv.Idf_Parametro = @Idf_Parametro

                            DELETE  BNE_Parametro_Vaga
                            FROM  BNE.BNE_Parametro_Vaga pv 
                                JOIN #temp t ON pv.Idf_Parametro = t.Idf_Parametro AND pv.Idf_Vaga = t.Idf_Vaga
                            WHERE  pv.Idf_Parametro = @Idf_Parametro

                            SELECT Idf_Vaga FROM #temp

                            DROP TABLE #temp";
        #endregion

        #region spFilialTransacao
        private const string spFilialTransacao = @"select top 40 pa.Idf_Filial from  BNE_Linha_Arquivo la
            join BNE_Transacao t on t.Idf_Transacao = la.Idf_Transacao
            join BNE_Plano_Adquirido pa on pa.Idf_Plano_Adquirido = t.Idf_Plano_Adquirido
             where pa.Idf_Filial is not null and t.idf_transacao = @Idf_Transacao ";
	    #endregion

        #endregion

        #region Métodos

        #region ApagarParametroVaga
        /// <summary>
        /// Apagar lista de parametro vaga
        /// </summary>
        /// <param name="ListIdVaga"></param>
        /// <param name="idParametro"></param>
        public static void ApagarParametroVaga(Dictionary<int, DateTime> ListIdVaga, int idFilial, Enumeradores.Parametro idParametro)
        {
            try
            {
                var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Parametro", SqlDbType = SqlDbType.Int, Size = 4, Value = (int)idParametro},
                    new SqlParameter { ParameterName = "@Idf_Filial", SqlDbType= SqlDbType.Int,Size= 4, Value = idFilial}
                };
                //DELETAR OS PARAMETROS
                DataAccessLayer.ExecuteNonQuery(CommandType.Text, spDeleteParametro, parms);

                BLL.Custom.Solr.Buffer.BufferAtualizacaoVagaFilial.UpdateVagaFilial(idFilial);

            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, "Erro - Apagara parametro vaga");
            }

        }
        #endregion
       
        #endregion
	}
}