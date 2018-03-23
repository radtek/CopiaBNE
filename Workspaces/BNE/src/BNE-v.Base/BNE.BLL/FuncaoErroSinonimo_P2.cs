//-- Data: 02/03/2010 09:20
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
    public partial class FuncaoErroSinonimo // Tabela: BNE_Funcao_Erro_Sinonimo
    {
        #region Consultas                                                          

        private const string DELETARFUNCAOERROSINONIMO = @"DELETE BNE_Funcao_Erro_Sinonimo";

        private const string CARREGARFUCAODESCRICAOSINONIMO = @"DECLARE @SortExpression VARCHAR(200) = 'Des_Funcao_Erro_Sinonimo ASC'
                                                                DECLARE @i INT = (@PageIndex - 1) * @PageSize + 1
                                                                DECLARE @f INT = @PageIndex * @PageSize
                                                                DECLARE @sql VARCHAR(8000)
                                                                DECLARE @sqlcount VARCHAR(8000)

                                                                SET @sql = 'SELECT ROW_NUMBER() OVER (ORDER BY ' + @SortExpression + ' ) AS RowID,F.Des_Funcao, FES.* FROM BNE_Funcao_Erro_Sinonimo FES
                                                                JOIN plataforma.TAB_Funcao F ON FES.Idf_Funcao = F.Idf_Funcao
                                                                WHERE 1=1 '
                                                                
                                                                IF(@Flg_Erro IS NOT NULL)
                                                                  BEGIN 
                                                                    SET @sql = @sql +  ' AND FES.Flg_Erro = ' + CONVERT(VARCHAR, @Flg_Erro) + ''  
                                                                  END     
                                                                IF(@Des_Funcao_Erro_Sinonimo IS NOT NULL)
                                                                  BEGIN 
                                                                    SET @sql = @sql + ' AND FES.Des_Funcao_Erro_Sinonimo = ''' + @Des_Funcao_Erro_Sinonimo + ''''    
                                                                  END                                                                     
                                                                IF(@Idf_Funcao IS NOT NULL)
                                                                  BEGIN
                                                                    SET @sql = @sql + ' AND Fes.Idf_Funcao = ' + CONVERT(VARCHAR, @Idf_Funcao) + ''
                                                                  END
                                                                SET @sqlcount = 'SELECT COUNT(*) FROM ( ' +@sql+ ') as temp' 
                                                                SET @sql = '
                                                                SELECT * FROM ( '+ @sql +'
                                                                ) AS temp WHERE RowID BETWEEN ' + CONVERT(VARCHAR, @i) + ' AND ' + CONVERT(VARCHAR, @f) 

                                                                EXECUTE (@sql)
                                                                EXECUTE (@sqlcount)";

        private const string SPSELECTDESCRICAO = @"
        SELECT  FES.*
        FROM    BNE_Funcao_Erro_Sinonimo FES WITH(NOLOCK)
        WHERE   FES.Des_Funcao_Erro_Sinonimo = @Des_Funcao_Erro_Sinonimo
                AND FES.Flg_Inativo = 0";

        #endregion

        #region Metodos

        #region CarregarFuncaoSinonimo
        /// <summary>
        /// Método responsável por retornar um DataTable com todos os registros da tabela BNE_Funcao_Erro_Sinonimo
        /// </summary>
        /// <returns></returns>
        public static DataTable CarregarFuncaoSinonimo(int pageIndex,int PageSize, bool ? flgErro, string descricaoErroSinonimo, int ? idFuncao,out int totalRegistros)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@PageIndex", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@PageSize", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Flg_Erro", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Des_Funcao_Erro_Sinonimo", SqlDbType.VarChar, 255));
            parms.Add(new SqlParameter("@Idf_Funcao", SqlDbType.Int, 4));

            parms[0].Value = pageIndex;
            parms[1].Value = PageSize;
            if (flgErro.HasValue)
                parms[2].Value = flgErro;
            else
                parms[2].Value = DBNull.Value;
            if (!string.IsNullOrEmpty(descricaoErroSinonimo))
                parms[3].Value = descricaoErroSinonimo;
            else
                parms[3].Value = DBNull.Value;
            if (idFuncao.HasValue)
                parms[4].Value = idFuncao;
            else
                parms[4].Value = DBNull.Value;

            DataSet ds = DataAccessLayer.ExecuteReaderDs(CommandType.Text, CARREGARFUCAODESCRICAOSINONIMO, parms);
            totalRegistros = Convert.ToInt32(ds.Tables[1].Rows[0][0].ToString());
            return ds.Tables[0];

        }
        #endregion

        #region DeletarTabelaErroSinonimo
        /// <summary>
        /// Deleta todos os registros da tabela BNE_Funcao_Erro_Sinonimo.
        /// </summary>
        public static void DeletarTabelaErroSinonimo()
        {
            DataAccessLayer.ExecuteNonQuery(CommandType.Text, DELETARFUNCAOERROSINONIMO, null);
        }

        #endregion     

        #region CarregarPorDescricao
        /// <summary>
        /// Carrega um objeto da classe Função através da sua Descrição.
        /// </summary>
        /// <returns>objFuncao</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static bool CarregarPorDescricao(string descricaoFuncao, out FuncaoErroSinonimo objFuncaoErroSinonimo)
        {
            var parms = new List<SqlParameter> {new SqlParameter("@Des_Funcao_Erro_Sinonimo", SqlDbType.VarChar, 255)};
            parms[0].Value = descricaoFuncao;

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTDESCRICAO, parms))
            {
                objFuncaoErroSinonimo = new FuncaoErroSinonimo();
                if (SetInstance(dr, objFuncaoErroSinonimo))
                    return true;
            }
            objFuncaoErroSinonimo = null;
            return false;
        }
        public static FuncaoErroSinonimo CarregarPorDescricao(string descricaoFuncao)
        {
            var parms = new List<SqlParameter> {new SqlParameter("@Des_Funcao_Erro_Sinonimo", SqlDbType.VarChar, 255)};
            parms[0].Value = descricaoFuncao;

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTDESCRICAO, parms))
            {
                var objFuncaoErroSinonimo = new FuncaoErroSinonimo();
                if (SetInstance(dr, objFuncaoErroSinonimo))
                    return objFuncaoErroSinonimo;
            }
            throw (new RecordNotFoundException(typeof(FuncaoErroSinonimo)));
        }
        #endregion

        #endregion

    }
}