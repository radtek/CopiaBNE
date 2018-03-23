//-- Data: 08/02/2012 15:30
//-- Autor: Gieyson Stelmak

using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System;

namespace BNE.BLL
{
	public partial class PalavraChave // Tabela: BNE_Palavra_Chave
	{

        private const string Spselectdescricao = "SELECT * FROM BNE_Palavra_Chave WITH(NOLOCK) WHERE Des_Palavra_Chave = @Des_Palavra_Chave";

        private const string Spselectmaisutilizadasporfuncao = @"SET ROWCOUNT @quant_retorno
                                                                SELECT pc.Des_Palavra_Chave
                                                                    FROM BNE_Vaga_Palavra_Chave vpc
                                                                    inner join BNE_Palavra_Chave pc on vpc.Idf_Palavra_Chave = pc.Idf_Palavra_Chave
                                                                    inner join BNE_Vaga v on vpc.Idf_Vaga = v.Idf_Vaga
                                                                WHERE v.Idf_Funcao = @idf_Funcao
                                                                GROUP BY pc.Des_Palavra_Chave
                                                                ORDER BY COUNT(*) desc
                                                                SET ROWCOUNT 0";

        #region LoadDataReader
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <param name="desPalavraChave">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static IDataReader LoadDataReaderPorDescricao(string desPalavraChave, SqlTransaction trans = null)
        {
            var parms = new List<SqlParameter> {new SqlParameter("@Des_Palavra_Chave", SqlDbType.VarChar, 50)};

            parms[0].Value = desPalavraChave;

            if (trans != null)
                return DataAccessLayer.ExecuteReader(trans, CommandType.Text, Spselectdescricao, parms);

            return DataAccessLayer.ExecuteReader(CommandType.Text, Spselectdescricao, parms);
        }
        #endregion

        #region CarregarPorDescricao
	    /// <summary>
	    /// Método utilizado para retornar uma instância de PalavraChave a partir do banco de dados, dentro de uma transação.
	    /// </summary>
	    /// <param name="desPalavraChave">Chave do registro.</param>
	    /// <param name="objPalavraChave"> </param>
	    /// <param name="trans">Transação existente no banco de dados.</param>
	    /// <returns>Instância de PalavraChave.</returns>
	    /// <remarks>Gieyson Stelmak</remarks>
	    public static bool CarregarPorDescricao(string desPalavraChave, out PalavraChave objPalavraChave, SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReaderPorDescricao(desPalavraChave, trans))
            {
                objPalavraChave = new PalavraChave();
                if (SetInstance(dr, objPalavraChave))
                    return true;

                if (!dr.IsClosed)
                    dr.Close();
            }
            objPalavraChave = null;
            return false;
        }
        #endregion

        #region CarregarMaisUtilizada
        /// <summary>
        /// Método utilizado para retornar uma instância de PalavraChave a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="desPalavraChave">Chave do registro.</param>
        /// <param name="objPalavraChave"> </param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Instância de PalavraChave.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static List<PalavraChave> CarregarMaisUtilizada(int idFuncao, int quantidade, SqlTransaction trans)
        {
            List<PalavraChave> retorno = new List<PalavraChave>();

            var parms = new List<SqlParameter> {    new SqlParameter("@idf_Funcao", SqlDbType.Int),
                                                    new SqlParameter("@quant_retorno", SqlDbType.Int)};
            
            parms[0].Value = idFuncao;
            parms[1].Value = quantidade;

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spselectmaisutilizadasporfuncao, parms))
            {
                while (dr.Read())
                {
                    PalavraChave objPalavraChave;
                    PalavraChave.CarregarPorDescricao(Convert.ToString(dr["Des_Palavra_Chave"]), out objPalavraChave, trans);
                    retorno.Add(objPalavraChave);
                }

                if (!dr.IsClosed)
                    dr.Close();
            }
            
            return retorno;
        }
        #endregion

	}
}