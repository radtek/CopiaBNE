//-- Data: 28/04/2010 08:41
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BNE.BLL
{
    public partial class PalavraProibida // Tabela: BNE_Palavra_Proibida
    {
        #region Consultas

        private const string CARREGARPALAVRASPROIBIDAS = @"DECLARE @SortExpression VARCHAR(200) = 'Des_Palavra_Proibida ASC'
                                                           DECLARE @i INT = (@PageIndex - 1) * @PageSize + 1
                                                           DECLARE @f INT = @PageIndex * @PageSize
                                                           DECLARE @sql VARCHAR(8000)
                                                           DECLARE @sqlcount VARCHAR(8000)

                                                           SET @sql = 'SELECT ROW_NUMBER() OVER (ORDER BY ' + @SortExpression + ' ) AS RowID, * FROM BNE_Palavra_Proibida'

                                                           IF(@DescricaoPalavrasProibidas IS NOT NULL)
                                                           BEGIN
	                                                           SET @sql = @sql + ' WHERE Des_Palavra_Proibida  LIKE ''' + CONVERT(VARCHAR,@DescricaoPalavrasProibidas) + '%'''
                                                           END                                                          

                                                           SET @sqlcount = 'SELECT COUNT(*) FROM ( ' +@sql+ ') as ContadorRows'  

                                                           SET @sql = '
                                                           SELECT * FROM ( '+ @sql +'

                                                           ) AS temp WHERE RowID BETWEEN ' + CONVERT(VARCHAR, @i) + ' AND ' + CONVERT(VARCHAR, @f) 

                                                           EXECUTE (@sql)
                                                           EXECUTE (@sqlcount)";

        private const string DELETEPALAVRASPROIBIDAS = "DELETE BNE_Palavra_Proibida";

        private const string SPBUSCARPALAVRAPORDESCRICAO = "SELECT * FROM BNE_Palavra_Proibida WHERE Des_Palavra_Proibida LIKE + @Des_Palavra_Proibida + '%'";

        private const string SPSELECTDESCRICAO = "SELECT Des_Palavra_Proibida FROM BNE_Palavra_Proibida WHERE Flg_Inativo = 0";

        #endregion

        #region Metodos

        #region ListarPalavrasProibidas
        /// <summary>
        /// Método responsável por retornar uma IDataReader com todas as instâncias de PalavraProibida 
        /// </summary>
        /// <returns></returns>
        public static DataTable CarregarPalavrasProibidas(int pageIndex, int pageSize, string descricaoPalavrasProibidas, out int totalRegistro)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@PageIndex", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@PageSize", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@DescricaoPalavrasProibidas", SqlDbType.VarChar, 255));

            parms[0].Value = pageIndex;
            parms[1].Value = pageSize;
            if (!string.IsNullOrEmpty(descricaoPalavrasProibidas))
                parms[2].Value = descricaoPalavrasProibidas;
            else
                parms[2].Value = DBNull.Value;

            DataSet ds = DataAccessLayer.ExecuteReaderDs(CommandType.Text, CARREGARPALAVRASPROIBIDAS, parms);
            totalRegistro = Convert.ToInt32(ds.Tables[1].Rows[0][0].ToString());

            return ds.Tables[0];
        }

        #endregion

        #region DeletarPalavrasProibidas
        /// <summary>
        /// Deletar todos os registros de Bne_Palavras_Proibidas
        /// </summary>
        public static void DeletarPalavrasProibidas()
        {
            DataAccessLayer.ExecuteNonQuery(CommandType.Text, DELETEPALAVRASPROIBIDAS, null);
        }

        #endregion


        #region BuscarPorDescricaoPalavrasProibidas
        /// <summary>
        /// Método responsável por retornar uma IDataReader com descrições de PalavraProibida de acordo com a busca
        /// </summary>
        /// <returns>DataTable das descrições</returns>
        public static DataTable BuscarPorDescricaoPalavrasProibidas(string descricao)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Des_Palavra_Proibida", SqlDbType.VarChar, 100));
            parms[0].Value = descricao;

            return DataAccessLayer.ExecuteReaderDs(CommandType.Text, SPBUSCARPALAVRAPORDESCRICAO, parms).Tables[0];
        }

        #endregion

        #region ListarPalavrasProibidas
        /// <summary>
        /// Método responsável por retornar uma List com todas as instâncias de PalavraProibida 
        /// </summary>
        /// <returns></returns>
        public static List<string> ListarPalavrasProibidas()
        {
            List<String> lstPalavrasProibidas = new List<String>();

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTDESCRICAO, null))
            {
                while (dr.Read())
                    lstPalavrasProibidas.Add(dr["Des_Palavra_Proibida"].ToString().Trim());

                if (!dr.IsClosed)
                    dr.Close();
            }
            
            return lstPalavrasProibidas;
        }
        #endregion

        #endregion
    }
}
